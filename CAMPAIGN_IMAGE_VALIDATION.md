# Campaign Image Validation Rules

## Overview
Every campaign in the Athar platform **must** have exactly one image source: either a binary `Image` (byte array) OR an external `ImageUrl` (string URL), but never both and never neither.

## Validation Layers

### 1. **DTO Level Validation**
- `AddCampaignDto` and `UpdatCampaignDto` accept both optional fields:
  - `Image` (IFormFile?) - For manual image uploads
  - `ImageUrl` (string?) - For external image URLs
- `ImageUrl` is validated using `[ValidImageUrl]` attribute to ensure proper HTTP/HTTPS URL format

### 2. **Service Level Validation**
**CampaignService.CreateAsync:**
- ✅ Validates exactly one of `Image` or `ImageUrl` is provided
- ❌ Throws `ArgumentException` if both are null: "Either Image file or ImageUrl must be provided for the campaign."
- ❌ Throws `ArgumentException` if both are provided: "Cannot provide both Image file and ImageUrl. Please provide only one."
- Sets Image bytes to null when ImageUrl is used
- Sets ImageUrl to null when Image is uploaded

**CampaignService.UpdateAsync:**
- If new image data is provided, validates only one source is provided
- When uploading new Image → clears ImageUrl
- When setting new ImageUrl → clears Image
- If neither is provided (keeping existing), validates existing campaign has at least one image source

### 3. **Database Level Validation**
A check constraint is enforced at the database level:
```sql
ALTER TABLE [Campaigns] ADD CONSTRAINT [CK_Campaign_ImageSource] 
CHECK (([Image] IS NOT NULL AND [ImageUrl] IS NULL) OR 
       ([Image] IS NULL AND [ImageUrl] IS NOT NULL));
```

This ensures data integrity even if validation is bypassed at the application level.

### 4. **Import/Scraping Validation**
**Campaign Import Endpoint:**
- Scraped campaigns MUST have `ImageUrl`
- Scraped campaigns have `Image` explicitly set to null
- Campaigns without ImageUrl are skipped during import

## Usage Guidelines

### For Manual Campaign Creation (Charity Admin)
When creating a campaign manually, the charity admin must provide **either**:
- **Option 1:** Upload an image file (`Image` field) - for locally stored images
- **Option 2:** Provide an image URL (`ImageUrl` field) - for externally hosted images

Example request (with uploaded image):
```json
{
  "title": "Food Drive 2025",
  "description": "Help us feed families in need",
  "image": <binary file>,
  "imageUrl": null,
  "duration": 30,
  "goalAmount": 50000,
  "category": "Food",
  "charityID": 5
}
```

Example request (with image URL):
```json
{
  "title": "Food Drive 2025",
  "description": "Help us feed families in need",
  "image": null,
  "imageUrl": "https://example.com/campaign-image.jpg",
  "duration": 30,
  "goalAmount": 50000,
  "category": "Food",
  "charityID": 5
}
```

### For Scraped Campaigns
All scraped campaigns from external sources (like megakheir.org):
- ✅ Must have valid `ImageUrl` pointing to external image
- ✅ Have `Image` set to null (no binary data stored)
- ✅ Are validated during import - campaigns without ImageUrl are skipped

## Frontend Integration

### Displaying Campaign Images
The frontend should handle both image sources:

```typescript
// In your campaign component
function getCampaignImage(campaign: Campaign): string {
  if (campaign.imageUrl) {
    // Scraped campaign - use external URL
    return campaign.imageUrl;
  } else if (campaign.image) {
    // Manual campaign - convert byte array to data URL
    return `data:image/jpeg;base64,${campaign.image}`;
  }
  return '/default-campaign-image.png'; // Fallback
}
```

### Campaign Creation Form
The form should allow users to choose between:
1. **Upload Image** - File input for binary upload
2. **Image URL** - Text input for external URL

Validation should ensure only one option is selected.

## Error Handling

### Application-Level Errors
- **400 Bad Request**: "Either Image file or ImageUrl must be provided for the campaign."
- **400 Bad Request**: "Cannot provide both Image file and ImageUrl. Please provide only one."
- **400 Bad Request**: "ImageUrl must be a valid HTTP or HTTPS URL."

### Database-Level Errors
If the check constraint is violated, EF Core will throw:
```
Microsoft.Data.SqlClient.SqlException: The INSERT statement conflicted with the CHECK constraint "CK_Campaign_ImageSource"
```

## Database Verification

### Check Current Campaigns
Verify all campaigns comply with the constraint:
```sql
-- Should return no rows (all campaigns valid)
SELECT Id, Title, 
       CASE 
         WHEN Image IS NULL AND ImageUrl IS NULL THEN 'Both NULL'
         WHEN Image IS NOT NULL AND ImageUrl IS NOT NULL THEN 'Both NOT NULL'
         ELSE 'Valid'
       END as Status
FROM Campaigns
WHERE (Image IS NULL AND ImageUrl IS NULL) 
   OR (Image IS NOT NULL AND ImageUrl IS NOT NULL);
```

### View Scraped vs Manual Campaigns
```sql
-- Scraped campaigns (should all have ImageUrl, Image = NULL)
SELECT Id, Title, ImageUrl, 
       CASE WHEN Image IS NULL THEN 'NULL' ELSE 'HAS DATA' END as ImageStatus
FROM Campaigns
WHERE ExternalId IS NOT NULL;

-- Manual campaigns (should all have Image, ImageUrl = NULL)
SELECT Id, Title, ImageUrl,
       CASE WHEN Image IS NULL THEN 'NULL' ELSE 'HAS DATA' END as ImageStatus
FROM Campaigns
WHERE ExternalId IS NULL;
```

## Migration Applied
- **Migration Name**: `20251120174006_AddCampaignImageValidation`
- **Applied**: November 20, 2025
- **Constraint**: `CK_Campaign_ImageSource`

## Files Modified
1. **Validators/CampaignImageValidator.cs** - New validator for ImageUrl format
2. **DTOs/AddcampaignDto.cs** - Added ImageUrl field, made Image optional
3. **DTOs/UpdatCampaignDto.cs** - Added ImageUrl field, made Image optional
4. **Services/CampaignService.cs** - Added validation logic in CreateAsync and UpdateAsync
5. **Controllers/CampaignController.cs** - Added validation in Import endpoint
6. **Models/Context.cs** - Added database check constraint
7. **Migrations/20251120174006_AddCampaignImageValidation.cs** - New migration

## Testing Checklist
- [x] ✅ Database constraint applied successfully
- [x] ✅ API builds without errors
- [x] ✅ Existing scraped campaigns (23) have ImageUrl populated
- [ ] ⏳ Test creating campaign with Image only
- [ ] ⏳ Test creating campaign with ImageUrl only
- [ ] ⏳ Test creating campaign with both (should fail)
- [ ] ⏳ Test creating campaign with neither (should fail)
- [ ] ⏳ Test updating campaign image
- [ ] ⏳ Verify frontend displays both scraped and manual campaign images correctly

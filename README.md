# 🌟 Athar — Building Trust & Transparency in Charity

---

## ⚙️ Getting Started

1. **Clone the repository:**
   ```bash
   git clone git clone https://github.com/Omarioooo/Athar.git
   cd athar
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

3. **Run the application:**
   ```bash
   dotnet run
   ```
   
4. **Open your browser:**
   ```bash
   https://localhost:7032
   ```

##  Local Data Seeding (101 charities + 23 campaigns)

- On first run in `Development`, the API seeds the scraped dataset in the background from `AtharPlatform/AtharPlatform/SeedData/`.
- Seeding is idempotent: existing items (by ExternalId or exact name/title) are skipped.
- First run can take ~30–90s: Identity user creation hashes passwords and EF writes 100+ rows.
- To skip seeding (faster startup), set env var before running:

   ```powershell
   $env:SKIP_SCRAPED_SEED="1"; $env:ASPNETCORE_ENVIRONMENT="Development"; dotnet run --project Athar/AtharPlatform/AtharPlatform/AtharPlatform.csproj
   ```

- Manual import is also available via a script that calls the protected import endpoints:

   ```powershell
   # Default paths point to SeedData JSONs tracked in the repo
   powershell -File scripts/import.ps1 -BaseUrl "https://localhost:7032"
   ```

Data files included and tracked:
- `AtharPlatform/AtharPlatform/SeedData/charities101.json`
- `AtharPlatform/AtharPlatform/SeedData/campagins23.json`

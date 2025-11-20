using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AtharPlatform.Validators
{
    /// <summary>
    /// Validates that ImageUrl is a valid HTTP/HTTPS URL format
    /// </summary>
    public class ValidImageUrlAttribute : ValidationAttribute
    {
        private static readonly Regex UrlRegex = new Regex(
            @"^https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                // Null/empty is valid (handled by [Required] if needed)
                return ValidationResult.Success;
            }

            var url = value.ToString()!;
            
            if (!UrlRegex.IsMatch(url))
            {
                return new ValidationResult("ImageUrl must be a valid HTTP or HTTPS URL.");
            }

            return ValidationResult.Success;
        }
    }
}

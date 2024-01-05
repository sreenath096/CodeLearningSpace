using System.ComponentModel.DataAnnotations;

namespace LinkedInPost.CustomAttributes
{
    public class VaildExtensionAttribute : ValidationAttribute
    {
        private readonly string _validExtensions;

        public VaildExtensionAttribute(string validExtensions)
        {
            _validExtensions = validExtensions;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null && file.Length > 1)
            {
                string fileExtension = Path.GetExtension(file.FileName);
                if (!string.Equals(_validExtensions, fileExtension, StringComparison.OrdinalIgnoreCase))
                    return new ValidationResult($"File should have following extension {_validExtensions}. Please recheck the uploaded file.");
            }
            return ValidationResult.Success;
        }
    }
}

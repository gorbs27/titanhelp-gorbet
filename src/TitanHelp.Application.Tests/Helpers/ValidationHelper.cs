using System.ComponentModel.DataAnnotations;

namespace TitanHelp.Application.Tests.Helpers
{
    /// <summary>
    /// Helper class for validation testing
    /// </summary>
    public static class ValidationHelper
    {
        /// <summary>
        /// Validates an object and returns validation results
        /// </summary>
        public static (bool IsValid, List<ValidationResult> Results) ValidateObject(object obj)
        {
            var context = new ValidationContext(obj);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(obj, context, results, true);

            return (isValid, results);
        }

        /// <summary>
        /// Checks if validation failed for a specific property
        /// </summary>
        public static bool HasValidationErrorForProperty(object obj, string propertyName)
        {
            var (_, results) = ValidateObject(obj);
            return results.Any(r => r.MemberNames.Contains(propertyName));
        }

        /// <summary>
        /// Gets the validation error message for a specific property
        /// </summary>
        public static string GetValidationErrorMessage(object obj, string propertyName)
        {
            var (_, results) = ValidateObject(obj);
            var error = results.FirstOrDefault(r => r.MemberNames.Contains(propertyName));
            return error?.ErrorMessage ?? string.Empty;
        }

        /// <summary>
        /// Gets all validation error messages
        /// </summary>
        public static List<string> GetAllValidationErrors(object obj)
        {
            var (_, results) = ValidateObject(obj);
            return results.Select(r => r.ErrorMessage ?? string.Empty).ToList();
        }

        /// <summary>
        /// Validates that a property is required
        /// </summary>
        public static bool PropertyIsRequired(object obj, string propertyName)
        {
            var context = new ValidationContext(obj);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(obj, context, results, true);

            return results.Any(r =>
                r.MemberNames.Contains(propertyName) &&
                r.ErrorMessage?.Contains("required", StringComparison.OrdinalIgnoreCase) == true);
        }

        /// <summary>
        /// Validates that a property has max length constraint
        /// </summary>
        public static bool PropertyHasMaxLength(object obj, string propertyName, int expectedMaxLength)
        {
            var context = new ValidationContext(obj);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(obj, context, results, true);

            return results.Any(r =>
                r.MemberNames.Contains(propertyName) &&
                r.ErrorMessage?.Contains(expectedMaxLength.ToString()) == true);
        }
    }
}
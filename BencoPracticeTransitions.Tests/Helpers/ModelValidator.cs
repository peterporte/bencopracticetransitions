using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BencoPracticeTransitions.Tests.Helpers
{
    public static class ModelValidator
    {
        public static IList<ValidationResult> Validate(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeorgiaTech.Domain.ValueObjects;

namespace GeorgiaTech.Domain.Common
{
    public static class Errors
    {
        public static class General
        {
            public static Error GeneralError(string message) => new Error("general.error", message);
            public static Error ValueIsRequired(string valueName) => new Error("value.is.required", $"Value '{valueName}' is required.");
        }
        public static class Validation
        {
            //public static Error FromValidationRules(List<ValidationFailure> validationFailures) =>
            //  new Error("validation.error", string.Join(", ", validationFailures.Select(v => v.ErrorMessage)));

        }
    }
}

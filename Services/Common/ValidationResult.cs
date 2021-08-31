using System.Collections.Generic;
using System.Linq;

namespace Services.Common
{
    public class ValidationResult
    {
        private readonly List<string> _errors;

        public bool Valid => !_errors.Any();
        public string Errors => string.Join(" | ", _errors);

        private ValidationResult()
        {
            _errors = new List<string>();
        }

        private ValidationResult(string error)
        {
            _errors = new List<string> { error };
        }

        public static ValidationResult ValidResult() => new ValidationResult();
        public static ValidationResult NotValidResult(string error) => new ValidationResult(error);


        public ValidationResult AddError(string error)
        {
            _errors.Add(error);
            return this;
        }
    }
}

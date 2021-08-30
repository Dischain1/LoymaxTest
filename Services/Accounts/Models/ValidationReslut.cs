using System.Collections.Generic;
using System.Linq;

namespace Services.Accounts.Models
{
    public class ValidationReslut
    {
        private readonly List<string> _errors;

        public bool Valid => !_errors.Any();
        public string Errors => string.Join(" | ", _errors);

        protected ValidationReslut()
        {
            _errors = new List<string>();
        }

        public static ValidationReslut Create() => new ValidationReslut();

        public ValidationReslut AddError(string error)
        {
            _errors.Add(error);
            return this;
        }
    }
}

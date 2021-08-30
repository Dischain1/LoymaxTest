using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class ValidationReslut
    {
        private readonly List<string> _errors;

        public bool Valid => !_errors.Any();
        public string Errors => string.Join(" | ", _errors);

        private ValidationReslut()
        {
            _errors = new List<string>();
        }

        private ValidationReslut(string error)
        {
            _errors = new List<string> { error };
        }

        public static ValidationReslut ValidResult() => new ValidationReslut();
        public static ValidationReslut NotValidResult(string error) => new ValidationReslut(error);


        public ValidationReslut AddError(string error)
        {
            _errors.Add(error);
            return this;
        }
    }
}

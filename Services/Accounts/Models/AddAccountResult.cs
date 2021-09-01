using Services.Common;

namespace Services.Accounts.Models
{
    public class AddAccountResult : PostActionResult
    {
        public int? Id { get; }

        private AddAccountResult(int id)
        {
            Id = id;
        }

        private AddAccountResult(string error) : base(error) { }

        public static AddAccountResult FailedResult(string errors)
        {
            return new AddAccountResult(errors);
        }

        public static AddAccountResult SucceededResult(int id)
        {
            return new AddAccountResult(id);
        }
    }
}
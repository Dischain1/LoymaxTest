namespace Services.Common
{
    public abstract class PostActionResult
    {
        public bool Succeeded => string.IsNullOrWhiteSpace(Errors);
        public string Errors { get; }

        protected PostActionResult() { }

        protected PostActionResult(string error)
        {
            Errors = error;
        }
    }
}
namespace Opus.Contracts
{
    public class CreateJobResult
    {
        public bool Success { get; set; }
        public bool IsReferred { get; set; }

        public string[] Errors { get; set; }


        public static CreateJobResult Approved()
        {
            return new CreateJobResult()
            {
                Success = true
            };
        }

        public static CreateJobResult Referred()
        {
            return new CreateJobResult()
            {
                Success = true, // ?
                IsReferred = true
            };
        }

        public static CreateJobResult Declined(string message)
        {
            return new CreateJobResult()
            {
                Success = false,
                Errors = new[] { message }
            };
        }

        public static CreateJobResult Failed(params string[] errors)
        {
            return new CreateJobResult()
            {
                Success = false,
                Errors = errors
            };
        }

    }
}
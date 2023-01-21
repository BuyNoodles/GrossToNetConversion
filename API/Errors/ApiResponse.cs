namespace API.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetMessageStatusCode(statusCode);
        }


        public int StatusCode { get; set; }
        public string Message { get; set; }

        private string GetMessageStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request",
                404 => "Not Found",
                500 => "Internal Server Error",
                _ => null
            };
        }
    }
}

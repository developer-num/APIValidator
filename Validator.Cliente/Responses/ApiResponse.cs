using Validator.Core.CustomEntities;

namespace Validator.Cliente.Responses
{
    public class ApiResponse<T>
    {
        public ApiResponse(T data)
        {
            Succeeded = true;
            Message = string.Empty;
            Status = string.Empty;
            Errors = null;
            Data = data;
        }

        public T Data { get; set; }
        public string Status { get; set; }
        public bool Succeeded { get; set; }
        public string[]? Errors { get; set; }
        public string Message { get; set; }
        public MetaData? Meta { get; set; }
    }
}

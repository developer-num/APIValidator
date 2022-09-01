using Validator.Core.CustomEntities;

namespace Validator.Cliente.Responses
{
    public class ApiResponseError<T>
    {
        public ApiResponseError(T error)
        {
            Errors = error;
        }
        public T Errors { get; set; }

        public Error DetailError { get; set; }
    }
}

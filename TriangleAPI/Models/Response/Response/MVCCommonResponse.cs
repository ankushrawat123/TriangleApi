using Newtonsoft.Json;
using TriangleAPI.Models.Enums;

namespace TriangleAPI.Models.Response.Response
{
    public class MVCCommonResponse<T>
    {
        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("developerMessage")]
        public string DeveloperMessage { get; set; }
        public MVCCommonResponse()
        {

        }
        public MVCCommonResponse<T> Ok(T data)
        {
            this.Data = data;
            this.IsSuccess = true;
            this.Code = (int)ErrorCodes.Success;
            this.Status = ErrorCodes.Success.ToString();
            this.Message = ResponseStatus.Library.GetValueOrDefault(ErrorCodes.Success);
            return this;
        }

        public MVCCommonResponse<T> Fail(ErrorCodes status)
        {
            this.IsSuccess = false;
            this.Status = status.ToString();
            this.Message = ResponseStatus.Library.GetValueOrDefault(status);
            this.Code = (int)status;
            return this;
        }

        public MVCCommonResponse<T> Exception(ErrorCodes status, string developerMessage)
        {
            this.IsSuccess = false;
            this.Status = status.ToString();
            this.Code = (int)status;
            this.Message = ResponseStatus.Library.GetValueOrDefault(status);
            this.DeveloperMessage = developerMessage;
            return this;
        }
    }
}

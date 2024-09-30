using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Application.Helpers.Response
{
    public class ResponseModel<T>
    {
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
        public int? ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string UserErrorMessage { get; set; }

    }

    public static class DynamicResponse<T>
    {
        public static ResponseModel<T> Success()
        {
            return new ResponseModel<T> { IsSuccess = true };
        }
        public static ResponseModel<T> Success(T data)
        {
            return new ResponseModel<T> { IsSuccess = true, Data = data };
        }

        public static ResponseModel<T> Failed()
        {
            return new ResponseModel<T> { IsSuccess = false };
        }
        public static ResponseModel<T> Failed(T data)
        {
            return new ResponseModel<T> { IsSuccess = false, Data = data };
        }
        public static ResponseModel<T> Failed(T data, string errorMessage)
        {
            return new ResponseModel<T> { IsSuccess = false, Data = data, ErrorMessage = errorMessage };
        }
        public static ResponseModel<T> Failed(T data, string errorMessage, int? errorCode)
        {
            return new ResponseModel<T> { IsSuccess = false, Data = data, ErrorMessage = errorMessage, ErrorCode = errorCode };
        }
        public static ResponseModel<T> Failed(string errorMessage, int? errorCode)
        {
            return new ResponseModel<T> { IsSuccess = false, ErrorCode = errorCode, ErrorMessage = errorMessage }; ;
        }
        public static ResponseModel<T> Failed(T data, string errorMessage, int? errorCode, string userErrorMessage)
        {
            return new ResponseModel<T> { IsSuccess = false, Data = data, ErrorMessage = errorMessage, ErrorCode = errorCode, UserErrorMessage = userErrorMessage };
        }
        public static ResponseModel<T> Failed(string errorMessage, int? errorCode, string userErrorMessage)
        {
            return new ResponseModel<T> { IsSuccess = false, ErrorCode = errorCode, ErrorMessage = errorMessage, UserErrorMessage = userErrorMessage }; ;
        }
        public static ResponseModel<T> Failed(string errorMessage)
        {
            return new ResponseModel<T> { IsSuccess = false, ErrorMessage = errorMessage };
        }
    }
}

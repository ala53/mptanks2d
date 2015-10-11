using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Apis.Login.Models
{
    public class ErrorModel : ResponseModelBase
    {
        public static ErrorModel Empty => new ErrorModel();
        public string Message { get; set; }

        private ErrorModel(string msg)
        {
            Message = msg;
        }

        public ErrorModel() { }

        public static implicit operator ErrorModel(string message)
        {
            return new ErrorModel
            {
                IsOk = false,
                Message = message
            };
        }

        public static ErrorModel<T> Of<T>(T data, string message = null)
        {
            var err = (ErrorModel<T>)data;
            err.Message = message;
            return err;
        }

        public static ErrorModel Of(string message) =>
            new ErrorModel(message);
    }

    public class ErrorModel<T> : ResponseModelBase<T>
    {
        public string Message { get; set; }
        public T Data { get; set; }
        private ErrorModel(T data)
        {
            Data = data;
        }
        private ErrorModel(string msg, T data)
        {
            Message = msg;
            Data = data;
        }
        public static implicit operator ErrorModel<T>(T data)
        {
            return new ErrorModel<T>(data);
        }
    }
}

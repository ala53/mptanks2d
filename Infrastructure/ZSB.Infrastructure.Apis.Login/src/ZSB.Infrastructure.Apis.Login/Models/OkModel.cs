using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Apis.Login.Models
{
    public class OkModel : ResponseModelBase
    {
        public static OkModel Empty => new OkModel();
        public string Message { get; set; }
        private OkModel()
        {
            IsOk = true;
        }
        private OkModel(string msg)
        {
            Message = msg;
            IsOk = true;
        }

        public static OkModel<T> Of<T>(T data, string message = null)
        {
            var mdl = (OkModel<T>)data;
            mdl.Message = message;
            return mdl;
        }

        public static OkModel Of(string message)
        {
            return new OkModel(message);
        }
    }

    public class OkModel<T> : ResponseModelBase<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        private OkModel(T data, string message = null)
        {
            Data = data;
            IsOk = true;
        }
        public static implicit operator OkModel<T>(T data)
        {
            return new OkModel<T>(data);
        }
    }

}

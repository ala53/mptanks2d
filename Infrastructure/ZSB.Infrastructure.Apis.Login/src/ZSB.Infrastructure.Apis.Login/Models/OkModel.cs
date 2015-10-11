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

        public static OkModel<T> Of<T>(T data)
        {
            return (OkModel<T>)data;
        }

        public static OkModel Of(string message)
        {
            return new OkModel(message);
        }
    }

    public class OkModel<T> : ResponseModelBase<T>
    {
        public T Data { get; set; }
        private OkModel(T data)
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

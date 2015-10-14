using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ZSB.Infrastructure.Apis.Login.Models
{
    public abstract class ResponseModelBase
    {
        protected bool IsOk { get; set; }
        public string Type { get { return IsOk ? "ok" : "error"; } }
        public static implicit operator ResponseModelBase(Exception data)
        {
#if DEBUG
            return ErrorModel.Of(data);
#else
            return ErrorModel.Of(data.Message);
#endif
        }
    }
    public abstract class ResponseModelBase<T> : ResponseModelBase
    {
        public static implicit operator ResponseModelBase<T>(T data)
        {
            return OkModel.Of(data);
        }
    }
}

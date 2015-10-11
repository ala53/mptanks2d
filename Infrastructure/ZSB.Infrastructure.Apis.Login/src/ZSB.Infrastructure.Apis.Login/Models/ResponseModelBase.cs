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
        [JsonIgnore, XmlIgnore]
        public bool IsOk { get; set; }
        public string Type { get { return IsOk ? "ok" : "error"; } }
    }
    public abstract class ResponseModelBase<T> : ResponseModelBase
    {
    }
}

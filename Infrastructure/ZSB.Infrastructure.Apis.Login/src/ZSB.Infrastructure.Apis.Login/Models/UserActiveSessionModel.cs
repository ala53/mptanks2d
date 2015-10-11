using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ZSB.Infrastructure.Apis.Login.Models
{
    public class UserActiveSessionModel
    {
        [Key]
        public virtual string SessionKey { get; set; }
        public virtual DateTime ExpiryDate { get; set; }
        [JsonIgnore, XmlIgnore]
        public virtual UserModel Owner { get; set; }
    }
}

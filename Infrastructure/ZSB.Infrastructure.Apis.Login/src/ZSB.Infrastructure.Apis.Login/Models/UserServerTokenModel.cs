using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ZSB.Infrastructure.Apis.Login.Models
{
    public class UserServerTokenModel
    {
        [Key]
        public virtual string ServerToken { get; set; }
        public virtual DateTime ExpiryDate { get; set; }
        [Required, XmlIgnore, JsonIgnore]
        public virtual UserModel User { get; set; }
    }
}

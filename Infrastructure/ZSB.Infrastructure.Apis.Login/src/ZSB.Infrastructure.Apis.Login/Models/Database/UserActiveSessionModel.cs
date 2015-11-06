using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ZSB.Infrastructure.Apis.Account.Models
{
    public class UserActiveSessionModel
    {
        [Key]
        public string SessionKey { get; set; }
        public DateTime ExpiryDate { get; set; }
        public virtual UserModel Owner { get; set; }
        public UserActiveSessionModel()
        {

        }
        public UserActiveSessionModel(TimeSpan lifeTime)
        {
            SessionKey = Guid.NewGuid().ToString("N");
            ExpiryDate = DateTime.UtcNow + lifeTime;
        }
    }
}

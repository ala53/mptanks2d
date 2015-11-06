using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ZSB.Infrastructure.Apis.Account.Models
{
    public class UserServerTokenModel
    {
        [Key]
        public string ServerToken { get; set; }
        public DateTime ExpiryDate { get; set; }
        [Required]
        public virtual UserModel Owner { get; set; }

        public UserServerTokenModel()
        {
            ServerToken = Guid.NewGuid().ToString("N");
            ExpiryDate = DateTime.UtcNow.AddMinutes(2);
        }
    }
}

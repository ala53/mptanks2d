using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity.Metadata;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace ZSB.Infrastructure.Apis.Login.Models
{
    public class UserModel
    {
        public DateTime AccountCreationDate { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual string Username { get; set; }
        #region Email confirmation
        public DateTime EmailConfirmationSent { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public Guid EmailConfirmCode { get; set; }
        #endregion
        public bool IsPremiumAccount { get; set; }
        /// <summary>
        /// Keeps several hashes
        /// If the word is "password"
        /// We hash:
        /// password
        /// pASSWORD
        /// Password
        /// 
        /// If it's "PassWORD":
        /// pASSword
        /// PassWORD
        /// password
        /// 
        /// (AKA we copy facebook's design)
        /// </summary>
        public string[] PasswordHashes { get; set; }
        public virtual string PasswordHashesCommaDelimited
        {
            get
            {
                return string.Join(",", PasswordHashes);
            }
            set
            {
                PasswordHashes = value.Split(',');
            }
        }
        public virtual ICollection<UserActiveSessionModel> ActiveSessions { get; set; }
        public virtual ICollection<UserServerTokenModel> ActiveServerTokens { get; set; }
        public virtual Guid UniqueId { get; set; }
    }
}

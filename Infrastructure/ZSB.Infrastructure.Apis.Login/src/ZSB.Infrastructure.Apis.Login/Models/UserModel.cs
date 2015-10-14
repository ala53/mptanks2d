using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity.Metadata;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace ZSB.Infrastructure.Apis.Login.Models
{
    public class UserModel
    {
        public DateTime AccountCreationDate { get; set; }
        public string EmailAddress { get; set; }
        public string Username { get; set; }
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
        public string PasswordHashesCommaDelimited
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
        private List<UserActiveSessionModel> _backingSessions;
        public virtual List<UserActiveSessionModel> ActiveSessions
        {
            get
            {
                return _backingSessions ?? (_backingSessions = new List<UserActiveSessionModel>());
            }
            set
            {
                _backingSessions = value;
            }
        }
        private List<UserServerTokenModel> _backingTokens;
        public virtual List<UserServerTokenModel> ActiveServerTokens
        {
            get
            {
                return _backingTokens ?? (_backingTokens = new List<UserServerTokenModel>());
            }
            set { _backingTokens = value; }
        }
        public Guid UniqueId { get; set; }

        public void AddSession(UserActiveSessionModel mdl)
        {
            mdl.Owner = this;
            ActiveSessions.Add(mdl);
        }
        public void RemoveSession(UserActiveSessionModel mdl)
        {
            ActiveSessions.Remove(mdl);
        }

        public void AddToken(UserServerTokenModel mdl)
        {
            mdl.Owner = this;
            ActiveServerTokens.Add(mdl);
        }
        public void RemoveToken(UserServerTokenModel mdl)
        {
            ActiveServerTokens.Remove(mdl);
        }
    }
}

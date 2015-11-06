using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity.Metadata;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace ZSB.Infrastructure.Apis.Account.Models
{
    public class UserModel
    {
        internal Database.Contexts.LoginDatabaseContext DBContext { get; set; }
        public DateTime AccountCreationDate { get; set; }
        public string EmailAddress { get; set; }
        public string Username { get; set; }
        #region Email confirmation
        public DateTime EmailConfirmationSent { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public Guid UniqueConfirmationCode { get; set; }
        #endregion
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
                return _backingSessions ?? (_backingSessions =
                    DBContext?.Sessions?.Where(
                        a => a.Owner.UniqueId == UniqueId)?.ToList() ??
                    new List<UserActiveSessionModel>());
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
                return _backingTokens ?? (_backingTokens =
                    DBContext?.ServerTokens?.Where(a => a.Owner.UniqueId == UniqueId)?.ToList() ??
                    new List<UserServerTokenModel>());
            }
            set { _backingTokens = value; }
        }
        private List<UserOwnedProductModel> _backingProducts;
        public virtual List<UserOwnedProductModel> OwnedProducts
        {
            get
            {
                return _backingProducts ?? (_backingProducts =
                    DBContext?.OwnedProducts?.Where(a => a.Owner.UniqueId == UniqueId)?.ToList() ??
                    new List<UserOwnedProductModel>());
            }
            set { _backingProducts = value; }
        }
        public Guid UniqueId { get; set; }

        public virtual void AddSession(UserActiveSessionModel mdl)
        {
            mdl.Owner = this;
            ActiveSessions.Add(mdl);
            if (DBContext != null)
                DBContext.Sessions.Add(mdl);

        }
        public virtual void RemoveSession(UserActiveSessionModel mdl)
        {
            ActiveSessions.Remove(mdl);
            if (DBContext != null)
                DBContext.Sessions.Remove(mdl);
        }

        public virtual void AddToken(UserServerTokenModel mdl)
        {
            mdl.Owner = this;
            ActiveServerTokens.Add(mdl);
            if (DBContext != null)
                DBContext.ServerTokens.Add(mdl);
        }
        public virtual void RemoveToken(UserServerTokenModel mdl)
        {
            ActiveServerTokens.Remove(mdl);
            if (DBContext != null)
                DBContext.ServerTokens.Remove(mdl);
        }

        public virtual void AddProduct(UserOwnedProductModel mdl)
        {
            mdl.Owner = this;
            OwnedProducts.Add(mdl);
            if (DBContext != null)
                DBContext.OwnedProducts.Add(mdl);
        }
        public virtual void RemoveProduct(UserOwnedProductModel mdl)
        {
            OwnedProducts.Remove(mdl);
            if (DBContext != null)
                DBContext.OwnedProducts.Remove(mdl);
        }

        public UserModel With(Database.Contexts.LoginDatabaseContext ldb)
        {
            DBContext = ldb;
            return this;
        }
    }
}

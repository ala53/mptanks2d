using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Apis.Login.Database.Contexts
{
    public class LoginDatabaseContext : DbContext
    {
        public DbSet<Models.UserModel> Users { get; set; }
        public DbSet<Models.UserActiveSessionModel> Sessions { get; set; }
        public DbSet<Models.UserServerTokenModel> ServerTokens { get; set; }

        public LoginDatabaseContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.UserModel>()
                .Property(a => a.Username)
                .Required();
            modelBuilder.Entity<Models.UserModel>()
                .Property(a => a.UniqueId)
                .Required().ValueGeneratedOnAdd();
            modelBuilder.Entity<Models.UserModel>()
                .Property(a => a.EmailAddress)
                .Required();
            modelBuilder.Entity<Models.UserModel>()
                .Property(a => a.EmailConfirmCode)
                .Required().ValueGeneratedOnAdd();
            modelBuilder.Entity<Models.UserModel>()
                .Key(a => a.EmailAddress);
            modelBuilder.Entity<Models.UserModel>()
                .Key(a => a.Username);
            modelBuilder.Entity<Models.UserModel>()
                .Key(a => a.UniqueId);

            modelBuilder.Entity<Models.UserModel>()
                .Index(a => a.Username);
            modelBuilder.Entity<Models.UserModel>()
                .Index(a => a.EmailAddress);

            modelBuilder.Entity<Models.UserModel>()
                .Collection(a => a.ActiveSessions)
                .InverseReference(b => b.Owner);
            modelBuilder.Entity<Models.UserModel>()
                .Collection(a => a.ActiveServerTokens)
                .InverseReference(b => b.Owner);

            modelBuilder.Entity<Models.UserActiveSessionModel>()
                .Index(a => a.SessionKey);
            modelBuilder.Entity<Models.UserServerTokenModel>()
                .Index(a => a.ServerToken);

            base.OnModelCreating(modelBuilder);
        }
    }
}

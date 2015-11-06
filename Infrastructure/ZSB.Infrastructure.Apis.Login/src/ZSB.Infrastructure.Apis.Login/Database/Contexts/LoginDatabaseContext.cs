using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Apis.Account.Database.Contexts
{
    public class LoginDatabaseContext : DbContext
    {
        public DbSet<Models.UserModel> Users { get; set; }
        public DbSet<Models.UserActiveSessionModel> Sessions { get; set; }
        public DbSet<Models.UserServerTokenModel> ServerTokens { get; set; }
        public DbSet<Models.UserOwnedProductModel> OwnedProducts { get; set; }

        public LoginDatabaseContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.UserModel>()
                .Property(a => a.Username)
                .IsRequired();
            modelBuilder.Entity<Models.UserModel>()
                .Property(a => a.UniqueId)
                .IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<Models.UserModel>()
                .Property(a => a.EmailAddress)
                .IsRequired();
            modelBuilder.Entity<Models.UserModel>()
                .Property(a => a.UniqueConfirmationCode)
                .IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<Models.UserModel>()
                .HasAlternateKey(a => a.EmailAddress);
            modelBuilder.Entity<Models.UserModel>()
                .HasAlternateKey(a => a.Username);
            modelBuilder.Entity<Models.UserModel>()
                .HasKey(a => a.UniqueId);
            modelBuilder.Entity<Models.UserModel>()
                .Ignore(a => a.DBContext);

            modelBuilder.Entity<Models.UserModel>()
                .Index(a => a.Username);
            modelBuilder.Entity<Models.UserModel>()
                .Index(a => a.EmailAddress);

            modelBuilder.Entity<Models.UserModel>()
                .HasMany(a => a.ActiveSessions)
                .WithOne(b => b.Owner);
            modelBuilder.Entity<Models.UserModel>()
                .HasMany(a => a.ActiveServerTokens)
                .WithOne(b => b.Owner);
            modelBuilder.Entity<Models.UserModel>()
                .HasMany(a => a.OwnedProducts)
                .WithOne(b => b.Owner);

            modelBuilder.Entity<Models.UserActiveSessionModel>()
                .Index(a => a.SessionKey);
            modelBuilder.Entity<Models.UserServerTokenModel>()
                .Index(a => a.ServerToken);

            modelBuilder.Entity<Models.UserOwnedProductModel>()
                .Index(a => a.ProductKey);
            modelBuilder.Entity<Models.UserOwnedProductModel>()
                .HasKey(a => a.ProductKey);
            modelBuilder.Entity<Models.UserOwnedProductModel>()
                .HasAlternateKey(a => a.ProductId);
            modelBuilder.Entity<Models.UserOwnedProductModel>()
                .HasAlternateKey(a => a.EditionId);

            base.OnModelCreating(modelBuilder);
        }
    }
}

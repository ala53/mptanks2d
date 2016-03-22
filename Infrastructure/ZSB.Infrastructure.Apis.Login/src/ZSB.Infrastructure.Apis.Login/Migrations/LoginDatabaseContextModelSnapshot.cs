using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using ZSB.Infrastructure.Apis.Account.Database.Contexts;

namespace ZSB.Infrastructure.Apis.Login.Migrations
{
    [DbContext(typeof(LoginDatabaseContext))]
    partial class LoginDatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Annotation("ProductVersion", "7.0.0-beta8-15964")
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ZSB.Infrastructure.Apis.Account.Models.UserActiveSessionModel", b =>
                {
                    b.Property<string>("SessionKey");

                    b.Property<DateTime>("ExpiryDate");

                    b.Property<Guid?>("OwnerUniqueId");

                    b.HasKey("SessionKey");

                    b.Index("SessionKey");
                });

            modelBuilder.Entity("ZSB.Infrastructure.Apis.Account.Models.UserModel", b =>
                {
                    b.Property<Guid>("UniqueId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AccountCreationDate");

                    b.Property<string>("EmailAddress")
                        .IsRequired();

                    b.Property<DateTime>("EmailConfirmationSent");

                    b.Property<bool>("IsEmailConfirmed");

                    b.Property<string>("PasswordHashesCommaDelimited");

                    b.Property<Guid>("UniqueConfirmationCode")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Username")
                        .IsRequired();

                    b.HasKey("UniqueId");

                    b.Index("EmailAddress")
                        .Unique();

                    b.Index("Username")
                        .Unique();
                });

            modelBuilder.Entity("ZSB.Infrastructure.Apis.Account.Models.UserOwnedProductModel", b =>
                {
                    b.Property<string>("ProductKey");

                    b.Property<Guid>("EditionId");

                    b.Property<Guid?>("OwnerUniqueId");

                    b.Property<Guid>("ProductId");

                    b.Property<DateTime>("RedemptionDate");

                    b.HasKey("ProductKey");

                    b.Index("EditionId");

                    b.Index("ProductId");

                    b.Index("ProductKey");
                });

            modelBuilder.Entity("ZSB.Infrastructure.Apis.Account.Models.UserServerTokenModel", b =>
                {
                    b.Property<string>("ServerToken");

                    b.Property<DateTime>("ExpiryDate");

                    b.Property<Guid?>("OwnerUniqueId")
                        .IsRequired();

                    b.HasKey("ServerToken");

                    b.Index("ServerToken");
                });

            modelBuilder.Entity("ZSB.Infrastructure.Apis.Account.Models.UserActiveSessionModel", b =>
                {
                    b.HasOne("ZSB.Infrastructure.Apis.Account.Models.UserModel")
                        .WithMany()
                        .ForeignKey("OwnerUniqueId");
                });

            modelBuilder.Entity("ZSB.Infrastructure.Apis.Account.Models.UserOwnedProductModel", b =>
                {
                    b.HasOne("ZSB.Infrastructure.Apis.Account.Models.UserModel")
                        .WithMany()
                        .ForeignKey("OwnerUniqueId");
                });

            modelBuilder.Entity("ZSB.Infrastructure.Apis.Account.Models.UserServerTokenModel", b =>
                {
                    b.HasOne("ZSB.Infrastructure.Apis.Account.Models.UserModel")
                        .WithMany()
                        .ForeignKey("OwnerUniqueId");
                });
        }
    }
}

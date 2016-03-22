using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using ZSB.Infrastructure.Apis.Account.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using ZSB.Infrastructure.Apis.Account.Database;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ZSB.Infrastructure.Apis.Account.Controllers
{
    public class RedeemProductController : Controller
    {
        private LoginDB ldb;
        public RedeemProductController(Database.Contexts.LoginDatabaseContext ctx)
        {
            ldb = new LoginDB(ctx);
        }
        [HttpPost, Route("/Product/Redeem")]
        public async Task<ResponseModelBase> RedeemProduct([FromBody]RedeemProductRequestModel model)
        {
            if (!ModelState.IsValid)
                return ErrorModel.Of("invalid_request");
            var user = await ldb.FindBySessionKey(model.SessionKey);

            if (user == null) return ErrorModel.Of("not_logged_in");

            var tableStorageString = Startup.Configuration["Data:TableStorageConnectionString"];
            //update
            var account = CloudStorageAccount.Parse(tableStorageString);
            var client = account.CreateCloudTableClient();
            var table = client.GetTableReference("ZSBProductKeys");

            var partitionKey = model.ProductKey.Substring(0, 4);
            var key = model.ProductKey.ToUpper().Replace("-", "");
            var result = await table.ExecuteAsync(
                TableOperation.Retrieve<ProductKeyStorageModel>(partitionKey, key));

            if (result.HttpStatusCode != 200) //error
                return Models.ErrorModel.Of("product_key_not_found");

            //Do stuff (update the db)
            var obj = (ProductKeyStorageModel)result.Result;

            if (obj.HasBeenRedeemed)
                return ErrorModel.Of("product_key_already_redeemed");

            obj.HasBeenRedeemed = true;
            obj.RedemptionDate = DateTime.UtcNow;
            obj.RedeemerAccountId = user.UniqueId;
            obj.RedeemerAccountEmailAddress = user.EmailAddress;

            //and update the user
            user.OwnedProducts.Add(new UserOwnedProductModel
            {
                EditionId = obj.EditionId,
                ProductId = obj.ProductId,
                ProductKey = obj.Key,
                RedemptionDate = DateTime.UtcNow
            });

            await table.ExecuteAsync(TableOperation.Merge(obj));
            await ldb.UpdateUser(user);

            await Backend.EmailSender.SendEmail(user, Backend.EmailSender.ProductKeyRedeemedTemplate,
                new Dictionary<string, string>()
                {
                    {"-displayName-", obj.DisplayName },
                    {"-productName-", obj.ProductName },
                    {"-productId-", obj.ProductId.ToString() },
                    {"-editionName-", obj.EditionName },
                    {"-editionId-", obj.EditionId.ToString() },
                    {"-productKey-", obj.Key },
                    {"-downloadUrl-", (await Backend.ProductDatabase.GetProduct(obj.ProductId, obj.EditionId)).Product.DownloadUrl },
                    {"-redemptionDate-", DateTime.UtcNow.ToString("G") }
                });

            return OkModel.Of("product_key_redeemed");

        }

        [HttpPost, Route("/Product/Gift/Request")]
        public async Task<ResponseModelBase> RequestGiftProduct([FromBody]GiftProductRequestModel model)
        {
            if (!ModelState.IsValid)
                return ErrorModel.Of("invalid_request");
            var user = await ldb.FindBySessionKey(model.SessionKey);

            if (user == null) return ErrorModel.Of("not_logged_in");

            var otherUser = await ldb.FindByEmailAddress(model.EmailAddressToGiftTo);
            if (otherUser == null) return ErrorModel.Of("user_not_found");

            var productObject = user.OwnedProducts.Where(a => a.ProductKey == model.ProductKeyToGift).FirstOrDefault();
            if (productObject == null)
                return ErrorModel.Of("product_key_not_found");

            await Backend.EmailSender.SendEmail(user, Backend.EmailSender.ProductKeyGiftRequestedTemplate,
                new Dictionary<string, string>()
                {
                    {"-displayName-", productObject.DisplayName },
                    {"-productName-", productObject.ProductName },
                    {"-productId-", productObject.ProductId.ToString() },
                    {"-editionName-", productObject.EditionName },
                    {"-editionId-", productObject.EditionId.ToString() },
                    {"-productKey-", productObject.ProductKey },
                    {"-downloadUrl-", (await Backend.ProductDatabase.GetProduct(
                        productObject.ProductId, productObject.EditionId)).Product.DownloadUrl },
                    {"-redemptionDate-", DateTime.UtcNow.ToString("G") },
                    {"-giftEmailAddress-", model.EmailAddressToGiftTo },
                    {"-giftUsername-", otherUser.Username }
                });

            return OkModel.Of("product_key_gift_validation_sent");

        }
        [HttpGet, Route("/Product/Gift/Confirm/{userId}/{confirmCode}/{productKey}/{addressToGiftTo}")]
        public async Task<ResponseModelBase> ConfirmGiftProduct(Guid userId, Guid confirmCode, string productKey, string addressToGiftTo)
        {
            var user = await ldb.FindByUniqueId(userId);
            var userToGiveTo = await ldb.FindByEmailAddress(addressToGiftTo);

            if (user == null || userToGiveTo == null)
                return ErrorModel.Of("user_not_found");

            if (user.UniqueConfirmationCode != confirmCode)
                return ErrorModel.Of("email_confirmation_code_incorrect");

            var productObject = user.OwnedProducts.Where(a => a.ProductKey == productKey).FirstOrDefault();
            if (productObject == null)
                return ErrorModel.Of("product_key_not_found");

            //Transfer it
            user.OwnedProducts.Remove(productObject);
            userToGiveTo.OwnedProducts.Add(productObject);

            //Transform it into a gift object
            //productObject.EditionId = new Guid("e6c94547-355b-4d39-a5f0-513a3cc3b807");

            user.UniqueConfirmationCode = Guid.NewGuid();
            await ldb.UpdateUser(user);
            await ldb.UpdateUser(userToGiveTo);

            //And send the email
            await Backend.EmailSender.SendEmail(userToGiveTo, Backend.EmailSender.ProductKeyGiftedTemplate,
                new Dictionary<string, string>()
                {
                    {"-giftingUser-", user.Username },
                    {"-displayName-", productObject.DisplayName },
                    {"-productName-", productObject.ProductName },
                    {"-productId-", productObject.ProductId.ToString() },
                    {"-editionName-", productObject.EditionName },
                    {"-editionId-", productObject.EditionId.ToString() },
                    {"-productKey-", productObject.ProductKey },
                    {"-downloadUrl-", (await Backend.ProductDatabase.GetProduct(
                        productObject.ProductId, productObject.EditionId)).Product.DownloadUrl },
                    {"-redemptionDate-", DateTime.UtcNow.ToString("G") }
                });

            return OkModel.Of("product_key_gifted");
        }
    }
}

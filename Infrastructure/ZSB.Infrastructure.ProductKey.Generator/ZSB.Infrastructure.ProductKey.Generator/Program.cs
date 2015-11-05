using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.ProductKey.Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            string allowedCharacters, keyPrefix, productName, productId, editionName, editionId, displayName;

            allowedCharacters = "ABCDFGHJKLMNPQRSTVWXYZ0123456789";
            keyPrefix = "MPPA";
            productName = "MP Tanks 2D Premium Addon";
            productId = "a8e46236-ce79-4480-aeed-28faec2bc0b8";
            editionName = "ZSB Gift";
            editionId = "e6c94547-355b-4d39-a5f0-513a3cc3b807";
            displayName = "MP Tanks 2D Premium Addon (Gifted by ZSB)";

            int keyCount = 1000;

            CloudStorageAccount account = CloudStorageAccount.Parse(
                "DefaultEndpointsProtocol=https;AccountName=zsbstorage;AccountKey=Szr83kIE2xgO+5hbH9j2Z5HK17q62TM6l+NhTLfOfCwbi8Cj1OQgNutteUpr9uc6Ggm6Bya/3o1V1GWIJ9hyAA==;BlobEndpoint=https://zsbstorage.blob.core.windows.net/;TableEndpoint=https://zsbstorage.table.core.windows.net/;QueueEndpoint=https://zsbstorage.queue.core.windows.net/;FileEndpoint=https://zsbstorage.file.core.windows.net/");

            var client = account.CreateCloudTableClient();
            var table = client.GetTableReference("ZSBProductKeys");
            table.CreateIfNotExists();

            var filename = displayName.ToLower().Replace(" ", "_").Replace("(", "").Replace(")", "") + "_keys.csv";
            if (!File.Exists(filename))
                File.AppendAllText(filename,
                    "Unique Key, Created, Key Prefix, Display Name, Product Name, Product ID, Product Edition Name, Product Edition ID, Key Allowed Characters");

            var tsk = new List<Task>();

            int done = 0;
            for (var i = 1; i <= keyCount / 100; i++)
            {
                var batch = new TableBatchOperation();
                for (var j = 0; j < 100; j++)
                {
                    var key = UniqueKeyGenerator.GenerateUniqueKey(keyPrefix,
                        allowedCharacters, 24);
                    var obj = new ProductKeyStorageModel
                    {
                        KeyCreationDate = DateTime.UtcNow,
                        PartitionKey = keyPrefix,
                        Key = key,
                        RowKey = key.Replace("-", ""),
                        Prefix = keyPrefix,
                        EditionId = new Guid(editionId),
                        ProductId = new Guid(productId),
                        ProductName = productName,
                        EditionName = editionName,
                        DisplayName = displayName,
                        HasBeenRedeemed = false,
                        RedeemerAccountEmailAddress = "",
                        RedeemerAccountId = Guid.Empty,
                        RedemptionDate = DateTime.UtcNow,

                    };

                    batch.Add(TableOperation.Insert(obj));

                    File.AppendAllText(filename, "\n" +
                        "\"" + key + "\"," +
                        "\"" + obj.KeyCreationDate.ToShortDateString() + " " + obj.KeyCreationDate.ToShortTimeString() + "\"," +
                        "\"" + obj.Prefix + "\"," +
                        "\"" + obj.DisplayName + "\"," +
                        "\"" + obj.ProductName + "\"," +
                        "\"" + obj.ProductId + "\"," +
                        "\"" + obj.EditionName + "\"," +
                        "\"" + obj.EditionId + "\"," +
                        "\"" + allowedCharacters + "\"");
                }

                table.ExecuteBatch(batch);
                done += 100;
                Console.WriteLine($"{done}/{keyCount} done");
            }

            Task.WaitAll(tsk.ToArray());
            Console.Read();
        }
    }
}

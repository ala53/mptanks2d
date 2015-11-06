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
        static string storageKey = File.ReadAllText("CloudStorageKey.txt");
        static void Main(string[] args)
        {
            if (args.Length == 0)
            { Help(); return; }

            if (args[0] == "generate")
                Generate(args);
            else if (args[0] == "unredeem")
                Unredeem(args);
            else Help();

            Console.WriteLine("Press any key to exit");
            Console.Read();
        }

        public static void Help()
        {
            Console.WriteLine("ZSB Product Key Helper");
            Console.WriteLine("Help...");
            Console.WriteLine("Available Options:");
            Console.WriteLine("\tunredeem KEY-CODE-GOES-HERE - Marks a key code as free and able to be used.");
            Console.WriteLine("\thelp - Show this menu");
            Console.WriteLine("\tgenerate <Count> <Allowed Characters> <Key Prefix> <Product Name> <Product GUID> <Edition Name> <Edition GUID>");
            Console.WriteLine("\t\tE.g. generate 5 ABCDFGH MPTK \"MP Tanks 2D\" guid-here \"1337 Edition\" guid-here");
        }

        public static void Unredeem(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Missing <key> argument!");
                return;
            }

            string key = args[1].ToUpper().Replace("-", "");
            string partition = key.Substring(0, 4); //must be 4 char
            Console.WriteLine("Unredeeming key (marking available) " + key + ". Press any key to continue or CTRL+C to exit.");

            CloudStorageAccount account = CloudStorageAccount.Parse(storageKey);

            var client = account.CreateCloudTableClient();
            var table = client.GetTableReference("ZSBProductKeys");
            table.CreateIfNotExists();

            var result = table.Execute(TableOperation.Retrieve<ProductKeyStorageModel>(partition, key));

            if (result.HttpStatusCode != 200)
            {
                Console.WriteLine("An error occurred:");
                Console.WriteLine(result.Result);
                return;
            }

            var obj = (ProductKeyStorageModel)result.Result;

            obj.HasBeenRedeemed = false;
            obj.RedeemerAccountEmailAddress = "";
            obj.RedeemerAccountId = Guid.Empty;

            table.Execute(TableOperation.Merge(obj));

            Console.WriteLine($"Key {key} unredeemed. It is now available for use.");

        }

        public static void Generate(string[] args)
        {
            string allowedCharacters, keyPrefix, productName, productId, editionName, editionId, displayName;

            if (args.Length < 8)
            {
                Console.WriteLine("Missing arguments!");
                return;
            }

            int keyCount = int.Parse(args[1]);
            allowedCharacters = args[2];
            keyPrefix = args[3];
            productName = args[4];
            productId = args[5];
            editionName = args[6];
            editionId = args[7];
            displayName = productName + " (" + editionName + ")";

            if (keyPrefix.Length != 4)
            {
                Console.WriteLine("KeyPrefix MUST be 4 characters long");
                return;
            }

            Console.WriteLine($"Generating {keyCount} keys. Press enter to continue or press CTRL+C to cancel.");
            Console.Read();

            CloudStorageAccount account = CloudStorageAccount.Parse(storageKey);

            var client = account.CreateCloudTableClient();
            var table = client.GetTableReference("ZSBProductKeys");
            table.CreateIfNotExists();

            var filename = displayName.ToLower().Replace(" ", "_").Replace("(", "").Replace(")", "") + "_keys.csv";
            if (!File.Exists(filename))
                File.AppendAllText(filename,
                    "Unique Key, Created, Key Prefix, Display Name, Product Name, Product ID, Product Edition Name, Product Edition ID, Key Allowed Characters");

            Console.WriteLine("Key list saved to " + filename);

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
        }
    }
}

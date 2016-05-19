using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Inga.Azure
{
    public static class Storage
    {
        private static CloudStorageAccount Account
        {
            get
            {
                var csa = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageAccountConnectionString"));

                return csa;
            }
        }
                  
        private static CloudBlockBlob GetBlob(string container, string filename)
        {
            var storageAccount = Account;
            var blobClient = storageAccount.CreateCloudBlobClient();
            var c = blobClient.GetContainerReference(container);
            var blob = c.GetBlockBlobReference(filename);

            return blob;
        }

        /// <summary>
        /// Put blob.
        /// </summary>                
        public static void PutBlob(string container, string filename, string source)
        {
            var storageAccount = Account;
            var blobClient = storageAccount.CreateCloudBlobClient();
            var c = blobClient.GetContainerReference(container);
            var b = c.GetBlockBlobReference(filename);

            b.Properties.ContentType = MimeMapping.GetMimeMapping(filename);

            using (var fileStream = System.IO.File.OpenRead(source))
            {                
                b.UploadFromStream(fileStream);
            }            
        }

        /// <summary>
        /// Get list of blobs.
        /// </summary>
        public static List<string> GetBlobs(string container, string path)
        {
            var storageAccount = Account;
            var blobClient = storageAccount.CreateCloudBlobClient();
            var c = blobClient.GetContainerReference(container);
            var dir = c.GetDirectoryReference(path);

            var list = dir.ListBlobs();

            return list.OfType<CloudBlockBlob>().Select(b => b.Name.Substring(path.Length + 1)).ToList();
        }


        /// <summary>
        /// Remove blob.
        /// </summary>
        public static void DeleteBlob(string container, string filename)
        {
            var blob = GetBlob(container, filename);

            if (blob.Exists())
                blob.Delete();                
        }
    }
}
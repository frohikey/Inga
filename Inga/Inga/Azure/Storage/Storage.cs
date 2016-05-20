using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inga.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Inga.Azure.Storage
{
    public class Storage
    {
        private readonly CloudStorageAccount _account;

        public Storage(Connection connection)
        {
            _account = CloudStorageAccount.Parse($"DefaultEndpointsProtocol={connection.EndPoint};AccountName={connection.Account};AccountKey={connection.Key}");
        }
                  
        private CloudBlockBlob GetBlob(string container, string filename)
        {
            var storageAccount = _account;
            var blobClient = storageAccount.CreateCloudBlobClient();
            var c = blobClient.GetContainerReference(container);
            var blob = c.GetBlockBlobReference(filename);

            return blob;
        }

        /// <summary>
        /// Put blob.
        /// </summary>                
        public void PutBlob(string container, string filename, string source)
        {
            var storageAccount = _account;
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
        public List<string> GetBlobs(string container, string path)
        {
            var storageAccount = _account;
            var blobClient = storageAccount.CreateCloudBlobClient();
            var c = blobClient.GetContainerReference(container);
            var dir = c.GetDirectoryReference(path);

            var list = dir.ListBlobs();

            return list.OfType<CloudBlockBlob>().Select(b => b.Name.Substring(path.Length + 1)).ToList();
        }

        /// <summary>
        /// Remove blob.
        /// </summary>
        public void DeleteBlob(string container, string filename)
        {
            var blob = GetBlob(container, filename);

            if (blob.Exists())
                blob.Delete();                
        }
    }
}
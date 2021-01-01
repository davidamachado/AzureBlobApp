using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace TestWebAzureApp.Controllers
{
    
    public class BlobController
    {
        private static readonly string container = "images";
        private static readonly string connection = HomeController.Connection;
        private static BlobContainerClient bcc = new BlobContainerClient(connection, container);

        public static void Create()
        {
            //create container if it doesn't already exist
            bcc.CreateIfNotExists(PublicAccessType.Blob);
        }
        public static void Upload(IFormFile formFile)
        {
            //create a new MemoryStream and copy the file within the form onto it
            var ms = new MemoryStream();
            formFile.CopyTo(ms);
            ms.Position = 0;

            //upload file to the container
            bcc.UploadBlob(formFile.FileName, ms);
        }

        public static void Delete(string fileName)
        {
            //delete the blob from the container
            bcc.DeleteBlob(fileName);
        }

        public static bool Duplicate(string fileName)
        {
            bool answer;
            var bbc = new BlobBaseClient(connection, container, fileName);

            //Check if the blob already exists within the container
            if (bbc.Exists())
            {
                answer = true;
            }
            else
            {
                answer = false;
            }

            return answer;
        }

        public static IEnumerable<BlobClient> List()
        {
            foreach (BlobItem blob in bcc.GetBlobs())
            {
                yield return bcc.GetBlobClient(blob.Name);
            }
        }

        public static string GetUri(string fileName)
        {
            var blobClient = new BlobClient(connection, container, fileName);
            return blobClient.Uri.ToString();
        }

        public static string GetSasUri(string fileName)
        {
            var blobClient = new BlobClient(connection, container, fileName);
            var blobServiceClient = blobClient.GetParentBlobContainerClient().GetParentBlobServiceClient();

            UserDelegationKey key = blobServiceClient.GetUserDelegationKey(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(7));

            //Create SAS token valid for 7 days
            var blobSasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = container,
                BlobName = fileName,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.AddDays(7)
            };

            //Specify read/write permissions for SAS
            blobSasBuilder.SetPermissions(BlobSasPermissions.Read | BlobSasPermissions.Write);

            //Add SAS token to blob URI
            var blobUriBuilder = new BlobUriBuilder(blobClient.Uri)
            {
                //Specify a user delegation key
                Sas = blobSasBuilder.ToSasQueryParameters(key, blobServiceClient.AccountName)
            };

            return blobUriBuilder.ToUri().ToString();
        }
    }
}

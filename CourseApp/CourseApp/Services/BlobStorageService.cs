
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CourseApp.Services
{
    public class BlobStorageService
    {
        string accessKey = string.Empty;
        private static IConfiguration configuration;
        public BlobStorageService()
        {
            string connectionString = configuration.GetConnectionString("AccessKey");
            this.accessKey = connectionString;
        }

        public string UploadFileToBlob(string name, byte[] data, string type)
        {
            var uploadTask = Task.Run(() => this.UploadFileToBlobAsync(name, data, type));
            uploadTask.Wait();
            string fileUrl = uploadTask.Result;
            return fileUrl;
        }

        public async void DeleteBlobData(string fileUrl)
        {
            Uri uriObj = new Uri(fileUrl);
            string BlobName = Path.GetFileName(uriObj.LocalPath);

            // Parse the storage account access key
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(accessKey);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            string strContainerName = "mediafiles";
            // Get reference to blob container
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);

            string pathPrefix = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd") + "/";
            CloudBlobDirectory blobDirectory = cloudBlobContainer.GetDirectoryReference(pathPrefix);
           // Get reference to blob
            CloudBlockBlob blockBlob = blobDirectory.GetBlockBlobReference(BlobName);

            //Delete blob from container      
            await blockBlob.DeleteAsync();
        }


        private string GenerateFileName(string fileName)
        {
            string strFileName = string.Empty;
            string[] strName = fileName.Split('.');
            strFileName = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd") + "/" + DateTime.Now.ToUniversalTime().ToString("yyyyMMdd\\THHmmssfff") + "." + strName[strName.Length - 1];
            return strFileName;
        }

        private async Task<string> UploadFileToBlobAsync(string fileName, byte[] fileData, string fileMimeType)
        {

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(accessKey);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            string strContainerName = "mediafiles";
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);
            fileName = this.GenerateFileName(fileName);

            if (await cloudBlobContainer.CreateIfNotExistsAsync())
            {
                await cloudBlobContainer.SetPermissionsAsync(   new BlobContainerPermissions{ 
                    PublicAccess = BlobContainerPublicAccessType.Blob 
                });
            }

            if (fileName != null && fileData != null)
            {
                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
                cloudBlockBlob.Properties.ContentType = fileMimeType;
                await cloudBlockBlob.UploadFromByteArrayAsync(fileData, 0, fileData.Length);
                return cloudBlockBlob.Uri.AbsoluteUri;
            }
            return "";
        }
    }
}



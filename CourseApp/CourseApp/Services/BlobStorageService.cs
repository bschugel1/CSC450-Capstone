
using CourseApp.Models.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CourseApp.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private BlobStorageSettings _settings;
        public BlobStorageService(IOptions<BlobStorageSettings> settings)
        {
            _settings = settings.Value;
        }

        public string UploadFileToBlob(string namePath, byte[] data, string type)
        {
            var uploadTask = Task.Run(() => this.UploadFileToBlobAsync(namePath, data, type));
            uploadTask.Wait();
            string fileUrl = uploadTask.Result;
            return fileUrl;
        }
        public Stream DownloadFileFromBlob(string namePath, string type)
        {
            var downloadTask = Task.Run(() => this.DownloadFileFromBlobAsync(namePath, type));
            downloadTask.Wait();
            var fileStream = downloadTask.Result;
            return fileStream;
        }
        public async void DeleteBlobData(string fileUrl)
        {
            Uri uriObj = new Uri(fileUrl);
            string BlobName = Path.GetFileName(uriObj.LocalPath);

            // Parse the storage account access key
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_settings.ConnectionString);
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

        private async Task<string> UploadFileToBlobAsync(string fileNamePath, byte[] fileData, string fileMimeType)
        {
            if (fileData == null || fileNamePath == null)
            {
                throw new Exception("The Data provided was NULL");
            }

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_settings.ConnectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            string strContainerName = "mediafiles";
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);


            if (await cloudBlobContainer.CreateIfNotExistsAsync())
            {
                await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
            }
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileNamePath);
            cloudBlockBlob.Properties.ContentType = fileMimeType;
            await cloudBlockBlob.UploadFromByteArrayAsync(fileData, 0, fileData.Length);
            return cloudBlockBlob.Uri.AbsoluteUri;
        }

        private async Task<Stream> DownloadFileFromBlobAsync(string namePath, string type)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_settings.ConnectionString);

            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            string strContainerName = "mediafiles";
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);

            if (await cloudBlobContainer.CreateIfNotExistsAsync())
            {
                await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
            }
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(namePath);
            return await cloudBlockBlob.OpenReadAsync();
        }

        public CloudBlobDirectory GetDirectory(string namePath)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_settings.ConnectionString);

            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            string strContainerName = "mediafiles";
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);

            CloudBlobDirectory directory = cloudBlobContainer.GetDirectoryReference(namePath);
            return directory;
        }
    }
}



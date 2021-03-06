﻿
using CourseApp.Configuration;
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
            Uri relativeUri = new Uri(fileUrl, UriKind.Relative);
            string BlobName = Path.GetFileName(relativeUri.ToString());

            // Parse the storage account access key
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_settings.ConnectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            string strContainerName = _settings.BlobStorageContainer;
            // Get reference to blob container
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);
            CloudBlobDirectory blobDirectory = cloudBlobContainer.GetDirectoryReference("");
            // Get reference to blob
            CloudBlockBlob blockBlob = blobDirectory.GetBlockBlobReference(fileUrl);
            //Delete blob from container      
            await blockBlob.DeleteAsync();
        }

        private async Task<string> UploadFileToBlobAsync(string fileNamePath, byte[] fileData, string fileMimeType)
        {
            if (fileData == null || fileNamePath == null)
            {
                throw new Exception("The Data provided was NULL");
            }

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_settings.ConnectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            string strContainerName = _settings.BlobStorageContainer;
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
            // Blob container for mediafiles
            string strContainerName = _settings.BlobStorageContainer;
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
            string strContainerName = _settings.BlobStorageContainer;
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);
            CloudBlobDirectory directory = cloudBlobContainer.GetDirectoryReference(namePath);
            return directory;
        }
    }
}



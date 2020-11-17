using System.IO;

namespace CourseApp.Services
{
    public interface IBlobStorageService
    {
        void DeleteBlobData(string fileUrl);
        string UploadFileToBlob(string namePath, byte[] data, string type);
        Stream DownloadFileFromBlob(string namePath, string type);
    }
}
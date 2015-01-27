using System.IO;
using Portal.Model;
using Portal.Services.Clients.ServiceModel;
using Portal.Services.Contracts;
using File = Portal.Model.File;

namespace Portal.Services.Clients
{
    public class FileServiceClient : IFileService
    {
        private readonly ServiceClient<IFileServiceChannel> _fileService = new ServiceClient<IFileServiceChannel>();

        public Model.FileInfo GetFileInfo(FileRequest fileRequest)
        {
            var proxy = _fileService.CreateProxy();
            return proxy.GetFileInfo(fileRequest);
        }

        public Model.FileInfo UploadFile(File file)
        {
            var proxy = _fileService.CreateProxy();
            return proxy.UploadFile(file);
        }

        public void DeleteFile(FileRequest fileRequest)
        {
            var proxy = _fileService.CreateProxy();
            proxy.DeleteFile(fileRequest);
        }

        public File DownloadFile(FileRequest fileRequest)
        {
            var proxy = _fileService.CreateProxy();
            return proxy.DownloadFile(fileRequest);
        }
    }
}
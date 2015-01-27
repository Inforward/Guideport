using System.IO;
using Portal.Model;
using System.ServiceModel;
using File = Portal.Model.File;

namespace Portal.Services.Contracts
{
    public interface IFileServiceChannel : IFileService, IClientChannel { }

    [ServiceContract(Namespace = "http://guideport.firstallied.com")]
    public interface IFileService
    {
        [OperationContract]
        Model.FileInfo GetFileInfo(FileRequest fileRequest);

        [OperationContract]
        Model.FileInfo UploadFile(File file);

        [OperationContract]
        void DeleteFile(FileRequest fileRequest);

        [OperationContract]
        File DownloadFile(FileRequest fileRequest);
    }
}

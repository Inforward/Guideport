using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Data;
using Portal.Infrastructure.Helpers;
using Portal.Model;
using Portal.Services.Contracts;

namespace Portal.Domain.Services
{
    public class FileService : IFileService
    {
       private readonly IFileRepository _fileRepository;

       public FileService(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public Model.FileInfo GetFileInfo(FileRequest fileRequest)
        {
            return _fileRepository.GetFileInfo(fileRequest.FileID);
        }


        public Model.FileInfo UploadFile(Model.File file)
        {
            var sourceStream = file.Stream;
            var buffer = new Byte[FileHelper.DefaultBufferSize];

            using (var memoryStream = new MemoryStream())
            {
                int read;

                while ((read = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    memoryStream.Write(buffer, 0, read);
                }

                file.Data = memoryStream.ToArray();

                memoryStream.Close();
                sourceStream.Close();
            }

            _fileRepository.Add(file);
            _fileRepository.Save();

            return file.Info;
        }

        public void DeleteFile(FileRequest fileRequest)
        {
            _fileRepository.DeleteFile(fileRequest.FileID);
        }

        public Model.File DownloadFile(FileRequest fileRequest)
        {
            var file = _fileRepository.GetFile(fileRequest.FileID);
            
            if (file == null)
                throw new Exception("Unable able to find file.  FileID: " + fileRequest.FileID);

            file.Length = file.Data.Length;
            file.Stream = new MemoryStream(file.Data);
            file.Data = null;

            return file;
        }
    }
}

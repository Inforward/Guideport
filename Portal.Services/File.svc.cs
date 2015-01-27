using Portal.Data;
using Portal.Domain.Services;

namespace Portal.Services
{
    public class File : FileService
    {
        public File(IFileRepository fileRepository)
            : base(fileRepository)
        { }
    }
}

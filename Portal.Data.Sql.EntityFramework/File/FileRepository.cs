using System.Data.Entity;
using System.IO;
using System.Linq;
using File = Portal.Model.File;

namespace Portal.Data.Sql.EntityFramework
{
    public class FileRepository : EntityRepository<MasterContext>, IFileRepository
    {
        public Model.FileInfo GetFileInfo(int fileID)
        {
            return FindBy<Model.FileInfo>(f => f.FileID == fileID).FirstOrDefault();
        }

        public void CreateFileInfo(Model.FileInfo fileInfo)
        {
            Add(fileInfo);
            Save();
        }

        public void DeleteFile(int fileID)
        {
            var fileInfo = GetFileInfo(fileID);

            if (fileInfo == null)
                return;

            Delete(fileInfo);

            var file = FindBy<File>(f => f.FileID == fileID).FirstOrDefault();

            if (file != null)
                Delete(file);

            Save();
        }

        public File GetFile(int fileID)
        {
            return FindBy<File>(fc => fc.FileID == fileID)
                        .Include(f => f.Info)
                        .FirstOrDefault();
        }
    }
}

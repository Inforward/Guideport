using Portal.Model;

namespace Portal.Data
{
    public interface IFileRepository : IEntityRepository
    {
        FileInfo GetFileInfo(int fileID);
        void CreateFileInfo(FileInfo fileInfo);
        void DeleteFile(int fileID);
        File GetFile(int fileID);
    }
}

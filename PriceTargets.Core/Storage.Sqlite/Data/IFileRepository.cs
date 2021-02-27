

using HamstersRocket.Core.Storage.Sqlite.Model;

namespace HamstersRocket.Core.Storage.Sqlite.Data
{
    public interface IFileRepository
    {
        long SaveFile(string source);

        //  void DeleteFile(long id);
        FileEntry GetFile(string[] tags);

        void AddTags(long id, string[] tags);
        //  void DeleteTag(long id, params string[] tags);
        //  void DeleteTag(long id);
        //  string [] GetAllTags(long id, params string[] tags);

    }
}
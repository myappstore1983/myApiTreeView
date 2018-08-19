using myApiTreeView.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace myApiTreeView.Services
{
    public interface IFolderService
    {
        Task<List<Folder>> GetRootFolders();
        List<Folder> GetAllFolders(List<Folder> list);
        Task<Folder> GetFolderById(int folderId);
        void AddFolder(Folder folder);
        void DeleteFolder(Folder folder);
    }
}
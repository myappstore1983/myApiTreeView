using myApiTreeView.Models;
using myApiTreeView.API.Dtos;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace myApiTreeView.Services
{
    
    public interface IFolderService
    {
         void AddFolder(Folder folder);
    
         Task<Folder> GetFolder(int? parentFolderId);

          Task<List<Folder>> GetRootFolders();

         List<Folder> GetAllFolders(List<Folder> list,ref List<TestCase> testcases);

    }
}
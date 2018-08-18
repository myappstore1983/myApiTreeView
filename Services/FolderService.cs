
using System.Collections.Generic;
using System.Threading.Tasks;
using myApiTreeView.API.Data;
using myApiTreeView.API.Dtos;
using myApiTreeView.Models;

namespace myApiTreeView.Services
{

    public class FolderService : IFolderService
    {
        private readonly IDataRepo _repo;

        public FolderService(IDataRepo repo)
        {
            _repo = repo;
        }

        public void AddFolder(Folder folder)
        {
               Folder folderObject = new Folder();
                if(folder.ParentFolderId == 0)
                {
                    folderObject.FolderId =  folder.FolderId;
                    folderObject.Name = $"Root"+folder.ParentFolderId;
                    _repo.Add(folderObject);
                    _repo.SaveAll();
                } 
                else
                {
                    Folder parentFolderObject = _repo.GetFolder(folder.ParentFolderId).Result;
                    folderObject.Name = folder.Name;
                    folderObject.FolderId = folder.FolderId;
                    parentFolderObject.SubFolders.Add(folderObject);
                   _repo.SaveAll();
                       
             }
        }

        public List<Folder> GetAllFolders(List<Folder> foldersList,ref List<TestCase> testcases)
        {
            return _repo.GetAllFolders(foldersList,ref testcases);
        }

        public Task<Folder> GetFolder(int? parentFolderId)
        {
           return _repo.GetFolder(parentFolderId);
        }

        public Task<List<Folder>> GetRootFolders()
        {
            return _repo.GetRootFolders();
        }
    }
}


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

        public Task<List<Folder>> GetRootFolders()
        {
            return _repo.GetRootFolders();
        }

        public List<Folder> GetAllFolders(List<Folder> foldersList)
        {
            List<TestCase> testcases = new List<TestCase>();
            return _repo.GetAllFolders(foldersList, ref testcases);
        }
        public void DeleteFolder(Folder folder)
        {
            _repo.Delete<Folder>(folder);
            _repo.SaveAll();
        }

        public Task<Folder> GetFolderById(int folderId)
        {
            return _repo.GetFolderById(folderId);
        }

        public void AddFolder(Folder folder)
        {
            Folder parentFolderObject = _repo.GetFolderById(folder.ParentFolderId).Result;
            Folder folderObject = _repo.GetFolderById(folder.FolderId).Result;
            //if ( parentFolderObject != null) // Folder aleady exists with this FolderId, Skip this record
            //{
            //    _repo.Add(folder);
            //    _repo.SaveAll();
            //}
            
                if ((folderObject == null && parentFolderObject != null) || folder.FolderId == 0 )
                {
                    _repo.Add(folder);
                    _repo.SaveAll();
                }
                else
                {
                    parentFolderObject.SubFolders.Add(folder);
                    _repo.SaveAll();

                }
            
        }

    }
}

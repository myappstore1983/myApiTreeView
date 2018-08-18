using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using myApiTreeView.Models;

namespace myApiTreeView.API.Data
{
    public class DataRepo : IDataRepo
    {
        private readonly DataContext _context;
        public DataRepo(DataContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
             _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
             _context.Remove(entity);
        }

         public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }      

        public async Task<Folder> GetFolder(int? parentFolderId)
        {
             return await  _context.folders.Include(x => x.SubFolders)
                            .Where(x => x.FolderId == parentFolderId).FirstOrDefaultAsync();
                              
        }

        public  List<Folder> GetAllFolders(List<Folder> folders,ref List<TestCase> testcases)
        {
             int i = 0;
            List<Folder> foldersList = new List<Folder>();
           

            if(folders.Count > 0)
            {
                foldersList.AddRange(folders);
            }

            foreach(Folder x in folders)
            {
                Folder folder = _context.folders.Include(y => y.SubFolders).Include(t => t.TestCases)
                                .Where(f => f.FolderId == x.FolderId)
                                .Select(f => new Folder { FolderId = f.FolderId, Name = f.Name, ParentFolderId = f.ParentFolderId, SubFolders = f.SubFolders,TestCases = f.TestCases }).First();
                if(folder.SubFolders == null)
                {
                    i++;
                    continue;
                }

                if (folder.TestCases.Count > 0)
                {
                    testcases.AddRange(folder.TestCases);
                }
                List<Folder> subfolder = folder.SubFolders.ToList();
                folder.SubFolders = GetAllFolders(subfolder,ref testcases);
                foldersList[i] = folder;
                i++;
            }
            
           
            return   foldersList;
        }

        public Task<List<Folder>> GetRootFolders()
        {
           return _context.folders.Include(x => x.SubFolders)
                                .Where(x => x.ParentFolderId == null)
                                .Select(f => new Folder { FolderId = f.FolderId, Name = f.Name, 
                                ParentFolderId = f.ParentFolderId, SubFolders = f.SubFolders }).ToListAsync();
        }
        public async Task<TestCase> GetTestCase(int testCaseId)
        {
           return await  _context.testCases.Where(x => x.TestCaseId == testCaseId).FirstOrDefaultAsync(); 
        }


        // public  List<Folder> GetAllFolders(List<Folder> list)
        // {
        //      int z = 0;
        //     List<Folder> lists = new List<Folder>();

        //     if(list.Count > 0)
        //     {
        //         lists.AddRange(list);
        //     }

        //     foreach(Folder x in list)
        //     {
        //         Folder folder = _context.folders.Include(y => y.SubFolders).Include(t => t.TestCases)
        //                         .Where(y => y.FolderId == x.FolderId)
        //                         .Select(y => new Folder { FolderId = y.FolderId, Name = y.Name, ParentFolderId = y.ParentFolderId, SubFolders = y.SubFolders,TestCases = y.TestCases }).First();
        //         if(folder.SubFolders == null)
        //         {
        //             z++;
        //             continue;
        //         }

        //         List<Folder> subfolder = folder.SubFolders.ToList();
        //         folder.SubFolders = GetAllFolders(subfolder);
        //         lists[z] = folder;
        //         z++;
        //     }
        //     return   lists;
        // }

       

    }
}
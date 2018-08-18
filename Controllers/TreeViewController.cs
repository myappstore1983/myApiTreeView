using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using myApiTreeView.API.Dtos;
using myApiTreeView.API.Data;
using myApiTreeView.Models;
using Newtonsoft.Json;
using myApiTreeView.Services;

namespace myApiTreeView.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TreeViewController : ControllerBase
    {
        private IFolderService _folderService = null;
        private ITestCaseService _testCaseService = null;
        public TreeViewController(IFolderService folderService,ITestCaseService testCaseService)
        {
             _folderService = folderService;
             _testCaseService = testCaseService;
        }
      
        [HttpGet]
        public ActionResult<IEnumerable<FolderDto>> Get()
        {
            List<Folder> rootFolders = _folderService.GetRootFolders().Result;
            return Content(JsonConvert.SerializeObject(
                new { 
                    TreeView = _folderService.GetAllFolders(rootFolders) },Formatting.Indented),"application/json"
                    );
        } 
     
        [HttpPost("Add")]
        [ProducesResponseType(201, Type = typeof(TestCase))]
        [ProducesResponseType(400)]
        public void AddTestCase([FromBody] TestCase testCase)
        {     
           _testCaseService.AddTestCase(testCase);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        public void DeleteTestCase(int id)
        {
            var testcase = _testCaseService.GetTestCase(id).Result;
           _testCaseService.DeleteTestCase(testcase);
        }

     #region "Unused methods"
        // public IActionResult AddFolder()
        // {
        //     Folder folder  = new Folder();
        //     folder.ParentFolderId =4;
        //     if (ModelState.IsValid)
        //     {
        //         try
        //         {
        //             Folder folderObject = new Folder();
        //             if(folder.ParentFolderId == 0)
        //             {
        //                 folderObject.Name = $"Root"+folder.ParentFolderId;
        //                 _repo.Add(folderObject);
        //                 _repo.SaveAll();
        //             } else
        //             {
        //                 Folder parentFolderObject = _repo.GetFolder(folder.ParentFolderId).Result;
        //                 folderObject.Name = $"Folder"+folder.ParentFolderId;;
        //                 parentFolderObject.SubFolders.Add(folderObject);
        //                 _repo.SaveAll();
                       
        //             }
        //             return Ok(folderObject);
        //         }catch(Exception ex)
        //         {
        //             return BadRequest(new { error = ex.Message });
        //         }
        //     }else
        //     {
        //         return StatusCode(500, new { error = ModelState.Values.SelectMany(x => x.Errors).ToList() });
        //     }
        // }

        // public IActionResult AddFolder1()
        // {
        //     DtoTestCase testCase  = new DtoTestCase();
        //     Folder folder  = _repo.GetFolder(testCase.FolderId).Result;
        //     folder.ParentFolderId =4;
        //     Folder folderObject = new Folder();
        //     if(folder == null)
        //         {
        //           folderObject.Name = $"Root"+folder.ParentFolderId;
        //           _repo.Add(folderObject);
        //           _repo.SaveAll();
        //         } else
        //         {
        //             Folder parentFolderObject = _repo.GetFolder(folder.ParentFolderId).Result;
        //             folderObject.Name = $"Folder"+folder.ParentFolderId;;
        //             parentFolderObject.SubFolders.Add(folderObject);
        //             _repo.SaveAll();                     
        //         }
        //         return Ok(folderObject);
        // }
         #endregion
    }
}

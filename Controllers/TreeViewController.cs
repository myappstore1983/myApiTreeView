using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using myApiTreeView.API.Dtos;
using myApiTreeView.Models;
using myApiTreeView.Services;
using System.Collections.Generic;
using System.Linq;
using Swashbuckle.AspNetCore.Swagger;
using System.Net;
using System.Threading.Tasks;

namespace myApiTreeView.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TreeViewController : ControllerBase
    {
        private IFolderService _folderService = null;
        private ITestCaseService _testCaseService = null;

        public TreeViewController(IFolderService folderService, ITestCaseService testCaseService)
        {
            _folderService = folderService;
            _testCaseService = testCaseService;
        }

        //[HttpGet]
        //public ActionResult<IEnumerable<FolderDto>> Get()
        //{
        //    List<TestCase> testcases = new List<TestCase>();
        //    List<Folder> rootFolders = _folderService.GetRootFolders().Result;
        //    _folderService.GetAllFolders(rootFolders);
        //    return Ok(rootFolders);
        //}

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<TestCaseDto>>> GetTestCasesInsideFolder(int id)
        {
            var testCasesResult = await _testCaseService.GetTestCases(id);
            if (testCasesResult == null || testCasesResult.Count == 0)
            {
                return NotFound();
            }
           
            return Ok(testCasesResult);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TestCaseDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddTestCase([FromBody]TestCaseDto testCaseDto)
        {
            if (ModelState.IsValid)
            {
                var isSuccess = await _testCaseService.AddTestCase(testCaseDto);
                if(isSuccess)
                    return StatusCode((int)HttpStatusCode.Created);
                else
                    return StatusCode((int)HttpStatusCode.BadRequest);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { error = ModelState.Values.SelectMany(x => x.Errors).ToList() });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(202)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteTestCase(int id)
        {
            var testcase = _testCaseService.GetTestCase(id);

            if (testcase.Result == null)
                return NotFound();

            await _testCaseService.DeleteTestCase(testcase.Result);

            return StatusCode((int)HttpStatusCode.Accepted);
        }

        //[HttpDelete("{folderId}")]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(202)]
        //[ProducesResponseType(404)]
        //public IActionResult DeleteFolder(int folderId)
        //{
        //    var folder = _folderService.GetFolderById(folderId).Result;

        //    if (folder == null)
        //        return NotFound();

        //    _folderService.DeleteFolder(folder);

        //    return StatusCode(202);
        //}

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

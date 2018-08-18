using myApiTreeView.Models;
using myApiTreeView.API.Dtos;
using System.Threading.Tasks;
using System.Collections.Generic;
using myApiTreeView.API.Data;
using System.Linq;

namespace myApiTreeView.Services
{
    public class TestCaseService : ITestCaseService
    {
       private readonly IDataRepo _repo;

        public TestCaseService(IDataRepo repo)
        {
            _repo = repo;
        }

        public void AddTestCase(TestCase testcase)
        {
            _repo.Add<TestCase>(testcase);
            _repo.SaveAll();
        }

        public void DeleteTestCase(TestCase testcase)
        {
            _repo.Delete<TestCase>(testcase);
            _repo.SaveAll();
        }

        public async Task<TestCase> GetTestCase(int testCaseId)
        {            
             return await _repo.GetTestCase(testCaseId);                             
        }   
    }
}

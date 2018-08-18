using System.Threading.Tasks;
using myApiTreeView.Models;

namespace myApiTreeView.Services
{
    public interface ITestCaseService
    {
        Task<TestCase> GetTestCase(int testCaseId);

        void AddTestCase(TestCase testcase);

        void DeleteTestCase(TestCase testcase);
    }
}
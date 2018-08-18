using System.ComponentModel.DataAnnotations;

namespace myApiTreeView.API.Dtos
{
    public class DtoTestCase
    {
        public DtoTestCase()
        {
            FolderId = 0;
        }
        public int TestCaseId { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }
        public int StepCount { get; set; }
        public int? FolderId { get; set; }

    }
}
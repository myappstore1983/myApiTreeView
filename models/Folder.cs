using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace myApiTreeView.Models
{
    public class Folder
    {
        public Folder()
        {
            SubFolders = new HashSet<Folder>();
            TestCases = new HashSet<TestCase>();
        }
         
        [Key]
        // [Column("Id")]  
        public int FolderId { get; set; }

        public string Name { get; set; }

        public int? ParentFolderId { get; set; }

        public ICollection<TestCase> TestCases {get;set;}

        public virtual Folder ParentFolder { get; set; }

        public virtual ICollection<Folder> SubFolders { get; set; }
    }
}
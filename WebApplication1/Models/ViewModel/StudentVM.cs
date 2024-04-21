using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.ViewModel
{
    public class StudentVM
    {
        public StudentVM()
        {
            this.SkillList = new List<int>();
        }
        public int StudentId { get; set; }
        [Required, StringLength(50), Display(Name = "Student Name")]
        public string StudentName { get; set; }
        public int Age { get; set; }
        [Required, Column(TypeName = "date"), Display(Name = "Date Of Birth"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }
        public string? Image { get; set; }
        public IFormFile? ImageFile { get; set; }
        public bool MaritalStatus { get; set; }
        public virtual List<int> SkillList { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        [Required, StringLength(50), Display(Name = "Student Name")]
        public string StudentName { get; set; }
        public int Age { get; set; }
        [Required, Column(TypeName ="date"), Display(Name = "Date Of Birth"), DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}", ApplyFormatInEditMode =true)]
        public DateTime DateOfBirth { get; set; }
        public string Image { get; set; }
        public bool MaritalStatus { get; set; }
        public virtual ICollection<StudentSkill>StudentSkills { get; set; } = new List<StudentSkill>();
    }
}

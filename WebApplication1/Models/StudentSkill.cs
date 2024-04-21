using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class StudentSkill
    {
        public int StudentSkillId { get; set; }
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        [ForeignKey("Skill")]
        public int SkillId { get; set; }
        public virtual Student? Student { get; set; }
        public virtual Skill? Skill { get; set; }
    }
}

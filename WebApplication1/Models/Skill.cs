namespace WebApplication1.Models
{
    public class Skill
    {
        public int SkillId { get; set; }
        public string SkillName { get; set; }
        public virtual ICollection<StudentSkill> StudentSkills { get; set; } = new List<StudentSkill>();
    }
}

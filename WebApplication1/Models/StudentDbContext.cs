using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class StudentDbContext:DbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext>options): base(options)
        {
            
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<StudentSkill> StudentSkills { get; set; }

    }
}

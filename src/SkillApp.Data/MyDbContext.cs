using Microsoft.EntityFrameworkCore;

using SkillApp.Entities.Entities;

namespace SkillApp.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext()
            : base()
        {                       
        }

        public DbSet<Player> Players { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(LocalDb)\v11.0;Database=MyDatabase;Trusted_Connection=True;");
        }
    }
}

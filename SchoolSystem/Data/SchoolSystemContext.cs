using Microsoft.EntityFrameworkCore;
using SchoolSystem.Models;

namespace SchoolSystem.Data
{
    public class SchoolSystemContext : DbContext
    {
        public DbSet<Student> Students { get; set; }


        public SchoolSystemContext(DbContextOptions<SchoolSystemContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string projectRootDirectory = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;
            string databaseDirectory = Path.Combine(projectRootDirectory, "Data");

            // Ensure the 'dp' directory exists
            if (!Directory.Exists(databaseDirectory))
            {
                Directory.CreateDirectory(databaseDirectory);
            }

            string databasePath = Path.Combine(databaseDirectory, "SchoolDB.db");
            optionsBuilder.UseSqlite($"Data Source={databasePath}");
        }

    }

}

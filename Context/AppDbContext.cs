using Microsoft.EntityFrameworkCore;

namespace todolist.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<todolist.Models.Task> Tasks { get; set; }
        public DbSet<todolist.Models.User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);

            var connectionstring = config.GetConnectionString("defaultConnection");
            optionsBuilder.UseSqlite($"Data Source=" + Path.Join(path, connectionstring));



            // base.OnConfiguring(optionsBuilder);
        }
    }
}
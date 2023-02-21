using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UNCPath.AccessLayer.Models;

namespace UNCPath.AccessLayer.DataAccess
{
    public class UNCDbContext: DbContext
    {
        public UNCDbContext(): base() { }

        public DbSet<Models.File> Files { get; set; }
        public DbSet<Folder> Folders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseSqlServer(config.GetConnectionString("Default"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //to create the middle table FolderFolder as a Folder might have multiple Folders which have alos Multiple folders....
            modelBuilder.Entity<Folder>().HasMany(i => i.Folders).WithMany();
        }
    }
}

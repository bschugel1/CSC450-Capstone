using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Configuration;
using CourseAppCloud.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;

namespace CourseAppCloud.DAL
{
    public class CourseContext : DbContext
    {

        public CourseContext(DbContextOptions<CourseContext> options)
        : base(options)
        { }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CourseContext>(options => options.UseSqlServer("Data Source=CourseAppCloudDev.db"));
        }

        public DbSet<Course> Courses { get; set; }
       

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
          .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
          .AddJsonFile("appsettings.json")
          .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("CloudAppDatabase"));
        }    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.DisplayName());
            }
        }
    }
}

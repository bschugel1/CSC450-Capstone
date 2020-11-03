using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using CourseApp.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CourseApp.DAL
{
    public class ApplicationContext : IdentityDbContext<UserModel, RoleModel, long>
    {

        public ApplicationContext([NotNullAttribute] DbContextOptions<ApplicationContext> options)
        : base(options)
        { }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer("Data Source=courseappdb.db"));
        }

      public DbSet<CourseModel> Courses { get; set; }
      public DbSet <SectionModel> Sections { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
          .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
          .AddJsonFile("appsettings.json")
          .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("CourseAppDB"));
        }    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            var sections = modelBuilder.Entity<SectionModel>();

            sections.HasOne<SectionModel>().WithMany().HasForeignKey(x => x.ParentSectionId);

            base.OnModelCreating(modelBuilder);
        }
    }
}

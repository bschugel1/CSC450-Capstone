using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using CourseApp.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CourseApp.Content;

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
        public DbSet<SectionModel> Sections { get; set; }
        public DbSet<UserCourseModel> UserCourses { get; set; }

        public DbSet<HTMLContentModel> HTMLContents { get; set; }
        public DbSet<EmbedModel> Videos { get; set; }
        public DbSet<FileModel> Files { get; set; }
        public DbSet<MediaItemModel> MediaItems { get; set; }

        public DbSet<TransactionModel> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
              .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
              .AddJsonFile("appsettings.json")
              .Build();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("CourseAppDB"));
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<MediaItemModel>().HasDiscriminator(x => x.MediaType);

            var mbSM = modelBuilder.Entity<SectionModel>();


            mbSM.HasOne<SectionModel>().WithMany().HasForeignKey(x => x.ParentSectionId);



            var mbUCM = modelBuilder.Entity<UserCourseModel>();
            mbUCM.HasKey(sc => new { sc.UserId, sc.CourseId });

            var mbTM = modelBuilder.Entity<TransactionModel>();

            mbTM.HasOne(u => u.User).WithMany().OnDelete(DeleteBehavior.Restrict);

            mbTM.HasOne(u => u.Course).WithMany().OnDelete(DeleteBehavior.Restrict);

            mbTM.Property(x => x.Amount).HasColumnType("decimal(18, 2)");
          
            
            var mbCM = modelBuilder.Entity<CourseModel>();
            mbCM.Property(x => x.Price).HasColumnType("decimal(18, 2)");
            


            mbUCM.HasOne(sc => sc.User)
            .WithMany(s => s.UserCourses)
            .HasForeignKey(sc => sc.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            mbUCM.HasOne(sc => sc.Course)
            .WithMany(c => c.UserCourses)
            .HasForeignKey(sc => sc.CourseId)
            .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(modelBuilder);


        }
    }
}

using Microsoft.EntityFrameworkCore;
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
        //Db Context Entity Sets
        public DbSet<CourseModel> Courses { get; set; }
        public DbSet<SectionModel> Sections { get; set; }
        public DbSet<UserCourseModel> UserCourses { get; set; }
        public DbSet<FeaturedCourseModel> FeaturedCourses { get; set; }
        public DbSet<HTMLContentModel> HTMLContents { get; set; }
        public DbSet<EmbedModel> Videos { get; set; }
        public DbSet<FileModel> Files { get; set; }
        public DbSet<MediaItemModel> MediaItems { get; set; }
        public DbSet<TransactionModel> Transactions { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer("Data Source=courseappdb.db"));
        }

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
            var mbSM = modelBuilder.Entity<SectionModel>();
            var mbUCM = modelBuilder.Entity<UserCourseModel>();
            var mbTM = modelBuilder.Entity<TransactionModel>();
            var mbCM = modelBuilder.Entity<CourseModel>();
            var mbMIM = modelBuilder.Entity<MediaItemModel>();

            mbSM.HasOne<SectionModel>().WithMany().HasForeignKey(x => x.ParentSectionId);

            mbUCM.HasKey(sc => new { sc.UserId, sc.CourseId });

            mbTM.HasOne(u => u.User).WithMany().OnDelete(DeleteBehavior.Restrict);
            mbTM.HasOne(u => u.Course).WithMany().OnDelete(DeleteBehavior.Restrict);
            mbTM.Property(x => x.Amount).HasColumnType("decimal(18, 2)");

            mbCM.Property(x => x.Price).HasColumnType("decimal(18, 2)");
            mbUCM.HasOne(sc => sc.User)
            .WithMany(s => s.UserCourses)
            .HasForeignKey(sc => sc.UserId)
            .OnDelete(DeleteBehavior.Restrict);
            mbUCM.HasOne(sc => sc.Course)
            .WithMany(c => c.UserCourses)
            .HasForeignKey(sc => sc.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

            mbMIM.HasDiscriminator(x => x.MediaType);

            base.OnModelCreating(modelBuilder);
        }
    }
}

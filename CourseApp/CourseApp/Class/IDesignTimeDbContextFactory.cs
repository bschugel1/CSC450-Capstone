using CourseApp.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CourseApp.Class
{
    public class IDesignTimeDbContextFactory
    {

        public class EmployeeFactory : IDesignTimeDbContextFactory<ApplicationContext>
        {
            public ApplicationContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
                optionsBuilder.UseSqlServer("CourseAppDB");

                return new ApplicationContext(optionsBuilder.Options);
            }
        }
    }
}

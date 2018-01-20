using Demo.Models.Configuration;
using Orc.EntityFramework;

namespace Demo.Models.DAL
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class ProjectDbContext : DbContext
    {
        //static ProjectDbContext()
        //{
        //    Database.SetInitializer(new DropCreateDatabaseAlways<ProjectDbContext>());
        //}
        public ProjectDbContext()
            : base("name=ProjectDbContext")
        {
            //this.Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new EmployeeConfiguration());
            modelBuilder.Configurations.Add(new ProjectConfiguration());

            modelBuilder.Entity<Project>().IgnoreCatelProperties();
            modelBuilder.Entity<Employee>().IgnoreCatelProperties();

            base.OnModelCreating(modelBuilder);
        }

        public  DbSet<Project> Projects { get; set; }
        public  DbSet<Employee> Employees { get; set; }

    }
}
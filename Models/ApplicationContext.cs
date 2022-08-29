using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApp.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<UserFile> UserFiles { get; set; }
        public DbSet<Files> Files { get; set; }
     

        public DbSet<Enrollee> Enrollees { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) 
           : base(options)
        {
            //  Database.EnsureCreated();
        }
        public DbSet<E1table> E1Tables { get; set; }
        public DbSet<E2table> E2Tables { get; set; }
        public DbSet<E3table> E3Tables { get; set; }

        //public ApplicationContext()
        //{
        //    Database.EnsureCreated();
        //}
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=WebDb;Username=postgres;Password=341289000");
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminRoleName = "admin";
            string userRoleName = "user";

            string adminEmail = "admin@mail.ru";
            string adminPassword = "123456";

            // добавляем роли
            Role adminRole = new Role { Id = 1, Name = adminRoleName };
            Role userRole = new Role { Id = 2, Name = userRoleName };
            User adminUser = new User { Id = 1, Email = adminEmail, Password = adminPassword, RoleId = adminRole.Id };

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser });
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Earning> Earnings { get; set; }
        public DbSet<License> Licenses { get; set; }

        public DbSet<MonthlyIncome> MonthlyIncomes { get; set; }
    }
}

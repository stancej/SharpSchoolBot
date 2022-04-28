using System;
using System.Text;
using System.Data.Entity;
using TelegramSchoolProject.SchoolDb.Models;


namespace TelegramSchoolProject.SchoolDb
{
    public partial class scTelegramDB: DbContext
    {
        public scTelegramDB()
            :base(@"data source=(local)\mssqllocaldb;initial catalog=SchoolDb;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Dictionary> Dictionaries { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dictionary>().ToTable("Dictionaries");
            modelBuilder.Entity<User>().ToTable("Users");
            base.OnModelCreating(modelBuilder);
        }
    }
}

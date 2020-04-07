using System;
using Microsoft.EntityFrameworkCore;

namespace Tasky.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {

        }
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
        {

        }

        public DbSet<Project> Project { get; set; }
        public DbSet<Work> Work { get; set; }
        public DbSet<User> User { get; set; }

        public DbSet<UserProjects> UserProjects { get; set; }
        public DbSet<UserWorks> UserWorks { get; set; }

    }
}
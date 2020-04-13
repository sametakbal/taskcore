using System;
using Microsoft.EntityFrameworkCore;

namespace Tasky.Models
{
    public sealed class DatabaseContext : DbContext
    {
        private static DatabaseContext _context = null;
        private static readonly object padlock = new object();
        private DatabaseContext()
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
        public DbSet<Mail> Mail { get; set; }

        public static DatabaseContext getContext()
        {
                if (_context == null)
                {
                    lock (padlock)
                    {
                        if (_context == null)
                        {
                            _context = new DatabaseContext();
                        }
                    }
                }
                return _context;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-9EK8BDA\\SQLEXPRESS;Database=taskydb;Trusted_Connection=True;");
            }
        }

    }
}
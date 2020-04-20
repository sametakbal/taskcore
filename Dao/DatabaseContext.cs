using System;
using Microsoft.EntityFrameworkCore;
using taskcore.Models;

namespace taskcore.Dao
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
        public DbSet<UserMates> UserMates { get; set; }
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
                optionsBuilder.UseSqlServer("Server=localhost;Database=taskcoredb;User Id=SA;Password=Blcbm8745Database;");
            }
        }

    }
}
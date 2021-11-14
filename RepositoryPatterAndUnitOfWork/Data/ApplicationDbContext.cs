using Microsoft.EntityFrameworkCore;
using RepositoryPatterAndUnitOfWork.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatterAndUnitOfWork.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(U => {
                U.Property(u=>u.Id).ValueGeneratedOnAdd();
            });
               
        }
        public DbSet<User> Users { get; set; }
    }
}

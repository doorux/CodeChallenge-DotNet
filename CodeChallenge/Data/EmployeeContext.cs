using CodeChallenge.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Data
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Compensation> Compensation { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Define employee navigation for compensation
            modelBuilder.Entity<Compensation>()
                .HasKey(c => new { c.EmployeeID });
        }

    }

}

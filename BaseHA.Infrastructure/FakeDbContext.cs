using BaseHA.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseHA.Infrastructure
{
    public class FakeDbContext : DbContext
    {

        public FakeDbContext(DbContextOptions<FakeDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Unit> Units { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)=> optionsBuilder.LogTo(Console.WriteLine);
    }
}

using BaseHA.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseHA.Infrastructure
{
    public class Fake2DbContext : DbContext
    {

        public Fake2DbContext(DbContextOptions<Fake2DbContext> options) : base(options)
        {
        }

        public virtual DbSet<Unitssss> Unitsssses { get; set; }

    }
}

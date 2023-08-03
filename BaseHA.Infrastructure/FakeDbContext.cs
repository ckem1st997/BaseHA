using BaseHA.Domain.Entity;
using BaseHA.Infrastructure.EntityConfigurations;
using Elasticsearch.Net;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BaseHA.Infrastructure
{
    public class FakeDbContext : DbContext
    {

        // đăng ký ở starup
        public FakeDbContext(DbContextOptions<FakeDbContext> options)
            : base(options)
        {
            System.Diagnostics.Debug.WriteLine("FakeDbContext::ctor ->" + this.GetHashCode());
        }

        public virtual DbSet<BeginningWareHouse> BeginningWareHouses { get; set; }
        public virtual DbSet<Inward> Inwards { get; set; }
        public virtual DbSet<InwardDetail> InwardDetails { get; set; }
        public virtual DbSet<Outward> Outwards { get; set; }
        public virtual DbSet<OutwardDetail> OutwardDetails { get; set; }
        public virtual DbSet<SerialWareHouse> SerialWareHouses { get; set; }
        public virtual DbSet<Domain.Entity.Unit> Units { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }
        public virtual DbSet<Domain.Entity.WareHouse> WareHouses { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new VendorEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new BeginningWareHouseEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new InwardDetailEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new InwardEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OutwardDetailEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OutwardEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SerialWareHouseEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UnitEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new WareHouseEntityTypeConfiguration());


        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }


    }

}

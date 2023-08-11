﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Serilog;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Diagnostics;
using BaseHA.Domain.Entity;

namespace BaseHA.Domain.Contexts
{
    public partial class WareHouseContext : DbContext
    {
        public virtual DbSet<Audit> Audits { get; set; } = null!;
        public virtual DbSet<AuditCouncil> AuditCouncils { get; set; } = null!;
        public virtual DbSet<AuditDetail> AuditDetails { get; set; } = null!;
        public virtual DbSet<AuditDetailSerial> AuditDetailSerials { get; set; } = null!;
        public virtual DbSet<BeginningWareHouse> BeginningWareHouses { get; set; } = null!;
        public virtual DbSet<Inward> Inwards { get; set; } = null!;
        public virtual DbSet<InwardDetail> InwardDetails { get; set; } = null!;
        public virtual DbSet<Outward> Outwards { get; set; } = null!;
        public virtual DbSet<OutwardDetail> OutwardDetails { get; set; } = null!;
        public virtual DbSet<SerialWareHouse> SerialWareHouses { get; set; } = null!;
        public virtual DbSet<Unit> Units { get; set; } = null!;
        public virtual DbSet<Vendor> Vendors { get; set; } = null!;
        public virtual DbSet<WareHouse> WareHouses { get; set; } = null!;
        public virtual DbSet<WareHouseItem> WareHouseItems { get; set; } = null!;
        public virtual DbSet<WareHouseItemCategory> WareHouseItemCategories { get; set; } = null!;
        public virtual DbSet<WareHouseItemUnit> WareHouseItemUnits { get; set; } = null!;
        public virtual DbSet<WareHouseLimit> WareHouseLimits { get; set; } = null!;
        public virtual DbSet<WarehouseBalance> WarehouseBalances { get; set; } = null!;

        public WareHouseContext(DbContextOptions<WareHouseContext>
    options) : base(options)
    {
    }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
                optionsBuilder.LogTo(Log.Information, LogLevel.Information, DbContextLoggerOptions.UtcTime).EnableSensitiveDataLogging().EnableDetailedErrors().EnableServiceProviderCaching();
                optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information, DbContextLoggerOptions.UtcTime).EnableSensitiveDataLogging().EnableDetailedErrors().EnableServiceProviderCaching();
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.

                optionsBuilder.UseSqlServer("Data Source=desktop-itlr9t6;Initial Catalog=WarehouseManagement;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Audit>(entity =>
            {
                entity.ToTable("Audit");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate)
                    .HasPrecision(0)
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate)
                    .HasPrecision(0)
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.VoucherCode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.VoucherDate)
                    .HasPrecision(0)
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.WareHouseId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.HasOne(d => d.WareHouse)
                    .WithMany(p => p.Audits)
                    .HasForeignKey(d => d.WareHouseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Audits_PK_WareHouse");
            });

            modelBuilder.Entity<AuditCouncil>(entity =>
            {
                entity.ToTable("AuditCouncil");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.AuditId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Role)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Audit)
                    .WithMany(p => p.AuditCouncils)
                    .HasForeignKey(d => d.AuditId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AuditCouncils_PK_Audit");
            });

            modelBuilder.Entity<AuditDetail>(entity =>
            {
                entity.ToTable("AuditDetail");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.AuditId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.AuditQuantity).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.Conclude)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ItemId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.Quantity).HasColumnType("decimal(15, 2)");

                entity.HasOne(d => d.Audit)
                    .WithMany(p => p.AuditDetails)
                    .HasForeignKey(d => d.AuditId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AuditDetails_PK_Audit");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.AuditDetails)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_AuditDetails_PK_WareHouseItem");
            });

            modelBuilder.Entity<AuditDetailSerial>(entity =>
            {
                entity.ToTable("AuditDetailSerial");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.AuditDetailId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.ItemId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Serial)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.HasOne(d => d.AuditDetail)
                    .WithMany(p => p.AuditDetailSerials)
                    .HasForeignKey(d => d.AuditDetailId)
                    .HasConstraintName("FK_AuditDetailSerials_PK_AuditDetail");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.AuditDetailSerials)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AuditDetailSerials_PK_WareHouseItem");
            });

            modelBuilder.Entity<BeginningWareHouse>(entity =>
            {
                entity.ToTable("BeginningWareHouse");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate)
                    .HasPrecision(0)
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ItemId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate)
                    .HasPrecision(0)
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Quantity).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.UnitId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UnitName).HasMaxLength(255);

                entity.Property(e => e.WareHouseId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.BeginningWareHouses)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BeginningWareHouses_PK_WareHouseItem");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.BeginningWareHouses)
                    .HasForeignKey(d => d.UnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BeginningWareHouses_PK_Unit");

                entity.HasOne(d => d.WareHouse)
                    .WithMany(p => p.BeginningWareHouses)
                    .HasForeignKey(d => d.WareHouseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BeginningWareHouses_PK_WareHouse");
            });

            modelBuilder.Entity<Inward>(entity =>
            {
                entity.ToTable("Inward");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate)
                    .HasPrecision(0)
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Deliver).HasMaxLength(255);

                entity.Property(e => e.DeliverAddress).HasMaxLength(50);

                entity.Property(e => e.DeliverDepartment).HasMaxLength(50);

                entity.Property(e => e.DeliverPhone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate)
                    .HasPrecision(0)
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Reason).HasMaxLength(255);

                entity.Property(e => e.ReasonDescription).HasMaxLength(255);

                entity.Property(e => e.Receiver).HasMaxLength(255);

                entity.Property(e => e.ReceiverAddress).HasMaxLength(50);

                entity.Property(e => e.ReceiverDepartment).HasMaxLength(50);

                entity.Property(e => e.ReceiverPhone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Reference)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.VendorId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.Voucher)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.VoucherCode)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.VoucherDate)
                    .HasPrecision(0)
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.WareHouseId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("WareHouseID")
                    .HasDefaultValueSql("('')");

                entity.HasOne(d => d.Vendor)
                    .WithMany(p => p.Inwards)
                    .HasForeignKey(d => d.VendorId)
                    .HasConstraintName("FK_WareHouseInwards_PK_Vendor");

                entity.HasOne(d => d.WareHouse)
                    .WithMany(p => p.Inwards)
                    .HasForeignKey(d => d.WareHouseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WareHouseInwards_PK_WareHouse");
            });

            modelBuilder.Entity<InwardDetail>(entity =>
            {
                entity.ToTable("InwardDetail");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.AccountMore)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.AccountYes)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(15, 2)")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.CustomerId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerName).HasMaxLength(255);

                entity.Property(e => e.DepartmentId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.DepartmentName).HasMaxLength(255);

                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeName).HasMaxLength(255);

                entity.Property(e => e.InwardId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ItemId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(15, 2)")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.ProjectId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.ProjectName).HasMaxLength(255);

                entity.Property(e => e.Quantity).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.StationId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.StationName).HasMaxLength(255);

                entity.Property(e => e.Status).HasMaxLength(255);

                entity.Property(e => e.Uiprice)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("UIPrice")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.Uiquantity)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("UIQuantity");

                entity.Property(e => e.UnitId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.HasOne(d => d.Inward)
                    .WithMany(p => p.InwardDetails)
                    .HasForeignKey(d => d.InwardId)
                    .HasConstraintName("FK_InwardDetails_PK_Inward");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.InwardDetails)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InwardDetails_PK_WareHouseItem");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.InwardDetails)
                    .HasForeignKey(d => d.UnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InwardDetails_PK_Unit");
            });

            modelBuilder.Entity<Outward>(entity =>
            {
                entity.ToTable("Outward");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate)
                    .HasPrecision(0)
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Deliver).HasMaxLength(255);

                entity.Property(e => e.DeliverAddress).HasMaxLength(50);

                entity.Property(e => e.DeliverDepartment).HasMaxLength(50);

                entity.Property(e => e.DeliverPhone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate)
                    .HasPrecision(0)
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Reason).HasMaxLength(255);

                entity.Property(e => e.ReasonDescription).HasMaxLength(255);

                entity.Property(e => e.Receiver).HasMaxLength(255);

                entity.Property(e => e.ReceiverAddress).HasMaxLength(50);

                entity.Property(e => e.ReceiverDepartment).HasMaxLength(50);

                entity.Property(e => e.ReceiverPhone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Reference)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ToWareHouseId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.Voucher)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.VoucherCode)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.VoucherDate)
                    .HasPrecision(0)
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.WareHouseId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("WareHouseID")
                    .HasDefaultValueSql("('')");

                entity.HasOne(d => d.ToWareHouse)
                    .WithMany(p => p.OutwardToWareHouses)
                    .HasForeignKey(d => d.ToWareHouseId)
                    .HasConstraintName("FK_ToWareHouse_Outwards_PK_ToWareHouse");

                entity.HasOne(d => d.WareHouse)
                    .WithMany(p => p.OutwardWareHouses)
                    .HasForeignKey(d => d.WareHouseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Warehouse_Outwards_PK_WareHouse");
            });

            modelBuilder.Entity<OutwardDetail>(entity =>
            {
                entity.ToTable("OutwardDetail");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.AccountMore).HasMaxLength(255);

                entity.Property(e => e.AccountYes).HasMaxLength(255);

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(15, 2)")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.CustomerId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerName).HasMaxLength(255);

                entity.Property(e => e.DepartmentId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.DepartmentName).HasMaxLength(255);

                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeName).HasMaxLength(255);

                entity.Property(e => e.ItemId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.OutwardId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(15, 2)")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.ProjectId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.ProjectName).HasMaxLength(255);

                entity.Property(e => e.Quantity).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.StationId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.StationName).HasMaxLength(255);

                entity.Property(e => e.Status).HasMaxLength(255);

                entity.Property(e => e.Uiprice)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("UIPrice")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.Uiquantity)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("UIQuantity");

                entity.Property(e => e.UnitId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.OutwardDetails)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OutwardDetails_PK_WareHouseItem");

                entity.HasOne(d => d.Outward)
                    .WithMany(p => p.OutwardDetails)
                    .HasForeignKey(d => d.OutwardId)
                    .HasConstraintName("FK_OutwardDetails_PK_Outward");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.OutwardDetails)
                    .HasForeignKey(d => d.UnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OutwardDetails_PK_Unit");
            });

            modelBuilder.Entity<SerialWareHouse>(entity =>
            {
                entity.ToTable("SerialWareHouse");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.InwardDetailId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.ItemId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.OutwardDetailId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.Serial)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.HasOne(d => d.InwardDetail)
                    .WithMany(p => p.SerialWareHouses)
                    .HasForeignKey(d => d.InwardDetailId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_SerialWareHouses_PK_InwardDetail");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.SerialWareHouses)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SerialWareHouses_PK_WareHouseItem");

                entity.HasOne(d => d.OutwardDetail)
                    .WithMany(p => p.SerialWareHouses)
                    .HasForeignKey(d => d.OutwardDetailId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_SerialWareHouses_PK_OutwardDetail");
            });

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.ToTable("Unit");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UnitName).HasMaxLength(255);
            });

            modelBuilder.Entity<Vendor>(entity =>
            {
                entity.ToTable("Vendor");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.Code)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ContactPerson).HasMaxLength(50);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<WareHouse>(entity =>
            {
                entity.ToTable("WareHouse");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.Code)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ParentId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.Path)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<WareHouseItem>(entity =>
            {
                entity.ToTable("WareHouseItem");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CategoryId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("CategoryID");

                entity.Property(e => e.Code)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Country).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Inactive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.UnitId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.VendorId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("VendorID");

                entity.Property(e => e.VendorName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.WareHouseItems)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_WareHouseItems_PK_WareHouseItemCategory");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.WareHouseItems)
                    .HasForeignKey(d => d.UnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WareHouseItems_PK_Unit");

                entity.HasOne(d => d.Vendor)
                    .WithMany(p => p.WareHouseItems)
                    .HasForeignKey(d => d.VendorId)
                    .HasConstraintName("FK_WareHouseItems_PK_Vendor");
            });

            modelBuilder.Entity<WareHouseItemCategory>(entity =>
            {
                entity.ToTable("WareHouseItemCategory");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Code)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Inactive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ParentId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.Path)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_WareHouseItemCategories_PK_Parent");
            });

            modelBuilder.Entity<WareHouseItemUnit>(entity =>
            {
                entity.ToTable("WareHouseItemUnit");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IsPrimary)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ItemId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UnitId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.WareHouseItemUnits)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_WareHouseItemUnits_PK_WareHouseItem");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.WareHouseItemUnits)
                    .HasForeignKey(d => d.UnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WareHouseItemUnits_PK_Unit");
            });

            modelBuilder.Entity<WareHouseLimit>(entity =>
            {
                entity.ToTable("WareHouseLimit");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasPrecision(0)
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ItemId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate)
                    .HasPrecision(0)
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UnitId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UnitName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.WareHouseId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.WareHouseLimits)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WareHouseLimits_PK_WareHouseItem");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.WareHouseLimits)
                    .HasForeignKey(d => d.UnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WareHouseLimits_PK_Unit");

                entity.HasOne(d => d.WareHouse)
                    .WithMany(p => p.WareHouseLimits)
                    .HasForeignKey(d => d.WareHouseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WareHouseLimits_PK_WareHouse");
            });

            modelBuilder.Entity<WarehouseBalance>(entity =>
            {
                entity.ToTable("WarehouseBalance");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(15, 2)")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.ItemId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Quantity).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.WarehouseId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
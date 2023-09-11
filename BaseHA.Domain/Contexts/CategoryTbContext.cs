using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Serilog;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Diagnostics;
using BaseHA.Domain.Entity;

namespace BaseHA.Domain.Contexts
{
    public partial class CategoryTbContext : DbContext
    {
        public virtual DbSet<CategoryTb> CategoryTbs { get; set; } = null!;
        public virtual DbSet<Intent> Intents { get; set; } = null!;

        public CategoryTbContext(DbContextOptions<CategoryTbContext>
    options) : base(options)
    {
    }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
                optionsBuilder.LogTo(Log.Information, LogLevel.Information, DbContextLoggerOptions.UtcTime).EnableSensitiveDataLogging().EnableDetailedErrors();
                optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information, DbContextLoggerOptions.UtcTime).EnableSensitiveDataLogging().EnableDetailedErrors();
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.

                optionsBuilder.UseSqlServer("Data Source=LAPTOP-2OONI8N5;Initial Catalog=Category;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryTb>(entity =>
            {
                entity.ToTable("CategoryTB");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Category)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.IntentCodeEn)
                    .HasMaxLength(255)
                    .HasColumnName("Intent_Code_EN");

                entity.Property(e => e.IntentCodeVn)
                    .HasMaxLength(255)
                    .HasColumnName("Intent_Code_VN");
            });

            modelBuilder.Entity<Intent>(entity =>
            {
                entity.ToTable("Intent");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CategoryId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.IntentCodeEn)
                    .HasMaxLength(255)
                    .HasColumnName("Intent_Code_EN");

                entity.Property(e => e.IntentEn)
                    .HasMaxLength(255)
                    .HasColumnName("Intent_EN");

                entity.Property(e => e.IntentVn)
                    .HasMaxLength(255)
                    .HasColumnName("Intent_VN");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Intents)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Category_Intent");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

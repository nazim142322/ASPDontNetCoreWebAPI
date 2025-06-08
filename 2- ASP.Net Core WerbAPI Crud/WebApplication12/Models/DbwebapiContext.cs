using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication12.Models;

public partial class DbwebapiContext : DbContext
{
    public DbwebapiContext()
    {
    }

    public DbwebapiContext(DbContextOptions<DbwebapiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Students__3214EC0753A35719");

            entity.Property(e => e.FatherName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.StudentGender)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.StudnetName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

using System;
using System.Collections.Generic;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class GrossToNetContext : DbContext
{
    public GrossToNetContext()
    {
    }

    public GrossToNetContext(DbContextOptions<GrossToNetContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("employees");

            entity.HasIndex(e => e.Id, "Id_UNIQUE").IsUnique();

            entity.Property(e => e.FirstName).HasMaxLength(45);
            entity.Property(e => e.GrossIncome).HasPrecision(9, 2);
            entity.Property(e => e.LastName).HasMaxLength(45);
            entity.Property(e => e.WorkPosition).HasMaxLength(45);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DinhNhatMinh.Models;

public partial class QlsachContext : DbContext
{
    public QlsachContext()
    {
    }

    public QlsachContext(DbContextOptions<QlsachContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Sach> Saches { get; set; }

    public virtual DbSet<Tacgium> Tacgia { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=MINH-MINH;Initial Catalog=QLSACH;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Sach>(entity =>
        {
            entity.HasKey(e => e.Masach).HasName("PK__SACH__3FC48E4CBE3F2496");

            entity.ToTable("SACH");

            entity.Property(e => e.Masach)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MASACH");
            entity.Property(e => e.Matg)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MATG");
            entity.Property(e => e.Namxuatban).HasColumnName("NAMXUATBAN");
            entity.Property(e => e.Sotrang).HasColumnName("SOTRANG");
            entity.Property(e => e.Tensach)
                .HasMaxLength(50)
                .HasColumnName("TENSACH");

            entity.HasOne(d => d.MatgNavigation).WithMany(p => p.Saches)
                .HasForeignKey(d => d.Matg)
                .HasConstraintName("FK_S_TG");
        });

        modelBuilder.Entity<Tacgium>(entity =>
        {
            entity.HasKey(e => e.Matg).HasName("PK__TACGIA__6023721A1F9E4C0E");

            entity.ToTable("TACGIA");

            entity.Property(e => e.Matg)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MATG");
            entity.Property(e => e.Tentacgia)
                .HasMaxLength(50)
                .HasColumnName("TENTACGIA");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

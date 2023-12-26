using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WpfApp7.Models;

public partial class QlduAn05Context : DbContext
{
    public QlduAn05Context()
    {
    }

    public QlduAn05Context(DbContextOptions<QlduAn05Context> options)
        : base(options)
    {
    }

    public virtual DbSet<DuAn> DuAns { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<ThamGium> ThamGia { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=MINH-MINH;Initial Catalog=QLDuAn05;Integrated Security=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DuAn>(entity =>
        {
            entity.HasKey(e => e.MaDa).HasName("PK__DuAn__2725867AC2A8CDF1");

            entity.ToTable("DuAn");

            entity.Property(e => e.MaDa)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MaDA");
            entity.Property(e => e.TenDuAn).HasMaxLength(45);
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.MaNv).HasName("PK__NhanVien__2725D70A506A82A6");

            entity.ToTable("NhanVien");

            entity.Property(e => e.MaNv)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MaNV");
            entity.Property(e => e.HoTen).HasMaxLength(45);
            entity.Property(e => e.NgayTuyenDung).HasMaxLength(45);
        });

        modelBuilder.Entity<ThamGium>(entity =>
        {
            entity.HasKey(e => new { e.MaNhanVien, e.MaDuAn }).HasName("PK__ThamGia__294FBF59503FE503");

            entity.Property(e => e.MaNhanVien)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.MaDuAn)
                .HasMaxLength(10)
                .IsFixedLength();

            entity.HasOne(d => d.MaDuAnNavigation).WithMany(p => p.ThamGia)
                .HasForeignKey(d => d.MaDuAn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ThamGia__MaDuAn__29572725");

            entity.HasOne(d => d.MaNhanVienNavigation).WithMany(p => p.ThamGia)
                .HasForeignKey(d => d.MaNhanVien)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ThamGia__MaNhanV__286302EC");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

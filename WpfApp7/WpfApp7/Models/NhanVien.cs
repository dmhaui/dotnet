using System;
using System.Collections.Generic;

namespace WpfApp7.Models;

public partial class NhanVien
{
    public string MaNv { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public string? NgayTuyenDung { get; set; }

    public double? HeSoLuong { get; set; }

    public virtual ICollection<ThamGium> ThamGia { get; set; } = new List<ThamGium>();
}

using System;
using System.Collections.Generic;

namespace WpfApp7.Models;

public partial class ThamGium
{
    public string MaNhanVien { get; set; } = null!;

    public string MaDuAn { get; set; } = null!;

    public int? SoGioLamViec { get; set; }

    public virtual DuAn MaDuAnNavigation { get; set; } = null!;

    public virtual NhanVien MaNhanVienNavigation { get; set; } = null!;
}

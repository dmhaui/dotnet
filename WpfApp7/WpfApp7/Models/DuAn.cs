using System;
using System.Collections.Generic;

namespace WpfApp7.Models;

public partial class DuAn
{
    public string MaDa { get; set; } = null!;

    public string TenDuAn { get; set; } = null!;

    public virtual ICollection<ThamGium> ThamGia { get; set; } = new List<ThamGium>();
}

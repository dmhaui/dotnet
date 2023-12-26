using System;
using System.Collections.Generic;

namespace DinhNhatMinh.Models;

public partial class Sach
{
    public string Masach { get; set; } = null!;

    public string Tensach { get; set; } = null!;

    public int? Namxuatban { get; set; }

    public int Sotrang { get; set; }

    public string? Matg { get; set; }

    public virtual Tacgium? MatgNavigation { get; set; }
}

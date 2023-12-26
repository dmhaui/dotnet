using System;
using System.Collections.Generic;

namespace DinhNhatMinh.Models;

public partial class Tacgium
{
    public string Matg { get; set; } = null!;

    public string Tentacgia { get; set; } = null!;

    public virtual ICollection<Sach> Saches { get; set; } = new List<Sach>();
}

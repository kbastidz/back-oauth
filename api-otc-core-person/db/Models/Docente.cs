using System;
using System.Collections.Generic;

namespace db.Models;

public partial class Docente
{
    public int Id { get; set; }

    public string Cedula { get; set; } = null!;

    public string Distrito { get; set; } = null!;

    public string? Estado { get; set; }
}

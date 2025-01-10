﻿using System;
using System.Collections.Generic;

namespace db.Models;

public partial class Catalogo
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string? Estado { get; set; }
}

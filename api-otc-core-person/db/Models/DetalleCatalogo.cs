using System;
using System.Collections.Generic;

namespace db.Models;

public partial class DetalleCatalogo
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Estado { get; set; }

    public int? IdCatalogo { get; set; }

    public virtual Catalogo? IdCatalogoNavigation { get; set; }
}

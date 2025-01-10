﻿using System;
using System.Collections.Generic;

namespace db.Models;

public partial class DocenteToMaterium
{
    public int Id { get; set; }

    public int? IdCurso { get; set; }

    public int? IdMateria { get; set; }

    public int? IdUsuario { get; set; }

    public string? Estado { get; set; }

    public virtual Curso? IdCursoNavigation { get; set; }

    public virtual Materium? IdMateriaNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}

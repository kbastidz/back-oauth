﻿using System;
using System.Collections.Generic;

namespace db.Models;

public partial class Curso
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string? Estado { get; set; }

    public virtual ICollection<DocenteToMaterium> DocenteToMateria { get; set; } = new List<DocenteToMaterium>();

    public virtual ICollection<EstudianteToCurso> EstudianteToCursos { get; set; } = new List<EstudianteToCurso>();
}

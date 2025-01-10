using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.dominio
{
    public partial class Usuario
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Contrasenia { get; set; }
        public string Rol { get; set; }
        public string Estado { get; set; }
        public int? IdPersona { get; set; }
        public Persona persona { get; set; }
    }
}

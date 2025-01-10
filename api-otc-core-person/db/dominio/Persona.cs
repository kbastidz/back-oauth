using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.dominio
{
    public partial class Persona
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Cedula { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public string Contacto { get; set; }
        public string Pais { get; set; }
        /*public int? IdUsuario { get; set; }
        public Usuario user { get; set; }*/
    }
}

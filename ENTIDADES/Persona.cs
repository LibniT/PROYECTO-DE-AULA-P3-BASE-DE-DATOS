using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTIDADES
{
    public class Persona
    {
        public string Cedula { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }

        public Persona(string cedula, string telefono, string email, string nombre)
        {
            Cedula = cedula;
            Telefono = telefono;
            Email = email;
            Nombre = nombre;
        }

        public Persona() { }
    }
}

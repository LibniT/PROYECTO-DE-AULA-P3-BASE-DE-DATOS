using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTIDADES
{
    public class Administrador : Persona
    {

        public string Usuario { get; set; }
        public string Constrasena { get; set; }

        public Administrador(string cedula, string telefono, string email, string nombre, string usuario, string constrasena) : base(cedula, telefono, email, nombre)
        {
            Usuario = usuario;
            Constrasena = constrasena;
        }



    }
}

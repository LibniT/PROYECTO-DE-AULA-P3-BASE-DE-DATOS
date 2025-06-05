using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_Mano.Entity
{
    public class AdministradorEntity
    {
        public int IdAdmin { get; set; }
        public string Nombre { get; set; }
        public string Usuario { get; set; }
        public string Contraseña { get; set; }


        public AdministradorEntity()
        {
            IdAdmin = 0;
            Nombre = string.Empty;
            Usuario = string.Empty;
            Contraseña = string.Empty;
        }

        public AdministradorEntity(int idAdmin, string nombre, string usuario, string contraseña)
        {
            IdAdmin = idAdmin;
            Nombre = nombre;
            Usuario = usuario;
            Contraseña = contraseña;
        }

    }
}

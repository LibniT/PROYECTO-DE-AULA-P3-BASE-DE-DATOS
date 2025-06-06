using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_Mano.Entity
{
    public class ClienteEntity
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Cedula { get; set; }
        public string Estado { get; set; }
        public int Strikes { get; set; }
        public decimal DeudaTotal { get; set; }
        public decimal LimiteCredito { get; set; }
        public DateTime FechaCreacion { get; set; }

        public ClienteEntity()
        {
            Id = 0;
            Nombre = string.Empty;
            Cedula = string.Empty;
            Estado = "Activo"; // Estado por defecto
            Strikes = 0;
            DeudaTotal = 0.0m;
            LimiteCredito = 0.0m;
            FechaCreacion = DateTime.Now; // Fecha actual por defecto
        }

        public ClienteEntity(int id, string nombre, string cedula, string estado, int strikes, decimal deudaTotal, decimal limiteCredito, DateTime fechaCreacion)
        {
            Id = id;
            Nombre = nombre;
            Cedula = cedula;
            Estado = estado;
            Strikes = strikes;
            DeudaTotal = deudaTotal;
            LimiteCredito = limiteCredito;
            FechaCreacion = fechaCreacion;
        }


    }
}

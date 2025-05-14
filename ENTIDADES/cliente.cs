using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;


namespace ENTIDADES
{
    public class Cliente : Persona
    {
        public decimal LimiteCredito { get; set; }
        public decimal DeudaTotal { get; set; }
        public string Direccion { get; set; }


        public Cliente(string cedula, string telefono, string email, string apellido, int id_proveedor, int id_cliente, decimal limiteCredito, decimal deudaTotal, string direccion)
            : base(cedula, telefono, email, apellido)
        {
            LimiteCredito = limiteCredito;
            DeudaTotal = deudaTotal;
            Direccion = direccion;
        }

        public Cliente() { }


    }
}

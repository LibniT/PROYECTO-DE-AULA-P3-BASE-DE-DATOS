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
        public enum Estado
        {
            Activo,
            Inactivo,
            Pendiente
        }

        public int Strikes { get; set; }
        public List<Venta> HistorialCompras { get; set; } = new List<Venta>();

        public Cliente(string cedula, string telefono, string email, string nombre, decimal limiteCredito, decimal deudaTotal, string direccion) : base(cedula, telefono, email, nombre)
        {
            LimiteCredito = limiteCredito;
            DeudaTotal = deudaTotal;
            Direccion = direccion;
        }

        public void ActualizarDeuda(decimal monto)
        {
            if (monto < 0)
            {
                throw new ArgumentException("El monto no puede ser negativo.");
            }
            if (DeudaTotal + monto > LimiteCredito)
            {
                throw new InvalidOperationException("El monto excede el límite de crédito.");
            }
            DeudaTotal += monto;
        }

        public List<Pago> HistorialCredito()
        {
            throw new NotImplementedException();
        }

        public void ContadorAdvertencias()
        {
            Strikes++;
            if (Strikes >= 3)
            {
                // Implementar lógica para bloquear al cliente
                throw new InvalidOperationException("El cliente ha alcanzado el límite de advertencias.");
            }
        }



    }
}

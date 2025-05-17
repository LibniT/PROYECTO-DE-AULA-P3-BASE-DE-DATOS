using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTIDADES
{
    public class Deuda : Pago
    {

        public DateTime FechaVencimiento { get; set; }
        public decimal Interes { get; set; }

        public override void CambiarEstado(Estado estado)
        {
            // Implementar lógica para cambiar el estado de la deuda
            throw new NotImplementedException();
        }

        public override void CrearFactura()
        {
            // Implementar lógica para crear la factura de la deuda
            throw new NotImplementedException();
        }

        public void GenerarRecordatorio()
        {
            // Implementar lógica para generar un recordatorio de pago
            throw new NotImplementedException();
        }

        public decimal CalcularInteres(decimal monto)
        {
            // Implementar lógica para calcular el interés sobre el monto de la deuda
            return monto * Interes / 100;
        }

        public List<Cliente> ListaNegra(Cliente cliente, DateTime FechaVencimiento)
        {
            throw new NotImplementedException();
        }

        public Deuda()
        {
            // Constructor por defecto
        }

        public Deuda(int id, DateTime fecha, Cliente cliente, DateTime fechaVencimiento, decimal interes)
            : base()
        {
            Id = id;
            Fecha = fecha;
            Cliente = cliente;
            FechaVencimiento = fechaVencimiento;
            Interes = interes;
        }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ENTIDADES
{
    public class cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public decimal SaldoPendiente { get; set; }

        public cliente(int id, string nombre, string telefono, decimal saldoPendiente)
        {
            Id = id;
            Nombre = nombre;
            Telefono = telefono;
            SaldoPendiente = saldoPendiente;
        }

        public override string ToString()
        {
            return $"Id: {Id}, Nombre: {Nombre}, Telefono: {Telefono}, Saldo Pendiente: {SaldoPendiente}";
        }

    }
}

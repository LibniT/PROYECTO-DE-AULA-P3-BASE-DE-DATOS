using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTIDADES
{
    public class VentaDTO
    {
        public int ClienteId { get; set; }      // ID del cliente
        public string NombreCliente { get; set; } // Nombre del cliente
        public int VentaId { get; set; }        // ID de la venta
        public DateTime FechaVenta { get; set; } // Fecha de la venta
        public decimal Total { get; set; }      // Total de la venta
    }

}

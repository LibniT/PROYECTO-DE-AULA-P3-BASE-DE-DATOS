using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_Mano.Entity
{
    public class VentaEntity
    {
        public int IdVenta { get; set; }
        public string Descripcion { get; set; }
        public int IdCliente { get; set; }
        public decimal Total { get; set; }
        public DateTime Fecha { get; set; }
    }
}

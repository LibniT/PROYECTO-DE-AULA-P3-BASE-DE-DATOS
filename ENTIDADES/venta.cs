using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTIDADES
{
    class Venta : BaseEntity
    {
        public int Id_Cliente { get; set; }
        public int Id_Item { get; set; }
        public decimal Total { get; set; }
        public DateTime Fecha { get; set; } 

        public Venta(int id_cliente, int id_item, decimal total, DateTime fecha)
        {
            Id_Cliente = id_cliente;
            Id_Item = id_item;
            Total = total;
            Fecha = fecha;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTIDADES
{
    public class ItemVenta
    {

        public int Id_Producto { get; set; }
        public int Id_Item { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal SubTotal { get; set; }

        public ItemVenta(int id_producto, int id_item, int cantidad, decimal precio)
        {
            Id_Producto = id_producto;
            Id_Item = id_item;
            Cantidad = cantidad;
            Precio = precio;
            SubTotal = precio * cantidad;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTIDADES
{
    public class Venta 
    {

        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public Cliente Cliente { get; set; }
        public List<Carrito> Productos { get; set; } = new List<Carrito>();
        public decimal Total => Productos.Sum(p => p.Subtotal);

        public Venta(int id, DateTime fecha, Cliente cliente)
        {
            Id = id;
            Fecha = fecha;
            Cliente = cliente;
        }

        public Venta() { }


        public void AgregarProducto(Carrito producto)
        {
            Productos.Add(producto);
        }

    }
}

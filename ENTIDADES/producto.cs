using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTIDADES
{
    public class Producto
    {

        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int CantidadStock { get; set; }


        public Producto(int id, string nombre, decimal precio, int cantidadStock)
        {
            Id = id;
            Nombre = nombre;
            Precio = precio;
            CantidadStock = cantidadStock;
        }

        public Producto() { }

        public void ActualizarStock(int cantidad)
        {
            if (cantidad < 0)
            {
                throw new ArgumentException("La cantidad no puede ser negativa.");
            }
            if (cantidad > CantidadStock)
            {
                throw new InvalidOperationException("No hay suficiente stock disponible.");
            }

            CantidadStock -= cantidad;
        }



    }
}

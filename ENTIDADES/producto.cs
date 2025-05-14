using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTIDADES
{
    class Producto : NamedEntity
    {
        public decimal Precio { get; set; }
        public int CantidadStock { get; set; }


        public Producto(string nombre, decimal precio, int cantidadStock)
        {
            Nombre = nombre;
            Precio = precio;
            CantidadStock = cantidadStock;
        }
    }
}

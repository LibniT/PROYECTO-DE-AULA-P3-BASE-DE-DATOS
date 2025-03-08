using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTIDADES
{
    class venta
    {

        public int Id { get; set; }
        public cliente Cliente { get; set; }
        public List<producto> Productos { get; set; }
        public decimal Total { get; set; }
        public bool Pagado { get; set; }
        public DateTime Fecha { get; set; }


        public venta(int id, cliente cliente, List<producto> productos, decimal total, bool pagado, DateTime fecha)
        {
            Id = id;
            Cliente = cliente;
            Productos = productos;
            Total = total;
            Pagado = pagado;
            Fecha = fecha;
        }

        public override string ToString()
        {
            return $"Id: {Id}, Cliente: {Cliente}, Productos: {Productos}, Total: {Total}, Pagado: {Pagado}, Fecha: {Fecha}";
        }

    }
}

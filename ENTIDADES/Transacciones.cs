using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTIDADES
{
    public class Transacciones : BaseEntity
    {
        public int Id_Cliente { get; set; }
        public int Id_Venta { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }


        public Transacciones(int id_cliente, int id_venta, decimal monto, DateTime fecha, string descripcion)
        {
            Id_Cliente = id_cliente;
            Id_Venta = id_venta;
            Monto = monto;
            Fecha = fecha;
            Descripcion = descripcion;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTIDADES
{
    public class Deudas : BaseEntity
    {
        public int Id_Cliente { get; set; }
        public int Id_Venta { get; set; }
        public DateTime FechaVencimiento { get; set; }

        public Deudas(int id_cliente, int id_venta, DateTime fechaVencimiento)
        {
            Id_Cliente = id_cliente;
            Id_Venta = id_venta;
            FechaVencimiento = fechaVencimiento;
        }

    }
}

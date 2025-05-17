using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTIDADES
{
    public abstract class Pago
    {

        public int Id { get; set; }
        public enum Estado
        {
            Activo,
            Inactivo,
            Pendiente
        }

        public string MetodoPago { get; set; }
        public Venta Venta { get; set; }


        public abstract void CambiarEstado(Estado estado);
        public abstract void CrearFactura();


    }
}

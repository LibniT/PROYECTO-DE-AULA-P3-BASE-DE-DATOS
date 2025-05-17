using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTIDADES
{
    public class Pago : BaseEntity
    {

        public int Id_Deuda { get; set; }
        public string MetodoPago { get; set; }


        public Pago(int id_deuda, string metodoPago)
        {
            Id_Deuda = id_deuda;
            MetodoPago = metodoPago;
        }

        public Pago() { }

    }
}

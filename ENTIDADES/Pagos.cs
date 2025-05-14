using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTIDADES
{
    public class Pagos : BaseEntity
    {

        public int Id_Deuda { get; set; }
        public string MetodoPago { get; set; }


        public Pagos(int id_deuda, string metodoPago)
        {
            Id_Deuda = id_deuda;
            MetodoPago = metodoPago;
        }

        public Pagos() { }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTIDADES
{
    [NotMapped] // This tells EF that this class doesn't map to a database table
    public abstract class Pago
    {
        public abstract int Id { get; set; }
        public abstract string MetodoPago { get; set; }
        public abstract int venta_id { get; set; }
        public abstract VENTA Venta { get; set; }

       
       
    }
}

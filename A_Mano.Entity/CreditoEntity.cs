using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_Mano.Entity
{
    public class CreditoEntity
    {
        public int IdCredito { get; set; }
        public string Estado { get; set; }
        public int IdVenta { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public decimal MontoTotal { get; set; }
        public decimal TotalAbonado { get; set; }
        public decimal SaldoPendiente => MontoTotal - TotalAbonado;
    }
}

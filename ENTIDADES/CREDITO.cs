using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENTIDADES
{
    [Table("CREDITO")]
    public class CREDITO
    {
        [Key]
        [Column("id_credito")]
        public int Id { get; set; }

        [Column("estado")]
        [StringLength(15)]
        public string Estado { get; set; }

        [Column("id_venta")]
        public int VentaId { get; set; }

        [Column("FechaVencimiento")]
        public DateTime FechaVencimiento { get; set; }

        // Relación con VENTA
        [ForeignKey("VentaId")]
        public virtual VENTA Venta { get; set; }

        public CREDITO() { }
    }
}

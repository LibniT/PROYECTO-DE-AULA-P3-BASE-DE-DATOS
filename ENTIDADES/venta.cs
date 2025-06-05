using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENTIDADES
{
    [Table("VENTA")]
    public class VENTA
    {
        [Key]
        [Column("id_venta")]
        public int Id { get; set; }

        [Column("fecha")]
        public DateTime Fecha { get; set; } = DateTime.Now; // Valor por defecto

        [Column("descripcion")]
        [StringLength(150)]
        public string Descripcion { get; set; }

        [Column("id_cliente")]
        public int ClienteId { get; set; }

        [Column("total")]
        public decimal Total { get; set; }

        // Relación con CLIENTE
        [ForeignKey("ClienteId")]
        public virtual CLIENTE Cliente { get; set; }

        public VENTA() { }
    }
}

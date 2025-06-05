using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENTIDADES
{
    [Table("ABONO")]
    public class ABONO
    {
        [Key]
        [Column("id_abono")]
        public int Id { get; set; }

        [Column("id_credito")]
        public int CreditoId { get; set; }

        [Column("fecha_abono")]
        public DateTime FechaAbono { get; set; } = DateTime.Now; // Valor por defecto

        [Column("monto_abonado")]
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto abonado debe ser mayor que 0.")]
        public decimal MontoAbonado { get; set; }

        [Column("metodo_pago")]
        [StringLength(35)]
        public string MetodoPago { get; set; }

        // Relación con CREDITO
        [ForeignKey("CreditoId")]
        public virtual CREDITO Credito { get; set; }

        public ABONO() { }
    }
}

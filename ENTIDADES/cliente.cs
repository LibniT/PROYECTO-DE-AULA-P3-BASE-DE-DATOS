using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENTIDADES
{
    [Table("CLIENTE")]
    public class CLIENTE : Persona
    {
        [Key]
        [Column("id_cliente")]
        public int Id { get; set; }

        [Column("cedula")]
        [StringLength(12)]
        [Index(IsUnique = true)] // Restricción de unicidad
        public string Cedula { get; set; }

        [Column("nombre")]
        [Required]
        [StringLength(60)]
        public string Nombre { get; set; }

        [Column("telefono")]
        [StringLength(10)]
        public string Telefono { get; set; }

        [Column("email")]
        [StringLength(55)]
        public string Email { get; set; }

        [Column("deudaTotal")]
        public decimal DeudaTotal { get; set; }

        [Column("direccion")]
        [StringLength(40)]
        public string Direccion { get; set; }

        [Column("limiteCredito")]
        public decimal LimiteCredito { get; set; }

        [Column("estado")]
        [StringLength(15)]
        [Required]
        public string Estado { get; set; } // Asegurarse de que sea requerido

        [Column("strikes")]
        public int Strikes { get; set; }

        public CLIENTE() { }

        public CLIENTE(int id, string cedula, string nombre, string telefono, string email, decimal deudaTotal,
            string direccion, decimal limiteCredito, string estado, int strikes)
        {
            Id = id;
            Cedula = cedula;
            Nombre = nombre;
            Telefono = telefono;
            Email = email;
            DeudaTotal = deudaTotal;
            Direccion = direccion;
            LimiteCredito = limiteCredito;
            Estado = estado;
            Strikes = strikes;
        }

        public CLIENTE(string cedula, string nombre, string telefono, string email, decimal deudaTotal,
            string direccion, decimal limiteCredito, string estado, int strikes)
        {
            Cedula = cedula;
            Nombre = nombre;
            Telefono = telefono;
            Email = email;
            DeudaTotal = deudaTotal;
            Direccion = direccion;
            LimiteCredito = limiteCredito;
            Estado = estado;
            Strikes = strikes;
        }
    }
}

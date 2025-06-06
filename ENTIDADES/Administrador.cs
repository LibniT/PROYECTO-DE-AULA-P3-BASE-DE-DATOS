using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENTIDADES
{
    [Table("ADMINISTRADOR")]
    public class ADMINISTRADOR : Persona
    {
        [Key]
        [Column("id_admin")]
        public int Id { get; set; }

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

        [Column("usuario")]
        [Required]
        [StringLength(25)]
        public string Usuario { get; set; }

        [Column("contraseña")]
        [Required]
        [StringLength(20)]
        public string Contraseña { get; set; }

        public ADMINISTRADOR() { }

        public ADMINISTRADOR(int id, string nombre, string telefono, string email, string usuario, string contraseña)
            : base(id, nombre, telefono, email)
        {
            Usuario = usuario;
            Contraseña = contraseña;
        }

        public ADMINISTRADOR(string nombre, string telefono, string email, string usuario, string contraseña)
        {
            Nombre = nombre;
            Telefono = telefono;
            Email = email;
            Usuario = usuario;
            Contraseña = contraseña;
        }
    }
}

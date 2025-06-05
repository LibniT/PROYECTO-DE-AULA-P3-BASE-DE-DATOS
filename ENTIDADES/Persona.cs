using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTIDADES
{
    public class Persona
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("nombre")]
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Column("telefono")]
        [StringLength(20)]
        public string Telefono { get; set; }

        [Column("email")]
        [StringLength(100)]
        public string Email { get; set; }

        public Persona(int id, string nombre, string telefono, string email)
        {
            Id = id;
            Nombre = nombre;
            Telefono = telefono;
            Email = email;
        }

        public Persona() { }
    }
}

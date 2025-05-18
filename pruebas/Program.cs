using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOGICA;
using PRUEBAS;

namespace pruebas
{
    public class Program
    {
        static void Main(string[] args)
        {

            try
            {
                using (var context = new OracleService())
                {
                    var clientes = context.Clientes.ToList(); // Acceso a la tabla CLIENTE
                    Console.WriteLine("Conexión exitosa y datos recuperados.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Se produjo un error:");
                Console.WriteLine($"Error: {ex.Message}");

                Exception inner = ex.InnerException;
                while (inner != null)
                {
                    Console.WriteLine($"Inner: {inner.Message}");
                    inner = inner.InnerException;
                }
            }

        }
    }
}

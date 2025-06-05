using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using ENTIDADES;

namespace PRIMERA_ENTREGA_PROYECTO_DE_AULA
{
    public partial class FrmFiado: Form
    {
        public FrmFiado()
        {
            InitializeComponent();
            
        }


        private void iconButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string rutaArchivo = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fiado_temp.txt");

            if (!System.IO.File.Exists(rutaArchivo))
            {
                MostrarMensaje("El archivo fiado_temp.txt no existe.");
                return;
            }

            string[] datos = System.IO.File.ReadAllText(rutaArchivo).Split('|');

            if (datos.Length != 3)
            {
                MostrarMensaje("El archivo no contiene el formato esperado.");
                return;
            }

            if (!int.TryParse(datos[0].Trim(), out int clienteId) || !decimal.TryParse(datos[2].Trim(), out decimal total))
            {
                MostrarMensaje("ID o Total no válidos.");
                return;
            }

            DateTime fechaSeleccionada = Calendario.SelectionStart;

            // Crear la venta primero
            VENTA nuevaVenta = new VENTA
            {
                Fecha = DateTime.Now,
                Descripcion = datos[1].Trim(),
                ClienteId = clienteId,
                Total = total
            };

            DBLVenta dbLVenta = new DBLVenta();
            string resultadoVenta = dbLVenta.AddVenta(nuevaVenta);

            if (resultadoVenta != "Registro exitoso")
            {
                MostrarMensaje("Error al registrar la venta: " + resultadoVenta);
                return;
            }

            // Recuperar la venta recién insertada
            VENTA ventaReciente = dbLVenta.GetAllVentas().FindLast(v => v.ClienteId == clienteId && v.Total == total && v.Descripcion == datos[1].Trim());

            if (ventaReciente == null)
            {
                MostrarMensaje("No se pudo recuperar la venta recién creada.");
                return;
            }

            // Crear el crédito asociado
            CREDITO nuevoCredito = new CREDITO
            {
                VentaId = ventaReciente.Id,
                Estado = "Pendiente",
                FechaVencimiento = fechaSeleccionada
            };

            DBLCredito dbLCredito = new DBLCredito();
            string resultadoCredito = dbLCredito.AddCredito(nuevoCredito);

            MostrarMensaje(resultadoCredito);

            // Eliminar el archivo temporal
            System.IO.File.Delete(rutaArchivo);
        }

        private void MostrarMensaje(string mensaje, string titulo = "Información")
        {
            MessageBox.Show(mensaje, titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            if (e.Start <= DateTime.Today)
            {
                MessageBox.Show("No puedes seleccionar fechas anteriores o iguales a hoy.", "Fecha inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Calendario.SetDate(DateTime.Today.AddDays(1));
            }
        }

        private void Calendar_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

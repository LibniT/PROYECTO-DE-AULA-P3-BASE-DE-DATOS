using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using ENTIDADES;

namespace PRIMERA_ENTREGA_PROYECTO_DE_AULA
{
    public partial class FrmVentaN: Form
    {
        public FrmVentaN()
        {
            InitializeComponent();
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos())
            {
                LimpiarCampos();
                return;
            }

            string linea = CrearLinea();
            string rutaArchivo = GuardarLineaEnArchivo(linea);

            if (rutaArchivo == null)
            {
                return; // Error al guardar el archivo
            }

            AbrirFormInPanel(new FrmFiado());
        }

        private bool ValidarCampos()
        {
            // Obtener valores y limpiar espacios
            string id = TxtId.Text.Trim();
            string descripcion = TxtDescripcion.Text.Trim();
            string totalTexto = TxtTotal.Text.Trim();

            // Validaciones básicas
            if (string.IsNullOrEmpty(id))
            {
                MostrarMensaje("El campo 'ID' no puede estar vacío.", "Validación");
                TxtId.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(descripcion))
            {
                MostrarMensaje("El campo 'Descripción' no puede estar vacío.", "Validación");
                TxtDescripcion.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(totalTexto))
            {
                MostrarMensaje("El campo 'Total' no puede estar vacío.", "Validación");
                TxtTotal.Focus();
                return false;
            }

            // Validar que el total sea un número decimal válido
            if (!decimal.TryParse(totalTexto, out decimal total) || total <= 0)
            {
                MostrarMensaje("El campo 'Total' debe ser un número mayor que cero.", "Validación");
                TxtTotal.Focus();
                return false;
            }

            return true;
        }

        private string CrearLinea()
        {
            string id = TxtId.Text.Trim();
            string descripcion = TxtDescripcion.Text.Trim();
            string totalTexto = TxtTotal.Text.Trim();
            decimal total = decimal.Parse(totalTexto); // Se asume que la validación ya pasó

            return $"{id}|{descripcion}|{total}";
        }

        private string GuardarLineaEnArchivo(string linea)
        {
            string rutaArchivo = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fiado_temp.txt");

            try
            {
                System.IO.File.WriteAllText(rutaArchivo, linea);
                return rutaArchivo;
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al guardar el archivo: {ex.Message}", "Error");
                return null;
            }
        }

        private void MostrarMensaje(string mensaje, string titulo = "Información")
        {
            MessageBox.Show(mensaje, titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LimpiarCampos()
        {
            TxtId.Clear();
            TxtDescripcion.Clear();
            TxtTotal.Clear();
        }


        private Form activeForm = null;
        private void AbrirFormInPanel(Form Formhijo)

        {
            if (activeForm != null)
            {
                activeForm.Close();
            }
            activeForm = Formhijo;
            Formhijo.TopLevel = false;
            Formhijo.FormBorderStyle = FormBorderStyle.None;
            Formhijo.Dock = DockStyle.Fill;
            //if (this.panelDesktop.Controls.Count > 0)
            //    this.panelDesktop.Controls.RemoveAt(0);
            this.panel2.Controls.Add(Formhijo);
            this.panel2.Tag = Formhijo;
            Formhijo.BringToFront();
            Formhijo.Show();

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            AbrirFormInPanel( new FrmVistaClientes());
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void TxtDescripcion_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

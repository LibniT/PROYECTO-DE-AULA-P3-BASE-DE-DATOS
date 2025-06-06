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
    public partial class FrmCliente : Form
    {
        public FrmCliente()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }



        private void btnCrear_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos())
            {
                MessageBox.Show("Por favor, complete todos los campos correctamente.");
                LimpiarCampos();
                return;
            }

            CLIENTE cliente = CrearCliente();
            string resultado = AgregarCliente(cliente);
            MessageBox.Show(resultado);
            LimpiarCampos();
        }

        private bool ValidarCampos()
        {
            // Validar que todos los campos requeridos estén completos
            string nombre = TxtNombre.Text.Trim();
            string cedula = TxtCedula.Text.Trim();
            string telefono = TxtTelefono.Text.Trim();
            string email = TxtEmail.Text.Trim();
            string direccion = TxtDireccion.Text.Trim();
            decimal LimCredito;

            return !string.IsNullOrEmpty(nombre) &&
                   !string.IsNullOrEmpty(cedula) &&
                   !string.IsNullOrEmpty(telefono) &&
                   !string.IsNullOrEmpty(email) &&
                   !string.IsNullOrEmpty(direccion) &&
                   decimal.TryParse(TxtLimCredito.Text.Trim(), out LimCredito) &&
                   LimCredito > 0; // Asegurarse de que el límite de crédito sea mayor que 0
        }

        private CLIENTE CrearCliente()
        {
            string nombre = TxtNombre.Text.Trim();
            string cedula = TxtCedula.Text.Trim();
            string telefono = TxtTelefono.Text.Trim();
            string email = TxtEmail.Text.Trim();
            string direccion = TxtDireccion.Text.Trim();
            decimal LimCredito = decimal.Parse(TxtLimCredito.Text.Trim()); // Se asume que la validación ya pasó

            // Crear el objeto CLIENTE
            return new CLIENTE(cedula, nombre, telefono, email, 0, direccion, LimCredito, "Activo", 0);
        }

        private string AgregarCliente(CLIENTE cliente)
        {
            // Instanciar la capa BLL
            DBLCliente dbCliente = new DBLCliente();
            return dbCliente.AddCliente(cliente);
        }

        

        private void LimpiarCampos()
        {
            TxtNombre.Text = "";
            TxtCedula.Text = "";
            TxtTelefono.Text = "";
            TxtEmail.Text = "";
            TxtDireccion.Text = "";
            TxtLimCredito.Text = "";
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void TxtCedula_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

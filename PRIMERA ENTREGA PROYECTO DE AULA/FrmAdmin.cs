using ENTIDADES;
using System;
using System.Windows.Forms;
using BLL;


namespace PRIMERA_ENTREGA_PROYECTO_DE_AULA
{
    public partial class FrmAdmin: Form
    {
        public FrmAdmin()
        {
            InitializeComponent();
        }

        private void FrmVistaAdmin_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtId_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos())
            {
                MessageBox.Show("Por favor, complete todos los campos correctamente.");
                LimpiarCampos();
                return;
            }

            ADMINISTRADOR admin = CrearAdministrador();
            string resultado = AgregarAdministrador(admin);
            MessageBox.Show(resultado);
            LimpiarCampos();
        }

        private bool ValidarCampos()
        {
            // Validar que todos los campos requeridos estén completos
            string nombre = txtNombre.Text.Trim();
            string telefono = txtTelefono.Text.Trim();
            string email = txtEmail.Text.Trim();
            string usuario = txtUsuario.Text.Trim();
            string contraseña = txtContraseña.Text.Trim();

            return !string.IsNullOrEmpty(nombre) &&
                   !string.IsNullOrEmpty(telefono) &&
                   !string.IsNullOrEmpty(email) &&
                   !string.IsNullOrEmpty(usuario) &&
                   !string.IsNullOrEmpty(contraseña);
        }

        private ADMINISTRADOR CrearAdministrador()
        {
            string nombre = txtNombre.Text.Trim();
            string telefono = txtTelefono.Text.Trim();
            string email = txtEmail.Text.Trim();
            string usuario = txtUsuario.Text.Trim();
            string contraseña = txtContraseña.Text.Trim();

            // Crear el objeto ADMINISTRADOR
            return new ADMINISTRADOR(nombre, telefono, email, usuario, contraseña);
        }

        private string AgregarAdministrador(ADMINISTRADOR admin)
        {
            // Instanciar la capa BLL
            DBLAdministrador dbAdmin = new DBLAdministrador();
            return dbAdmin.AddAdministrador(admin);
        }

        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtTelefono.Text = "";
            txtEmail.Text = "";
            txtUsuario.Text = "";
            txtContraseña.Text = "";
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtCedula_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}


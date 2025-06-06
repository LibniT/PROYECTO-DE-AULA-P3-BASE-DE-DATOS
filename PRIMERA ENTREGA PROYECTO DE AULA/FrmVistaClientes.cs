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
using Microsoft.VisualBasic;

namespace PRIMERA_ENTREGA_PROYECTO_DE_AULA
{
    public partial class FrmVistaClientes: Form
    {

        private DBLCliente dbCliente;
        private CLIENTE cliente;



        public FrmVistaClientes()
        {
            InitializeComponent();
            CargarTabla();
        }


        private void CargarTabla()
        {
            lvClientes.Items.Clear();

            List<CLIENTE> clientes = ObtenerClientes();
            foreach (CLIENTE cliente in clientes)
            {
                ListViewItem item = new ListViewItem(cliente.Id.ToString());
                item.SubItems.Add(cliente.Cedula);
                item.SubItems.Add(cliente.Nombre);
                item.SubItems.Add(cliente.Direccion);
                item.SubItems.Add(cliente.Telefono);
                item.SubItems.Add(cliente.Email);
                item.SubItems.Add(cliente.LimiteCredito.ToString());
                item.SubItems.Add(cliente.Estado);
                item.SubItems.Add(cliente.Strikes.ToString());
                item.SubItems.Add(cliente.DeudaTotal.ToString());
                lvClientes.Items.Add(item);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void lstCliente_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void lvClientes_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private List<CLIENTE> ObtenerClientes()
        {
            List<CLIENTE> clientes = new List<CLIENTE>();
            DBLCliente dbCliente = new DBLCliente();
            clientes = dbCliente.GetAllClientes();
            return clientes;
        }


        #region Eliminar Cliente

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            int? id = PedirIdClienteParaBorrar();
            if (id == null) return;

            cliente = dbCliente.GetClienteById(id.Value);
            if (cliente == null)
            {
                MessageBox.Show("No se encontró un cliente con ese ID.");
                return;
            }

            if (ConfirmarBorrado(cliente))
            {
                string resultado = dbCliente.DeleteCliente(cliente.Id);
                MessageBox.Show(resultado);
                CargarTabla();
            }
        }

        private int? PedirIdClienteParaBorrar()
        {
            string input = Interaction.InputBox("Ingrese el ID del cliente a borrar:", "Borrar Cliente", "");
            if (!int.TryParse(input, out int id) || id <= 0)
            {
                MessageBox.Show("ID inválido. Por favor, ingrese un número entero positivo.");
                return null;
            }
            return id;
        }

        private bool ConfirmarBorrado(CLIENTE cliente)
        {
            DialogResult confirmacion = MessageBox.Show(
                $"¿Está seguro que desea borrar el cliente:\n\nNombre: {cliente.Nombre}\nID: {cliente.Id}?",
                "Confirmar borrado",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );
            return confirmacion == DialogResult.Yes;
        }

        #endregion

        #region Modificar Cliente
        private void BtnModificar_Click(object sender, EventArgs e)
        {
            int? id = PedirIdCliente();
            if (id == null) return;

            dbCliente = new DBLCliente();
            cliente = dbCliente.GetClienteById(id.Value);

            if (cliente == null)
            {
                MessageBox.Show("No se encontró un cliente con ese ID.");
                return;
            }

            ActualizarDatosCliente(cliente);

            string resultado = dbCliente.UpdateCliente(cliente);
            MessageBox.Show(resultado);
            CargarTabla();
        }

        private int? PedirIdCliente()
        {
            string input = Interaction.InputBox("Ingrese el ID del cliente a modificar:", "Modificar Cliente", "");

            if (!int.TryParse(input, out int id) || id <= 0)
            {
                MessageBox.Show("ID inválido. Por favor, ingrese un número entero positivo.");
                return null;
            }

            return id;
        }

        private void ActualizarDatosCliente(CLIENTE cliente)
        {
            cliente.Nombre = PedirValor("Nombre", cliente.Nombre);
            cliente.Direccion = PedirValor("Dirección", cliente.Direccion);
            cliente.Telefono = PedirValor("Teléfono", cliente.Telefono);
            cliente.Email = PedirValor("Email", cliente.Email);
            cliente.LimiteCredito = PedirDecimal("Límite de Crédito", cliente.LimiteCredito);
        }

        private string PedirValor(string campo, string valorActual)
        {
            return Interaction.InputBox($"{campo} actual: {valorActual}\nIngrese el nuevo {campo.ToLower()}:", $"Modificar {campo}", valorActual);
        }

        private decimal PedirDecimal(string campo, decimal valorActual)
        {
            string input = Interaction.InputBox($"{campo} actual: {valorActual}\nIngrese el nuevo {campo.ToLower()}:", $"Modificar {campo}", valorActual.ToString());

            if (!decimal.TryParse(input, out decimal nuevoValor))
            {
                MessageBox.Show($"{campo} inválido. Se mantendrá el valor actual.");
                return valorActual;
            }

            return nuevoValor;
        }

        #endregion

    }
}

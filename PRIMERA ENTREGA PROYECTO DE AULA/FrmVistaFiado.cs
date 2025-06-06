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
using DATOS;
using ENTIDADES;

namespace PRIMERA_ENTREGA_PROYECTO_DE_AULA
{
    public partial class FrmVistaFiado: Form
    {
        public FrmVistaFiado()
        {
            InitializeComponent();
            ConfigurarListView();
            CargarVentasFiado();

        }


        private void ConfigurarListView()
        {
            lvClientes.View = View.Details;
            lvClientes.Columns.Clear();
            lvClientes.Columns.Add("ID Venta", 70);
            lvClientes.Columns.Add("Fecha Venta", 100);
            lvClientes.Columns.Add("Total", 80);
            lvClientes.Columns.Add("ID Cliente", 70);
            lvClientes.Columns.Add("Nombre Cliente", 150);
            lvClientes.Columns.Add("Fecha Vencimiento", 120);
        }

        private void CargarVentasFiado()
        {
            DLVentaFiado dlVentaFiado = new DLVentaFiado();
            var ventas = dlVentaFiado.ObtenerVentasFiado();
            lvClientes.Items.Clear();
            foreach (var venta in ventas)
            {
                ListViewItem item = new ListViewItem(venta.VentaId.ToString());
                item.SubItems.Add(venta.FechaVenta.ToString("dd/MM/yyyy"));
                item.SubItems.Add(venta.Total.ToString("C"));
                item.SubItems.Add(venta.ClienteId.ToString());
                item.SubItems.Add(venta.NombreCliente);
                item.SubItems.Add(venta.FechaVencimiento.ToString("dd/MM/yyyy"));
                lvClientes.Items.Add(item);
            }
        }





        private void iconButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lvClientes_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}

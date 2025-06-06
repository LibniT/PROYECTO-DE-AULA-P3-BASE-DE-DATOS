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
    public partial class FrmVistaVentas: Form
    {
        public FrmVistaVentas()
        {
            InitializeComponent();
            CargarDatosVentas();
        }

        private void CargarDatosVentas()
        {
            DLVenta_ dlVenta = new DLVenta_();
            List<VentaDTO> ventas = dlVenta.ObtenerVentas(); // Llamar al método para obtener las ventas
                                                             // Limpiar el ListView antes de agregar nuevos elementos
            lvClientes.Items.Clear();
            // Agregar cada venta al ListView
            foreach (var venta in ventas)
            {
                ListViewItem item = new ListViewItem(venta.ClienteId.ToString()); // Primero: ID del cliente
                item.SubItems.Add(venta.NombreCliente);                           // Segundo: nombre del cliente
                item.SubItems.Add(venta.VentaId.ToString());                      // Tercero: ID de la venta
                item.SubItems.Add(venta.FechaVenta.ToString("dd/MM/yyyy"));       // Cuarto: fecha
                item.SubItems.Add(venta.Total.ToString("C"));                     // Quinto: total
                lvClientes.Items.Add(item);
            }

        }


        private void lvClientes_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

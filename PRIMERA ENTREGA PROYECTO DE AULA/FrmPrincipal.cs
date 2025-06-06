using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PRIMERA_ENTREGA_PROYECTO_DE_AULA
{
    public partial class FrmPrincipal: Form
    {

        private int bordersize = 2;
        public FrmPrincipal()
        {
            InitializeComponent();
            CollapseMenu(); 
            customizeDesing();
            this.Padding = new Padding(bordersize);
            this.BackColor = Color.FromArgb(98, 102, 244);
            var inicioUC = new UserControl();
            inicioUC.Dock = DockStyle.Fill;
            panelDesktop.Controls.Add(inicioUC);

            activeForm = null;
        }
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        private void panelDesktop_Paint(object sender, PaintEventArgs e)
        {

        }

        private void customizeDesing()
        {
            panelSubMenuUsuarios.Visible = false;
            panelMenuListados.Visible = false;
        }
        private void hideSubMenu()
        {
            if (panelSubMenuUsuarios.Visible == true)
                panelSubMenuUsuarios.Visible = false;
            if (panelMenuListados.Visible == true)
                panelMenuListados.Visible = false;
        }   

        private void showSubMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                hideSubMenu();
                subMenu.Visible = true;
            }
            else
                subMenu.Visible = false; 

        }
        private void panelTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            //var inicioUC = new UserControl();
            //inicioUC.Dock = DockStyle.Fill;
            //panelDesktop.Controls.Add(inicioUC);

            //activeForm = null;
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_NCCALCSIZE= 0x0083;
            if(m.Msg == WM_NCCALCSIZE && m.WParam.ToInt32() == 1)
            {
                return; 
            }
            base.WndProc(ref m); 

        }

        private void FrmPrincipal_Resize(object sender, EventArgs e)
        {
            AdjustFormPrincipal();
        }

        private void AdjustFormPrincipal()
        {
            switch (this.WindowState)
            {
                case FormWindowState.Maximized:
                    this.Padding = new Padding(0, 8, 8, 0);
                    break;
                case FormWindowState.Normal:
                    if (this.Padding.Top != bordersize)
                        this.Padding = new Padding(bordersize);
                    break; 
            }
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState= FormWindowState.Minimized;
        }

        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            if(this.WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            else 
                this.WindowState = FormWindowState.Normal;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit(); 
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            CollapseMenu(); 
        }
        private void CollapseMenu()
        {
            if (this.panelMenu.Width > 200)
            {
                panelMenu.Width = 100;
                pictureBox1.Visible = false;
                btnMenu.Dock = DockStyle.Top;
                foreach (Button menuButton in panelMenu.Controls.OfType<Button>())
                {
                    menuButton.Text = "";
                    menuButton.ImageAlign = ContentAlignment.MiddleCenter;
                    menuButton.Padding = new Padding(0);
                }
            }
            else
            {
                panelMenu.Width = 230;
                pictureBox1.Visible = true;
                btnMenu.Dock = DockStyle.None;
                foreach (Button menuButton in panelMenu.Controls.OfType<Button>())
                {
                    menuButton.Text ="   "+ menuButton.Tag.ToString();
                    menuButton.ImageAlign = ContentAlignment.MiddleLeft;
                    menuButton.Padding = new Padding(10,0,0,0);
                }

            }
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
            this.panelDesktop.Controls.Add(Formhijo);
            this.panelDesktop.Tag = Formhijo;
            Formhijo.BringToFront();
            Formhijo.Show();

        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            showSubMenu(panelSubMenuUsuarios);
        }

        private void iconButton6_Click(object sender, EventArgs e)
        {
            AbrirFormInPanel(new FrmAdmin());
        }

        private void iconButton7_Click(object sender, EventArgs e)
        {
            AbrirFormInPanel(new FrmCliente());
        }

        private void iconButton3_Click_1(object sender, EventArgs e)
        {
            showSubMenu(panelMenuListados);
        }

        private void iconButton5_Click(object sender, EventArgs e)
        {
            hideSubMenu();
            AbrirFormInPanel(new FrmVentaN());
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            panelDesktop.Controls.Clear();

            var inicio = new UserControl(); // Tu UserControl con el GIF
            inicio.Dock = DockStyle.Fill;
            panelDesktop.Controls.Add(inicio);

            activeForm = null;
        }

        private void iconButton10_Click(object sender, EventArgs e)
        {

        }

        private void iconButton4_Click(object sender, EventArgs e)
        {

        }

        private void iconButton8_Click(object sender, EventArgs e)
        {
            AbrirFormInPanel(new FrmVistaClientes()); 
        }

        private void iconButton9_Click(object sender, EventArgs e)
        {
            AbrirFormInPanel(new FrmVistaFiado()); 
        }

        private void iconButton4_Click_1(object sender, EventArgs e)
        {
            AbrirFormInPanel(new FrmVistaListaNegra()); 
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void iconButton2_Click_1(object sender, EventArgs e)
        {
            AbrirFormInPanel(new FrmVistaVentas());
        }
    }
}

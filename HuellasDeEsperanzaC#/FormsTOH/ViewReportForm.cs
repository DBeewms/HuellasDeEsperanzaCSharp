using HuellasDeEsperanzaC_.Models;
using HuellasDeEsperanzaC_.Servicio;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HuellasDeEsperanzaC_.FormsTOH
{
    public partial class ViewReportForm : Form
    {
        private Usuario usuarioActual;
        private GestorAdopcion gestorAdopcionUser;
        private GestorMascota gestorMascotaUser;

        public ViewReportForm(Usuario usuario, GestorAdopcion gestorAdopcion)
        {
            InitializeComponent();
            this.usuarioActual = usuario;
            this.gestorAdopcionUser = gestorAdopcion;
            this.gestorMascotaUser = new GestorMascota();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HomeAdminForm homeAdminForm = new HomeAdminForm(usuarioActual, gestorAdopcionUser);
            homeAdminForm.Show();
            this.Close();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                WindowState = FormWindowState.Normal;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Constantes para manejar el arrastre de la ventana
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);


        // Método para permitir arrastrar la ventana desde el panel2
        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            // Libera el control del mouse
            ReleaseCapture();
            // Envía un mensaje para iniciar el arrastre de la ventana
            SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

        private void ViewReportForm_Load(object sender, EventArgs e)
        {
            //try
            //{
            //    // Cargar el reporte de mascotas disponibles para adopción
            //    CargarReporteMascotas();
            //    this.reportViewer1.RefreshReport();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error al cargar el reporte: " + ex.Message);
            //}
        }

        private void CargarReporteMascotas()
        {
            //// Obtener la lista de mascotas disponibles para adopción
            //List<Mascota> mascotasDisponibles = gestorAdopcionUser.ObtenerMascotasDisponibles();

            //// Configurar el ReportViewer
            //reportViewer1.LocalReport.ReportEmbeddedResource = "HuellasDeEsperanzaC#.Reportes.RptMascotas.rdlc";
            //reportViewer1.LocalReport.DataSources.Clear();

            //// Crear una fuente de datos para el ReportViewer
            //ReportDataSource rds = new ReportDataSource("DsMacotas", mascotasDisponibles);
            //reportViewer1.LocalReport.DataSources.Add(rds);

            // Obtener la lista de mascotas disponibles para adopción

            List<Mascota> mascotasDisponibles = gestorMascotaUser.GetListaMascotas();

            // Crear una fuente de datos para el ReportViewer
            ReportDataSource rds = new ReportDataSource("DsMascotas", mascotasDisponibles);

            this.reportViewer1.LocalReport.DataSources.Clear();

            this.reportViewer1.LocalReport.DataSources.Add(rds);

            this.reportViewer1.LocalReport.ReportEmbeddedResource = "HuellasDeEsperanzaC#.Reportes.RptMascotas.rdlc";

            this.reportViewer1.RefreshReport();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AdminAddMascot adminAddMascot = new AdminAddMascot(usuarioActual, gestorAdopcionUser);
            adminAddMascot.Show();
            this.Close();
        }

        private void btnConfiguracion_Click(object sender, EventArgs e)
        {
            ConfiguracionForm configuracionForm = new ConfiguracionForm(usuarioActual, gestorAdopcionUser);
            configuracionForm.ShowDialog();
        }
    }
}

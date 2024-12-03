using HuellasDeEsperanzaC_.Models;
using HuellasDeEsperanzaC_.Servicio;
using Microsoft.Reporting.WinForms;
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

namespace HuellasDeEsperanzaC_.FormsTOH
{
    public partial class HomeAdminForm : Form
    {
        private Usuario usuarioActual;
        private GestorAdopcion gestorAdopcionUser;

        public HomeAdminForm(Usuario usuario, GestorAdopcion gestorAdopcion)
        {
            InitializeComponent();
            this.usuarioActual = usuario;
            this.gestorAdopcionUser = gestorAdopcion;
            SetSaludo();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

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

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                GestorMascota gestorMascota = new GestorMascota();
                gestorMascota.CargarDatosMascotas();
                // Obtener la lista de mascotas 
                List<Mascota> mascotasDisponibles = gestorMascota.GetListaMascotas();

                if (mascotasDisponibles == null || !mascotasDisponibles.Any())
                {
                    MessageBox.Show("No hay mascotas disponibles para mostrar en el informe.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Manejar valores nulos y formatear datos
                foreach (var mascota in mascotasDisponibles)
                {
                    // Validar propiedades
                    mascota.FechaNacimiento = (mascota.FechaNacimiento == DateTime.MinValue) ? DateTime.Now : mascota.FechaNacimiento;

                    mascota.Edad = mascota.CalcularEdad();

                    mascota.Tamaño = string.IsNullOrEmpty(mascota.Tamaño) ? "Sin tamaño" : mascota.Tamaño;
                    mascota.OrganizacionId = (mascota.OrganizacionId == 0) ? -1 : mascota.OrganizacionId;
                    mascota.EstaAdoptado = mascota.EstaAdoptado ? true : false;
                }

                // Crear una fuente de datos para el ReportViewer
                ReportDataSource rds = new ReportDataSource("DsDatos", mascotasDisponibles);

                ViewReportForm viewReportForm = new ViewReportForm(usuarioActual, gestorAdopcionUser);

                viewReportForm.reportViewer1.LocalReport.DataSources.Clear();

                viewReportForm.reportViewer1.LocalReport.DataSources.Add(rds);

                try
                {
                    viewReportForm.reportViewer1.LocalReport.ReportEmbeddedResource = "HuellasDeEsperanzaC_.Reportes.RptMascotas.rdlc";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar el recurso incrustado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                viewReportForm.reportViewer1.RefreshReport();


                viewReportForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el reporte: " + ex.Message);
            }
        }

        private string ObtenerSaludo()
        {
            int hora = DateTime.Now.Hour;
            if (hora >= 6 && hora < 12)
            {
                return "Buenos días";
            }
            else if (hora >= 12 && hora < 18)
            {
                return "Buenas tardes";
            }
            else
            {
                return "Buenas noches";
            }
        }

        private void SetSaludo()
        {
            string saludo = ObtenerSaludo();
            lblSaludo.Text = $"{saludo}, {usuarioActual.NombreCompleto}";
        }

        private void btnConfiguracion_Click(object sender, EventArgs e)
        {
            ConfiguracionForm configuracionForm = new ConfiguracionForm(usuarioActual, gestorAdopcionUser);
            configuracionForm.ShowDialog();
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            GeneralWaitingListForm generalWaitingListForm = new GeneralWaitingListForm(usuarioActual, gestorAdopcionUser);
            generalWaitingListForm.Show();
            this.Hide();
        }
    }
}

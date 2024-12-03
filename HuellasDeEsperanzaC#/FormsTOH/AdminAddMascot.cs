using HuellasDeEsperanzaC_.Models;
using HuellasDeEsperanzaC_.Servicio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HuellasDeEsperanzaC_.FormsTOH
{
    public partial class AdminAddMascot : Form
    {
        private Usuario usuarioActual;
        private GestorAdopcion gestorAdopcionUser;
        private string rutaImagenSeleccionada;

        public AdminAddMascot(Usuario usuario, GestorAdopcion gestorAdopcion)
        {
            InitializeComponent();
            this.usuarioActual = usuario;
            this.gestorAdopcionUser = gestorAdopcion;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HomeAdminForm homeAdminForm = new HomeAdminForm(usuarioActual, gestorAdopcionUser);
            homeAdminForm.Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            GeneralWaitingListForm generalWaitingListForm = new GeneralWaitingListForm(usuarioActual, gestorAdopcionUser);
            generalWaitingListForm.Show();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ViewReportForm viewReportForm = new ViewReportForm(usuarioActual, gestorAdopcionUser);
            viewReportForm.Show();
            this.Close();
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

        private void btnGuardarMascota_Click(object sender, EventArgs e)
        {
            // Validar campos requeridos
            if (string.IsNullOrEmpty(tbNombreMascota.Texts) ||
                string.IsNullOrEmpty(tbTipoMascota.Texts) ||
                string.IsNullOrEmpty(tbSexoMascota.Texts) ||
                string.IsNullOrEmpty(tbRazaMascota.Texts) ||
                string.IsNullOrEmpty(tbDescripcionMascota.Texts))
            {
                MessageBox.Show("Por favor, rellene todos los campos requeridos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Asignar imagen predeterminada si no se selecciona una imagen
            if (string.IsNullOrEmpty(rutaImagenSeleccionada))
            {
                string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                rutaImagenSeleccionada = Path.Combine(projectDirectory, "Resources\\icons8-pets-50.png");
            }

            Mascota mascota = new Mascota
            {
                Nombre = tbNombreMascota.Texts,
                Especie = tbTipoMascota.Texts,
                Sexo = tbSexoMascota.Texts,
                Raza = tbRazaMascota.Texts,
                FechaNacimiento = dtFechaMascota.Value,
                Descripcion = tbDescripcionMascota.Texts,
                RutaImagen = rutaImagenSeleccionada
            };

            GestorMascota gestorMascota = new GestorMascota();
            List<Mascota> mascotas = new List<Mascota>();
            gestorMascota.CargarDatosMascotas();

            bool esEdicion = false;
            for (int i = 0; i < mascotas.Count; i++)
            {
                if (mascotas[i].Id == mascota.Id)
                {
                    esEdicion = true;
                    break;
                }
            }

            if (esEdicion)
            {
                gestorMascota.ActualizarMascota(mascota, this);
            }
            else
            {
                gestorMascota.RegistrarMascota(mascota, this);
            }

            HomeAdminForm homeAdminForm = new HomeAdminForm(usuarioActual, gestorAdopcionUser);
            homeAdminForm.Show();
            this.Close();
        }

        private void AdminAddMascot_Load(object sender, EventArgs e)
        {

        }
    }
}

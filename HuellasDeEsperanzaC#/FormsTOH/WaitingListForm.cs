using HuellasDeEsperanzaC_.CustomUserControls;
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
    public partial class WaitingListForm : Form
    {
        private Usuario usuarioActual;
        private GestorAdopcion gestorAdopcionUser;

        public WaitingListForm(Usuario usuario, GestorAdopcion gestorAdopcion)
        {
            InitializeComponent();
            this.usuarioActual = usuario;
            this.gestorAdopcionUser = gestorAdopcion;
        }

        private void WaitingListForm_Load(object sender, EventArgs e)
        {
            gestorAdopcionUser.CargarDatosSolicitudes();
            CargarMascotaEnEspera();

        }

        private void CargarMascotaEnEspera()
        {
            // Obtener la primera solicitud en espera del usuario actual
            SolicitudAdopcion solicitudEnEspera = gestorAdopcionUser.ObtenerSolicitudPorId(usuarioActual.Id);

            if (solicitudEnEspera != null)
            {
                Mascota mascotaEnEspera = gestorAdopcionUser.ObtenerMascotaPorId(solicitudEnEspera.MascotaId);
                Usuario usuarioSolicitante = gestorAdopcionUser.ObtenerUsuarioPorId(solicitudEnEspera.UsuarioId);

                if (mascotaEnEspera != null)
                {
                    lblEstadoLista.Text = "En espera";
                    lblNombreM.Text = mascotaEnEspera.Nombre;
                    lblEspecieM.Text = mascotaEnEspera.Especie;
                    lblSexoM.Text = mascotaEnEspera.Sexo;
                    lblRazaM.Text = mascotaEnEspera.Raza;
                    lblFechaNacM.Text = mascotaEnEspera.FechaNacimiento.ToString("dd/MM/yyyy");
                    lblDescripcionM.Text = mascotaEnEspera.Descripcion;

                    string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                    string fullPath = Path.Combine(projectDirectory, mascotaEnEspera.RutaImagen);
                    if (File.Exists(fullPath))
                    {
                        pbCircleMascota.Image = Image.FromFile(fullPath);
                    }
                    else
                    {
                        string rutaImagenPredeterminada = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\icons8-pets-50.png");
                        pbCircleMascota.Image = Image.FromFile(rutaImagenPredeterminada);
                    }
                }
                else
                {
                    lblEstadoLista.Text = "No se encontró la mascota en espera";
                    lblNombreM.Text = string.Empty;
                    lblEspecieM.Text = string.Empty;
                    lblSexoM.Text = string.Empty;
                    lblRazaM.Text = string.Empty;
                    lblFechaNacM.Text = string.Empty;
                    lblDescripcionM.Text = string.Empty;
                    pbCircleMascota.Image = null;
                }
            }
            else
            {
                lblEstadoLista.Text = "No hay solicitudes de adopción en espera";
                lblNombreM.Text = string.Empty;
                lblEspecieM.Text = string.Empty;
                lblSexoM.Text = string.Empty;
                lblRazaM.Text = string.Empty;
                lblFechaNacM.Text = string.Empty;
                lblDescripcionM.Text = string.Empty;
                pbCircleMascota.Image = null;
            }
        }




        private void button1_Click(object sender, EventArgs e)
        {
            HomeGeneralForm homeGeneralForm = new HomeGeneralForm(usuarioActual, gestorAdopcionUser);
            homeGeneralForm.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AdoptMascot adoptMascot = new AdoptMascot(usuarioActual, gestorAdopcionUser);
            adoptMascot.Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddMascot addMascot = new AddMascot(usuarioActual, gestorAdopcionUser);
            addMascot.Show();
            this.Close();
        }

        private void btnConfiguracion_Click(object sender, EventArgs e)
        {
            ConfiguracionForm configuracionForm = new ConfiguracionForm(usuarioActual, gestorAdopcionUser);
            configuracionForm.ShowDialog();
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

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Close();
        }
    }
}
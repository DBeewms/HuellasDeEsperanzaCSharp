using HuellasDeEsperanzaC_.CustomUserControls;
using HuellasDeEsperanzaC_.Models;
using HuellasDeEsperanzaC_.Servicio;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HuellasDeEsperanzaC_.FormsTOH
{
    public partial class GeneralWaitingListForm : Form
    {
        private Usuario usuarioActual;
        private GestorAdopcion gestorAdopcionUser;

        public GeneralWaitingListForm(Usuario usuario, GestorAdopcion gestorAdopcion)
        {
            InitializeComponent();
            this.usuarioActual = usuario;
            this.gestorAdopcionUser = gestorAdopcion;
        }

        private void GeneralWaitingListForm_Load(object sender, EventArgs e)
        {
            gestorAdopcionUser.CargarDatosSolicitudes();
            CargarTodasLasSolicitudesEnEspera();
        }

        private void CargarTodasLasSolicitudesEnEspera()
        {
            List<SolicitudAdopcion> solicitudesEnEspera = gestorAdopcionUser.ObtenerSolicitudesEnEspera();

            if (solicitudesEnEspera.Count > 0)
            {
                // Limpiar cualquier contenido previo en el control que muestra las solicitudes
                flowLayoutPanel1.Controls.Clear();

                // Agrupar las solicitudes por usuario
                var solicitudesAgrupadasPorUsuario = solicitudesEnEspera.GroupBy(s => s.UsuarioId);

                foreach (var grupo in solicitudesAgrupadasPorUsuario)
                {
                    Usuario usuario = gestorAdopcionUser.ObtenerUsuarioPorId(grupo.Key);

                    if (usuario == null)
                    {
                        MetroFramework.MetroMessageBox.Show(this,
                            "Error: No se pudo encontrar el usuario con ID " + grupo.Key,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }

                    foreach (var solicitud in grupo)
                    {
                        Mascota mascota = gestorAdopcionUser.ObtenerMascotaPorId(solicitud.MascotaId);

                        if (mascota == null)
                        {
                            MetroFramework.MetroMessageBox.Show(this,
                                "Error: No se pudo encontrar la mascota con ID " + solicitud.MascotaId,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            continue;
                        }

                        WaitingCard waitingCard = new WaitingCard(solicitud.Id)
                        {
                            NombreMascota = mascota.Nombre,
                            RazaMascota = mascota.Raza,
                            SexoMascota = mascota.Sexo,
                            ImagenMascota = CargarImagenMascota(mascota.RutaImagen),
                            NombreSolicitante = usuario.NombreCompleto,
                            EmailSolicitante = usuario.CorreoElectronico,
                            TelefonoSolicitante = usuario.NumeroTelefono
                        };

                        flowLayoutPanel1.Controls.Add(waitingCard);
                    }
                }

                if (flowLayoutPanel1.Controls.Count == 0)
                {
                    MetroFramework.MetroMessageBox.Show(this,
                        "No hay solicitudes de adopción en espera",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this,
                        "Solicitudes en espera cargadas correctamente",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this,
                    "No hay solicitudes de adopción en espera",
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private Image CargarImagenMascota(string rutaImagen)
        {
            try
            {
                string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                string fullPath = Path.Combine(projectDirectory, rutaImagen);
                if (string.IsNullOrEmpty(rutaImagen) || !System.IO.File.Exists(fullPath))
                {
                    throw new System.IO.FileNotFoundException();
                }

                return Image.FromFile(fullPath);
            }
            catch (Exception)
            {
                // Ruta de la imagen predeterminada
                string rutaImagenPredeterminada = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\icons8-pets-50.png");
                return Image.FromFile(rutaImagenPredeterminada);
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void btnConfiguracion_Click(object sender, EventArgs e)
        {
            ConfiguracionForm configuracionForm = new ConfiguracionForm(usuarioActual, gestorAdopcionUser);
            configuracionForm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HomeAdminForm homeAdminForm = new HomeAdminForm(usuarioActual, gestorAdopcionUser);
            homeAdminForm.Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AdminAddMascot adminAddMascot = new AdminAddMascot(usuarioActual, gestorAdopcionUser);
            adminAddMascot.Show();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ViewReportForm viewReportForm = new ViewReportForm(usuarioActual, gestorAdopcionUser);
            viewReportForm.Show();
            this.Close();
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
    }
}

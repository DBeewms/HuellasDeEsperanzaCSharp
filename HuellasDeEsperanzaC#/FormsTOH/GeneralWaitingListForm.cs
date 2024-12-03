using HuellasDeEsperanzaC_.CustomUserControls;
using HuellasDeEsperanzaC_.Models;
using HuellasDeEsperanzaC_.Servicio;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
    }
}

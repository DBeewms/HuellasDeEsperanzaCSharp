using HuellasDeEsperanzaC_.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HuellasDeEsperanzaC_.Servicio
{
    public class GestorAdopcion
    {
        private List<SolicitudAdopcion> solicitudes = new List<SolicitudAdopcion>();
        private List<Mascota> mascotas;
        private List<Usuario> usuarios;

        public GestorAdopcion()
        {
            CargarDatosSolicitudes();
            CargarDatosMascotas();
            CargarDatosUsuarios();
        }

        // Obtener todas las solicitudes de adopción
        public List<SolicitudAdopcion> ObtenerSolicitudesAdopcion()
        {
            return solicitudes;
        }

        // Obtener una solicitud por ID
        public SolicitudAdopcion ObtenerSolicitudPorId(int solicitudId)
        {
            return solicitudes.FirstOrDefault(s => s.Id == solicitudId);
        }

        public SolicitudAdopcion ObtenerSolicitudPorUsuarioId(int usuarioId)
        {
            return solicitudes.FirstOrDefault(s => s.UsuarioId == usuarioId && s.Estado == EstadoSolicitud.Pendiente);
        }

        // Crear una nueva solicitud de adopción
        public void CrearSolicitudAdopcion(int usuarioId, int mascotaId, Form formulario)
        {
            if (UsuarioPendienteAdopcion(usuarioId))
            {
                MessageBox.Show(formulario, "Ya tienes una solicitud de adopción pendiente.", "Adopción pendiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int nuevoId = solicitudes.Count > 0 ? solicitudes.Max(s => s.Id) + 1 : 1;

            SolicitudAdopcion nuevaSolicitud = new SolicitudAdopcion
            {
                Id = nuevoId,
                UsuarioId = usuarioId,
                MascotaId = mascotaId,
                FechaSolicitud = DateTime.Now,
                Estado = EstadoSolicitud.Pendiente,
                Motivo = string.Empty
            };

            solicitudes.Add(nuevaSolicitud);
            GuardarDatosSolicitudes();

            MessageBox.Show(formulario, "Solicitud de adopción creada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Aprobar una solicitud
        public void AprobarSolicitud(int solicitudId, Usuario administrador)
        {
            SolicitudAdopcion solicitud = ObtenerSolicitudPorId(solicitudId);
            if (solicitud != null)
            {
                solicitud.AprobarSolicitud(administrador);
                Mascota mascota = ObtenerMascotaPorId(solicitud.MascotaId);
                if (mascota != null)
                {
                    mascota.EstaAdoptado = true;
                    mascota.AdoptanteId = solicitud.UsuarioId;

                    // Actualizar el estado de la mascota en GestorMascota
                    GestorMascota gestorMascota = new GestorMascota();
                    gestorMascota.ActualizarEstadoMascota(mascota);

                    GuardarDatosMascotas();
                }
                GuardarDatosSolicitudes();
            }
        }

        // Rechazar una solicitud
        public void RechazarSolicitud(int solicitudId, Usuario administrador, string motivo)
        {
            SolicitudAdopcion solicitud = ObtenerSolicitudPorId(solicitudId);
            if (solicitud != null)
            {
                solicitud.RechazarSolicitud(administrador, motivo);
                GuardarDatosSolicitudes();
            }
        }

        //// Obtener mascotas pendientes
        //public List<Mascota> ObtenerMascotas()
        //{
        //    return mascotas.Where(m => !m.EstaAdoptado && !solicitudes.Any(s => s.MascotaId == m.Id && s.Estado == EstadoSolicitud.Pendiente)).ToList();
        //}

        // Obtener mascota por ID
        public Mascota ObtenerMascotaPorId(int mascotaId)
        {
            return mascotas.FirstOrDefault(m => m.Id == mascotaId);
        }

        // Obtener usuario por ID
        public Usuario ObtenerUsuarioPorId(int usuarioId)
        {
            return usuarios.FirstOrDefault(u => u.Id == usuarioId);
        }

        // Verificar si el usuario tiene una solicitud pendiente
        public bool UsuarioPendienteAdopcion(int usuarioId)
        {
            return solicitudes.Any(s => s.UsuarioId == usuarioId && s.Estado == EstadoSolicitud.Pendiente);
        }

        // Obtener solicitudes en espera
        public List<SolicitudAdopcion> ObtenerSolicitudesEnEspera()
        {
            return solicitudes.Where(s => s.Estado == EstadoSolicitud.Pendiente).ToList();
        }

        // Métodos para cargar y guardar datos
        public void CargarDatosSolicitudes()
        {
            solicitudes.Clear();

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "solicitudes.dat");

            if (!File.Exists(path))
                return;

            using (FileStream fs = new FileStream(path, FileMode.Open))
            using (BinaryReader reader = new BinaryReader(fs))
            {
                while (fs.Position < fs.Length)
                {
                    SolicitudAdopcion solicitud = new SolicitudAdopcion
                    {
                        Id = reader.ReadInt32(),
                        UsuarioId = reader.ReadInt32(),
                        MascotaId = reader.ReadInt32(),
                        FechaSolicitud = DateTime.FromBinary(reader.ReadInt64()),
                        Estado = (EstadoSolicitud)reader.ReadInt32(),
                        Motivo = reader.ReadString()
                    };
                    solicitudes.Add(solicitud);
                }
            }
        }

        public void GuardarDatosSolicitudes()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "solicitudes.dat");

            using (FileStream fs = new FileStream(path, FileMode.Create))
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
                foreach (var solicitud in solicitudes)
                {
                    writer.Write(solicitud.Id);
                    writer.Write(solicitud.UsuarioId);
                    writer.Write(solicitud.MascotaId);
                    writer.Write(solicitud.FechaSolicitud.ToBinary());
                    writer.Write((int)solicitud.Estado);
                    writer.Write(solicitud.Motivo ?? string.Empty);
                }
            }
        }

        private void CargarDatosMascotas()
        {
            GestorMascota gestorMascota = new GestorMascota();
            gestorMascota.CargarDatosMascotas();
            mascotas = gestorMascota.GetListaMascotas();
        }

        private void GuardarDatosMascotas()
        {
            GestorMascota gestorMascota = new GestorMascota();
            gestorMascota.SetListaMascotas(mascotas);
            gestorMascota.EscribirArchivoMascotas();
        }

        private void CargarDatosUsuarios()
        {
            GestorUsuario gestorUsuario = new GestorUsuario();
            gestorUsuario.CargarDatosUsuarios();
            usuarios = gestorUsuario.GetListaUsuarios();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HuellasDeEsperanzaC_.Models
{
    public enum TipoUsuario
    {
        Comun,
        Organizacion,
        Administrador
    }
    public class Usuario
    {
       // Campos por defecto para todos los usuarios
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string CorreoElectronico { get; set; }
        public string HashContrasena { get; set; }
        public TipoUsuario Tipo { get; set; }
        public bool PerfilCompleto { get; set; }

        // Campos para completar el perfil
        public string Direccion { get; set; }
        public string NumeroTelefono { get; set; }
        public string NumeroCedula { get; set; }
        public string Ocupacion { get; set; }

        // Campos específicos para organizaciones
        public string Descripcion { get; set; }

        public List<Mascota> MascotasAdoptadas { get; set; }
        public List<ActividadVoluntariado> ActividadesInscritas { get; set; }
        
        public Usuario()
        {
            MascotasAdoptadas = new List<Mascota>();
            ActividadesInscritas = new List<ActividadVoluntariado>();
            Tipo = TipoUsuario.Comun;
            PerfilCompleto = false;
        }

        public void EstablecerContraseña(string contraseña)
        {
            HashContrasena = HashPassword(contraseña);
        }

        public bool VerificarContraseña(string contraseña)
        {
            return HashContrasena == HashPassword(contraseña);
        }

        public string HashPassword(string contraseña)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(contraseña));
                return Convert.ToBase64String(bytes);
            }
        }

        public void CompletarPerfil(string nombreCompleto, string direccion, string numeroTelefono, string numeroCedula, string ocupacion, string descripcion)
        {
            NombreCompleto = nombreCompleto;
            Direccion = direccion;
            NumeroTelefono = numeroTelefono;
            NumeroCedula = numeroCedula;
            Ocupacion = ocupacion;
            Descripcion = descripcion;
            PerfilCompleto = true;
        }

        public void CompletarPerfilUsuario(string nombreCompleto, string direccion, string numeroTelefono, string numeroCedula, string ocupacion)
        {
            NombreCompleto = nombreCompleto;
            Direccion = direccion;
            NumeroTelefono = numeroTelefono;
            NumeroCedula = numeroCedula;
            Ocupacion = ocupacion;
            PerfilCompleto = true;
        }

        public void AdoptarMascota(Mascota mascota)
        {
            if (PerfilCompleto)
            {
                MascotasAdoptadas.Add(mascota);
            }
            else
            {
                throw new InvalidOperationException("El perfil debe estar completo para adoptar una mascota.");
            }
        }

        public void InscribirseEnActividad(ActividadVoluntariado actividad)
        {
            if (PerfilCompleto)
            {
                ActividadesInscritas.Add(actividad);
            }
            else
            {
                throw new InvalidOperationException("El perfil debe estar completo para inscribirse en una actividad de voluntariado.");
            }
        }
    }
}

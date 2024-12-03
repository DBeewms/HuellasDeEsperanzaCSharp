using HuellasDeEsperanzaC_.Models;
using HuellasDeEsperanzaC_.Servicio;
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
    public partial class ConfiguracionForm : Form
    {
        private Usuario usuarioActual;
        private GestorAdopcion gestorAdopcionUser;  

        public ConfiguracionForm(Usuario usuario, GestorAdopcion gestorAdopcion)
        {
            InitializeComponent();
            this.usuarioActual = usuario;
            this.gestorAdopcionUser = gestorAdopcion;
            MostrarDatosUsuario();
        }

        private void MostrarDatosUsuario()
        {
            tbNombreCompleto.Texts = usuarioActual.NombreCompleto;
            tbEmail.Texts = usuarioActual.CorreoElectronico;
            tbDireccion.Texts = usuarioActual.Direccion;
            tbNumeroTelefono.Texts = usuarioActual.NumeroTelefono;
            tbNumeroCedula.Texts = usuarioActual.NumeroCedula;
            tbOcupacion.Texts = usuarioActual.Ocupacion;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string nombreCompleto = tbNombreCompleto.Texts.Trim();
            string correo = tbEmail.Texts.Trim();
            string direccion = tbDireccion.Texts.Trim();
            string numeroTelefono = tbNumeroTelefono.Texts.Trim();
            string numeroCedula = tbNumeroCedula.Texts.Trim();
            string ocupacion = tbOcupacion.Texts.Trim();

            // Validar campos obligatorios
            if (string.IsNullOrEmpty(nombreCompleto) || nombreCompleto.Length < 10)
            {
                MetroFramework.MetroMessageBox.Show(this, "El nombre completo no puede estar vacío y debe tener al menos 10 caracteres", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbNombreCompleto.Focus();
                return;
            }

            // Validar correo electrónico
            if (string.IsNullOrEmpty(correo) || !EsCorreoValido(correo))
            {
                MetroFramework.MetroMessageBox.Show(this, "Ingrese un correo electrónico válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbEmail.Focus();
                return;
            }

            // Verificar si el correo ya está tomado, excepto si es el mismo que el del usuario actual
            GestorUsuario gestorUsuario = new GestorUsuario();
            if (correo != usuarioActual.CorreoElectronico && gestorUsuario.CorreoElectronicoExiste(correo))
            {
                MetroFramework.MetroMessageBox.Show(this, "El correo electrónico ya está en uso", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbEmail.Focus();
                return;
            }

            // Validar dirección
            if (string.IsNullOrEmpty(direccion) || direccion.Length < 15)
            {
                MetroFramework.MetroMessageBox.Show(this, "La dirección debe tener al menos 15 caracteres", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbDireccion.Focus();
                return;
            }

            // Validar número de teléfono
            if (string.IsNullOrEmpty(numeroTelefono) || !EsTelefonoValido(numeroTelefono))
            {
                MetroFramework.MetroMessageBox.Show(this, "El número de teléfono debe tener el formato ####-####", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbNumeroTelefono.Focus();
                return;
            }

            // Validar ocupación
            if (string.IsNullOrEmpty(ocupacion) || ocupacion.Length < 10)
            {
                MetroFramework.MetroMessageBox.Show(this, "La ocupación debe tener al menos 10 caracteres", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbOcupacion.Focus();
                return;
            }

            bool hayCambios = false;

            if (nombreCompleto != usuarioActual.NombreCompleto ||
                direccion != usuarioActual.Direccion ||
                numeroTelefono != usuarioActual.NumeroTelefono ||
                numeroCedula != usuarioActual.NumeroCedula ||
                ocupacion != usuarioActual.Ocupacion)
            {
                usuarioActual.CompletarPerfilUsuario(nombreCompleto, direccion, numeroTelefono, numeroCedula, ocupacion);
                hayCambios = true;
            }

            if (correo != usuarioActual.CorreoElectronico)
            {
                usuarioActual.CorreoElectronico = correo;
                hayCambios = true;
            }

            if (!hayCambios)
            {
                MetroFramework.MetroMessageBox.Show(this, "No hay cambios para guardar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            gestorUsuario.ActualizarUsuario(usuarioActual, this, gestorAdopcionUser);

            this.Close();
        }

        private bool EsCorreoValido(string correo)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(correo);
                return addr.Address == correo;
            }
            catch
            {
                return false;
            }
        }

        private bool EsTelefonoValido(string telefono)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(telefono, @"^\d{4}-\d{4}$");
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            HomeGeneralForm homeForm = new HomeGeneralForm(usuarioActual, gestorAdopcionUser);
            homeForm.Show();
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

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblNumeroTelefono_Click(object sender, EventArgs e)
        {

        }

        private void lblDireccion_Click(object sender, EventArgs e)
        {

        }

        private void lblOcupacion_Click(object sender, EventArgs e)
        {

        }

        private void lblNumeroCedula_Click(object sender, EventArgs e)
        {

        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

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
using HuellasDeEsperanzaC_.FormsTOH;
using HuellasDeEsperanzaC_.Models;
using HuellasDeEsperanzaC_.Servicio;

namespace HuellasDeEsperanzaC_.FormsTOH
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
            this.ActiveControl = tbNombreCompleto;
            this.Size = new Size(902, 430);
            roundButton1.Location = new Point(710, 332);
            
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (isORA.Checked)
            {
                this.Size = new Size(902, 653);
                lblORA1.Visible = true;
                lblORA2.Visible = true;
                lblORA3.Visible = true;
                tbOra1.Visible = true;
                tbOra2.Visible = true;
                tbOra3.Visible = true;
                roundButton1.Location = new Point(710, 569);
            }
            else
            {
                this.Size = new Size(902, 430);
                lblORA1.Visible = false;
                lblORA2.Visible = false;
                lblORA3.Visible = false;
                tbOra1.Visible = false;
                tbOra2.Visible = false;
                tbOra3.Visible = false;
                roundButton1.Location = new Point(710, 332);
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void customTextBox3__TextChanged(object sender, EventArgs e)
        {

        }

        private void roundButton1_Click(object sender, EventArgs e)
        {
            Usuario usuario = new Usuario();
            GestorAdopcion gestorAdopcion = new GestorAdopcion();
            GestorUsuario GestorUsuario = new GestorUsuario();

            string nombreCompleto = tbNombreCompleto.Texts.Trim();
            string correoElectronico = tbEmail.Texts.Trim();
            string contraseña = tbPass.Texts;

            usuario.NombreCompleto = nombreCompleto;
            usuario.CorreoElectronico = correoElectronico;

            // Validar campos obligatorios
            if (string.IsNullOrEmpty(nombreCompleto) || string.IsNullOrEmpty(correoElectronico) || string.IsNullOrEmpty(contraseña))
            {
                MetroFramework.MetroMessageBox.Show(this, "Campos obligatorios no pueden estar vacíos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbNombreCompleto.Focus();
                return;
            }

            // Validar nombre completo
            if (string.IsNullOrEmpty(nombreCompleto) || nombreCompleto.Length < 10)
            {
                MetroFramework.MetroMessageBox.Show(this, "El nombre completo no puede estar vacío y debe tener al menos 10 caracteres", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbNombreCompleto.Focus();
                return;
            }

            // Validar correo electrónico
            if (string.IsNullOrEmpty(correoElectronico) || !EsCorreoValido(correoElectronico))
            {
                MetroFramework.MetroMessageBox.Show(this, "Ingrese un correo electrónico válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbEmail.Focus();
                return;
            }

            // Validar contraseña
            if (string.IsNullOrEmpty(contraseña))
            {
                MetroFramework.MetroMessageBox.Show(this, "La contraseña no puede estar vacía", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbPass.Focus();
                return;
            }
            else if (contraseña.Length < 11)
            {
                MetroFramework.MetroMessageBox.Show(this, "La contraseña debe tener al menos 11 caracteres", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbPass.Focus();
                return;
            }
            else
            {
                usuario.EstablecerContraseña(contraseña);
            }

            if (isORA.Checked)
            {
                usuario.Direccion = tbOra1.Texts.Trim();
                usuario.NumeroTelefono = tbOra2.Texts.Trim();
                usuario.Descripcion = tbOra3.Texts.Trim();
                usuario.Tipo = TipoUsuario.Organizacion;

                // Validar campos adicionales para organizaciones
                if (string.IsNullOrEmpty(usuario.Direccion) || string.IsNullOrEmpty(usuario.NumeroTelefono) || string.IsNullOrEmpty(usuario.Descripcion))
                {
                    MetroFramework.MetroMessageBox.Show(this, "Por favor llene todos los campos para la organización", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tbOra1.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(usuario.Direccion) || usuario.Direccion.Length < 15)
                {
                    MetroFramework.MetroMessageBox.Show(this, "La dirección debe tener al menos 15 caracteres", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tbOra1.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(usuario.NumeroTelefono) || !EsTelefonoValido(usuario.NumeroTelefono))
                {
                    MetroFramework.MetroMessageBox.Show(this, "El número de teléfono debe tener el formato ####-####", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tbOra2.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(usuario.Descripcion) || usuario.Descripcion.Length < 30)
                {
                    MetroFramework.MetroMessageBox.Show(this, "La descripción debe tener al menos 30 caracteres", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tbOra3.Focus();
                    return;
                }
            }
            else
            {
                usuario.Tipo = TipoUsuario.Comun;
            }

            // Verificar si el correo ya está registrado
            GestorUsuario.CargarDatosUsuarios(); // Asegura que los datos estén cargados
            Usuario usuarioExistente = GestorUsuario.BuscarUsuarioPorCorreo(correoElectronico);
            if (usuarioExistente != null)
            {
                MetroFramework.MetroMessageBox.Show(this, "El correo electrónico ya está registrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbEmail.Focus();
                return;
            }

            // Registrar usuario
            GestorUsuario.RegistrarUsuario(usuario, this, gestorAdopcion);

            // Redirigir al formulario Home
            HomeGeneralForm home = new HomeGeneralForm(usuario, gestorAdopcion);
            home.Show();
            this.Hide();
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

        private void roundButton2_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Hide();
        }

        private void tbNombreCompleto_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                tbEmail.Focus();
            }
        }

        private void tbEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                tbPass.Focus();
            }
        }

        private void isORA_Click(object sender, EventArgs e)
        {
            tbOra1.Focus();
        }

        private void tbOra2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                tbOra3.Focus();
            }
        }

        private void tbOra3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                roundButton1.PerformClick();
            }
        }

        private void tbPass_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                roundButton1.PerformClick();
            }
        }

        private void tbOra1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                tbOra2.Focus();
            }
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

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

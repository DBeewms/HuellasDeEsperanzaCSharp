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
    public partial class HomeGeneralForm : Form
    {
        private Usuario usuarioActual;
        private GestorAdopcion gestorAdopcionUser;

        public HomeGeneralForm(Usuario usuario, GestorAdopcion gestorAdopcion)
        {
            InitializeComponent();
            this.usuarioActual = usuario;
            this.gestorAdopcionUser = gestorAdopcion;
            // showUsuario();
            SetSaludo();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AdoptMascot adoptMascot = new AdoptMascot(usuarioActual, gestorAdopcionUser);
            adoptMascot.Show();
            this.Hide();
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void roundButton1_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Close();
        }

        // public void showUsuario()
        // {
        //     listBox1.Items.Clear();
        //     listBox1.Items.Add(usuarioActual.NombreCompleto);
        //     listBox1.Items.Add(usuarioActual.CorreoElectronico);
        //     listBox1.Items.Add(usuarioActual.Tipo);
        //     listBox1.Items.Add(usuarioActual.Direccion);
        //     listBox1.Items.Add(usuarioActual.NumeroTelefono);
        //     if (usuarioActual.NumeroCedula != null)
        //     {
        //         listBox1.Items.Add(usuarioActual.NumeroCedula);
        //     }
        //     if (usuarioActual.Ocupacion != null)
        //     {
        //         listBox1.Items.Add(usuarioActual.Ocupacion);
        //     }
        //     listBox1.Items.Add(usuarioActual.Descripcion);
        // }

        private void HomeGeneralForm_Load(object sender, EventArgs e)
        {
            gestorAdopcionUser.CargarDatosSolicitudes();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddMascot addMascot = new AddMascot(usuarioActual, gestorAdopcionUser);
            addMascot.Show();
            this.Hide();
        }

        private void panel5_Paint_1(object sender, PaintEventArgs e)
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

        private void btnConfiguracion_Click_1(object sender, EventArgs e)
        {
            ConfiguracionForm configuracionForm = new ConfiguracionForm(usuarioActual, gestorAdopcionUser);
            configuracionForm.ShowDialog();
            // TODO
            // Actualizar saludo después de configurar
            SetSaludo();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            WaitingListForm waitingListForm = new WaitingListForm(usuarioActual, gestorAdopcionUser);
            waitingListForm.Show();
            this.Hide();
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
            lblSaludo.Text = $"{saludo}, {usuarioActual.NombreCompleto.Split(' ')[0]}!";
        }
    }
}

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
    public class GestorMascota
    {
        List<Mascota> mascotas = new List<Mascota>();

        public void RegistrarMascota(Mascota mascota, Form formulario, bool esEdicion = false)
        {
            mascotas.Clear();
            CargarDatosMascotas();

            if (esEdicion)
            {
                // Buscar y actualizar la mascota existente según los datos cargados
                var mascotaExistente = mascotas.FirstOrDefault(m => m.Id == mascota.Id);
                if (mascotaExistente != null)
                {
                    int index = mascotas.IndexOf(mascotaExistente);
                    mascotas[index] = mascota;
                }
            }
            else
            {
                // Asignar un nuevo ID a la mascota
                if (mascotas.Count > 0)
                {
                    int maxId = mascotas.Max(m => m.Id);
                    mascota.Id = maxId + 1;
                }
                else
                {
                    mascota.Id = 1;
                }
                mascotas.Add(mascota);
            }

            EscribirArchivoMascotas();

            string mensaje;
            string titulo;

            if (esEdicion)
            {
                mensaje = "Mascota actualizada exitosamente";
                titulo = "Actualización exitosa";
            }
            else
            {
                mensaje = "Mascota registrada exitosamente";
                titulo = "Registro exitoso";
            }

            MetroFramework.MetroMessageBox.Show(formulario, mensaje, titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public Mascota BuscarMascotaPorID(int id)
        {
            return mascotas.FirstOrDefault(m => m.Id == id);
        }

        public Mascota BuscarMascotaPorNombre(string nombre)
        {
            return mascotas.FirstOrDefault(m => string.Equals(m.Nombre, nombre, StringComparison.OrdinalIgnoreCase));
        }

        public Mascota BuscarMascotaPorRaza(string raza)
        {
            return mascotas.FirstOrDefault(m => string.Equals(m.Raza, raza, StringComparison.OrdinalIgnoreCase));
        }

        public Mascota BuscarMascotaPorSexo(string sexo)
        {
            return mascotas.FirstOrDefault(m => string.Equals(m.Sexo, sexo, StringComparison.OrdinalIgnoreCase));
        }

        public Mascota BuscarMascotaPorEspecie(string especie)
        {
            return mascotas.FirstOrDefault(m => string.Equals(m.Especie, especie, StringComparison.OrdinalIgnoreCase));
        }

        public void EscribirArchivoMascotas()
        {
            using (FileStream mArchivoEscritor = new FileStream("mascotas.dat", FileMode.Create, FileAccess.Write))
            using (BinaryWriter Escritor = new BinaryWriter(mArchivoEscritor))
            {
                foreach (var mascota in mascotas)
                {
                    Escritor.Write(mascota.Id);
                    Escritor.Write(mascota.Nombre);
                    Escritor.Write(mascota.Especie);
                    Escritor.Write(mascota.Sexo);
                    Escritor.Write(mascota.Raza);
                    Escritor.Write(mascota.FechaNacimiento.ToBinary());
                    Escritor.Write(mascota.Descripcion);
                    Escritor.Write(mascota.RutaImagen ?? string.Empty);
                    Escritor.Write(mascota.EstaAdoptado);
                    Escritor.Write(mascota.AdoptanteId);
                }
            }
        }

        public void CargarDatosMascotas()
        {
            mascotas.Clear();

            if (!File.Exists("mascotas.dat"))
            {
                return;
            }

            using (FileStream mArchivoLector = new FileStream("mascotas.dat", FileMode.Open, FileAccess.Read))
            using (BinaryReader Lector = new BinaryReader(mArchivoLector))
            {
                while (mArchivoLector.Position < mArchivoLector.Length)
                {
                    Mascota mascota = new Mascota
                    {
                        Id = Lector.ReadInt32(),
                        Nombre = Lector.ReadString(),
                        Especie = Lector.ReadString(),
                        Sexo = Lector.ReadString(),
                        Raza = Lector.ReadString(),
                        FechaNacimiento = DateTime.FromBinary(Lector.ReadInt64()),
                        Descripcion = Lector.ReadString(),
                        RutaImagen = Lector.ReadString(),
                        EstaAdoptado = Lector.ReadBoolean(),
                        AdoptanteId = Lector.ReadInt32()
                    };

                    string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                    string fullPath = Path.Combine(projectDirectory, mascota.RutaImagen);
                    if (File.Exists(fullPath))
                    {
                        mascotas.Add(mascota);
                    }
                    else
                    {
                        MessageBox.Show("La imagen no se encontró: " + fullPath);
                    }
                }
            }
        }

        public void ActualizarMascota(Mascota mascota, Form formulario)
        {
            RegistrarMascota(mascota, formulario, true);
        }

        public void ActualizarEstadoMascota(Mascota mascota)
        {
            // Buscar la mascota en la lista y actualizar su estado
            var mascotaExistente = mascotas.FirstOrDefault(m => m.Id == mascota.Id);
            if (mascotaExistente != null)
            {
                int index = mascotas.IndexOf(mascotaExistente);
                mascotas[index] = mascota;
                EscribirArchivoMascotas();
            }
        }

        public string SeleccionarYGuardarImagen(Form formulario)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Archivos de imagen (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string sourceFilePath = openFileDialog.FileName;
                    string fileName = Path.GetFileName(sourceFilePath);
                    string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                    string targetDirectory = Path.Combine(projectDirectory, "FotosMascotas");

                    if (!Directory.Exists(targetDirectory))
                    {
                        Directory.CreateDirectory(targetDirectory);
                    }

                    string targetFilePath = Path.Combine(targetDirectory, fileName);
                    File.Copy(sourceFilePath, targetFilePath, true);

                    // Devolver la ruta relativa
                    return Path.Combine("FotosMascotas", fileName);
                }
            }

            return string.Empty;
        }

        public void SetListaMascotas(List<Mascota> nuevasMascotas)
        {
            mascotas = nuevasMascotas;
            EscribirArchivoMascotas();
        }

        public List<Mascota> GetListaMascotas()
        {
            return mascotas;
        }
    }
}

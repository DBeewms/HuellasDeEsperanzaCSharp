using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuellasDeEsperanzaC_.Models
{
    public class Mascota
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Raza { get; set; }
        public string Sexo { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int Edad { get; set; }

        public string Especie { get; set; }
        public string Tamaño { get; set; }
        public string Descripcion { get; set; }
        public string RutaImagen { get; set; }
        public bool EstaAdoptado { get; set; }
        public int AdoptanteId { get; set; }
        public int OrganizacionId { get; set; }

        public Mascota()
        {
            Nombre = string.Empty;
            Raza = string.Empty;
            Sexo = string.Empty;
            FechaNacimiento = DateTime.MinValue;
            Edad = 0;
            Especie = string.Empty;
            Tamaño = string.Empty;
            Descripcion = string.Empty;
            RutaImagen = string.Empty;
            EstaAdoptado = false;
            AdoptanteId = 0;
            OrganizacionId = 0;
        }

        public int CalcularEdad()
        {
            return DateTime.Now.Year - FechaNacimiento.Year;
        }

        public string ObtenerEdad()
        {
            int años = CalcularEdad();
            if (años == 0)
            {
                int meses = DateTime.Now.Month - FechaNacimiento.Month;
                if (meses == 0)
                {
                    return "1 mes";
                }
                return meses + " meses";
            }
            return años + " años";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErickRualesComplementario.Modelos
{
    public class Estudiantes
    {
        public int cod_estudiante {  get; set; }
        public string nombre {  get; set; }
        public string apellido { get; set; }
        public string curso { get; set; }
        public string paralelo { get; set; }
        public float nota_final {  get; set; }
    }
}

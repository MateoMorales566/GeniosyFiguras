using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeniosyFiguras.Dtos
{
    public class UsuarioConCalificacionDto
    {
        public int IdCalificacion { get; set; }
        public UsuarioDto Usuario { get; set; } = new UsuarioDto();

        public decimal NotaMatematicas { get; set; }
        public decimal NotaSociales { get; set; }
        public decimal NotaCiencias { get; set; }
        public decimal NotaArtes { get; set; }
        public decimal NotaLenguas { get; set; }
        public int IdCurso { get; set; }
    }

}
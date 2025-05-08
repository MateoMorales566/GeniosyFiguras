using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeniosyFiguras.Dtos
{
    public class CursoDto
    {
        public int IdCurso { get; set; }
        public string NombreCurso { get; set; }
        public int NumeroEstudiantes { get; set; }
        public int IdUsuario { get; set; }
        public string UsuarioCreador { get; set; }


        public int Response { get; set; }
        public string Message { get; set; }
    }
}
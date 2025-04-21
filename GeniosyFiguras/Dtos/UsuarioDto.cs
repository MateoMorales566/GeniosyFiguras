using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeniosyFiguras.Dtos
{
    public class UsuarioDto
    {
        public int IdUsuario { get; set; }
       
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Email{ get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string Contraseña { get; set; } = string.Empty;
        public int IdRol { get; set; }


        public int Response { get; set; }
        public string Message { get; set; } = string.Empty;

    }
}
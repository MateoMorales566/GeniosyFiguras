using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeniosyFiguras.Dtos
{
    public class EnemigoDto
    {
        public int IdEnemigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
        public int IdAtributoPoder { get; set; }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using GeniosyFiguras.Dtos;
using GeniosyFiguras.Repositories.Models;

namespace GeniosyFiguras.Repositories.Models
{
    public class EnemigoAtributo
    {
        public EnemigoDto Enemigo { get; set; }
        public Atributo_Poder Atributos { get; set; }
    }
}

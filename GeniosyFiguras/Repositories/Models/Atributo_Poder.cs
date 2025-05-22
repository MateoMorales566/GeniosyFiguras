using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeniosyFiguras.Repositories.Models
{
    public class Atributo_Poder
    {
        public decimal Fuerza { get; set; }
        public decimal Velocidad { get; set; }
        public decimal Durabilidad { get; set; }
        public decimal Inteligencia { get; set; }
        public decimal Mana { get; set; }
        public decimal Salud { get; set; }
        public int IdCalificacion { get; set; }
        public int Tipo { get; set; }  

    }
}
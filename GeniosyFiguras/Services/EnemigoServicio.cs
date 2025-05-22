using GeniosyFiguras.Dtos;
using GeniosyFiguras.Repositories;
using GeniosyFiguras.Repositories.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeniosyFiguras.Services
{
    public class EnemigoServicio
    {
        private readonly EnemigoRepositorio _enemigoRepositorio = new EnemigoRepositorio();

        public void Crear(EnemigoAtributo model)
        {
            _enemigoRepositorio.Crear(model);
        }
        public List<EnemigoAtributo> ObtenerTodos()
        {
            return _enemigoRepositorio.ObtenerTodos();
        }


    }

}
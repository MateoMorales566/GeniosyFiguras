using GeniosyFiguras.Dtos;
using GeniosyFiguras.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeniosyFiguras.Services
{
    public class AdministradorServicio
    {
        private readonly AdministradorRepositorio _repositorio;

        public AdministradorServicio()
        {
            _repositorio = new AdministradorRepositorio();
        }

        public List<UsuarioDto> ObtenerProfesores()
        {
            return _repositorio.ObtenerProfesores();
        }

    }
}
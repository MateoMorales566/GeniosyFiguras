using GeniosyFiguras.Dtos;
using GeniosyFiguras.Repositories;
using GeniosyFiguras.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GeniosyFiguras.Services
{
    public class CursoServicio
    {
        public List<CursoDto> ObtenerTodos()
        {
            CursoRepositorio cursoRepositorio = new CursoRepositorio();
            return cursoRepositorio.ObtenerTodos();
        }
        public CursoDto CrearCurso(CursoDto cursoModel)
        {
            CursoDto response = new CursoDto();
            CursoRepositorio repo = new CursoRepositorio();
            try
            {
                cursoModel.IdUsuario = 2;
                if (repo.CrearCurso(cursoModel) > 0)
                {
                    response.Response = 1;
                    response.Message = "Curso creado correctamente";
                }
                else
                {
                    response.Response = 0;
                    response.Message = "No se pudo crear el curso";
                }
            }
            catch (Exception e)
            {
                response.Response = 0;
                response.Message = e.Message;
            }
            return response;
        }
        public List<CursoDto> ObtenerCursosPorUsuario(string nombreUsuario)
        {
            CursoRepositorio repo = new CursoRepositorio();
            return repo.ObtenerCursosPorUsuario(nombreUsuario);
        }

        public CursoDto ActualizarCurso(CursoDto curso)
        {
            CursoRepositorio repositorio = new CursoRepositorio();
            int filas = repositorio.ActualizarCurso(curso);

            curso.Response = filas > 0 ? 1 : 0;
            curso.Message = filas > 0 ? "Curso actualizado correctamente" : "Error al actualizar el curso";

            return curso;
        }

        public CursoDto EliminarCurso(int id)
        {
            CursoRepositorio repo = new CursoRepositorio();
            return repo.EliminarCurso(id);
        }
        private readonly CursoRepositorio _cursoRepositorio = new CursoRepositorio();

        public void AsignarEstudianteACurso(int idUsuario, int idCurso)
        {
            _cursoRepositorio.AsignarEstudianteACurso(idUsuario, idCurso);
        }



    }




}
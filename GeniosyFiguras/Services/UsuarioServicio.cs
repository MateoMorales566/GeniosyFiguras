using GeniosyFiguras.Dtos;
using GeniosyFiguras.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeniosyFiguras.Services
{
    public class UsuarioServicio

    {
        private readonly UsuarioRepositorio _usuarioRepositorio = new UsuarioRepositorio();
        public UsuarioDto CreateUsuario(UsuarioDto usuarioModel)
        {

            UsuarioDto responseUsuarioDto = new UsuarioDto();
            UsuarioRepositorio usuarioRepositorio = new UsuarioRepositorio();
            try
            {
                usuarioModel.IdRol = 2;
                if(usuarioRepositorio.CreateUsuario(usuarioModel)!=0)
                {
                    responseUsuarioDto.Response = 1;
                    responseUsuarioDto.Message = "Creacion exitosa";
                    if (usuarioModel.IdRol == 2 && !string.IsNullOrEmpty(usuarioModel.Email))
                    {
                        usuarioRepositorio.EnviarCorreoConfirmacion(usuarioModel.Email, usuarioModel.Nombres);
                    }

                }
                else
                {
                    responseUsuarioDto.Response = 0;
                    responseUsuarioDto.Message = "Algo paso";
                }

             
                return responseUsuarioDto;

            }
            catch(Exception e)
            {
                responseUsuarioDto.Response = 0;
                responseUsuarioDto.Message = e.InnerException.ToString();
                return responseUsuarioDto;
            }
        }

        public List<UsuarioConCalificacionDto> ObtenerUsuariosPorCursoConNotas(int idCurso)
        {
            return new UsuarioRepositorio().ObtenerUsuariosPorCursoConNotas(idCurso);
        }

        public List<UsuarioDto> ObtenerTodos()
        {
            return new UsuarioRepositorio().ObtenerTodos();
        }

        public int CrearUsuarioYObtenerId(UsuarioDto usuario)
        {
            return new UsuarioRepositorio().InsertarYDevolverId(usuario);
        }

        public void CrearEstudianteConCalificaciones(UsuarioConCalificacionDto modelo)
        {
            var idUsuario = _usuarioRepositorio.InsertarYDevolverId(modelo.Usuario);
            _usuarioRepositorio.InsertarCalificacion(modelo, idUsuario);
        }

        public UsuarioConCalificacionDto ObtenerUsuarioConNotas(int idUsuario)
        {
            return _usuarioRepositorio.ObtenerUsuarioConNotas(idUsuario);
        }

        public void ActualizarUsuarioConNotas(UsuarioConCalificacionDto usuario)
        {
            _usuarioRepositorio.ActualizarUsuarioConNotas(usuario);
        }

        public void EliminarUsuarioYNotas(int idUsuario)
        {
            _usuarioRepositorio.EliminarUsuarioYNotas(idUsuario);
        }




    }
}
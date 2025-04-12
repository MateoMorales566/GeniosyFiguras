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
    }
}
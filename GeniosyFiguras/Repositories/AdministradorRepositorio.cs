using GeniosyFiguras.Dtos;
using GeniosyFiguras.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GeniosyFiguras.Repositories
{
    public class AdministradorRepositorio
    {
        public List<UsuarioDto> ObtenerProfesores()
        {
            var lista = new List<UsuarioDto>();

            using (SqlConnection conn = new SqlConnection(DBContextUtility.CadenaConexion))
            {
                string query = "SELECT IdUsuario, Nombres, Apellidos, Usuario FROM Usuario WHERE IdRol = 2";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new UsuarioDto
                    {
                        IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                        Nombres = dr["Nombres"].ToString(),
                        Apellidos = dr["Apellidos"].ToString(),
                        NombreUsuario = dr["Usuario"].ToString()
                    });
                }
            }

            return lista;
        }

    }
}
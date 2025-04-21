using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using GeniosyFiguras.Dtos;
using GeniosyFiguras.Utilities;

namespace GeniosyFiguras.Repositories
{
    public class UsuarioRepositorio
    {
        public int CreateUsuario(UsuarioDto usuario)
        {
            int comando = 0;
            
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();
            //consulta SQL
            string SQL = "INSERT INTO APP.DBO.[Usuario](Nombres,Apellidos,Email,Telefono,Usuario,Contraseña,IdRol)"
                + "VALUES ('" + usuario.Nombres + "','" + usuario.Apellidos + "','" + usuario.Email + "','" + usuario.Telefono + "','" + usuario.NombreUsuario + "','"
                + usuario.Contraseña + "'," + usuario.IdRol + ");";
            using (SqlCommand command= new SqlCommand(SQL, Connection.CONN()))
            {
                comando = command.ExecuteNonQuery();


            }
            Connection.Disconnect();

            return comando;
        }
    }
}
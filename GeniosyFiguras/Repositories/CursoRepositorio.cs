using GeniosyFiguras.Dtos;
using GeniosyFiguras.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GeniosyFiguras.Repositories
{
    public class CursoRepositorio
    {
        public List<CursoDto> ObtenerTodos()
        {
            List<CursoDto> listaCursos = new List<CursoDto>();
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            string SQL = "SELECT IdCurso, NombreCurso, NumeroEstudiantes, IdUsuario FROM APP.DBO.Curso";

            using (SqlCommand command = new SqlCommand(SQL, Connection.CONN()))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CursoDto curso = new CursoDto()
                        {
                            IdCurso = Convert.ToInt32(reader["IdCurso"]),
                            NombreCurso = reader["NombreCurso"].ToString(),
                            NumeroEstudiantes = Convert.ToInt32(reader["NumeroEstudiantes"]),
                            IdUsuario = Convert.ToInt32(reader["IdUsuario"])
                        };
                        listaCursos.Add(curso);
                    }
                }
            }

            Connection.Disconnect();
            return listaCursos;
        }
        public int CrearCurso(CursoDto curso)
        {
            int filas = 0;
            DBContextUtility connection = new DBContextUtility();
            connection.Connect();

            string SQL = "INSERT INTO APP.DBO.Curso (NombreCurso, NumeroEstudiantes, IdUsuario, UsuarioCreador) " +
                         "VALUES (@NombreCurso, @NumeroEstudiantes, @IdUsuario, @UsuarioCreador)";

            using (SqlCommand command = new SqlCommand(SQL, connection.CONN()))
            {
                command.Parameters.AddWithValue("@NombreCurso", curso.NombreCurso);
                command.Parameters.AddWithValue("@NumeroEstudiantes", curso.NumeroEstudiantes);
                command.Parameters.AddWithValue("@IdUsuario", curso.IdUsuario);
                command.Parameters.AddWithValue("@UsuarioCreador", curso.UsuarioCreador);

                filas = command.ExecuteNonQuery();
            }

            connection.Disconnect();
            return filas;
        }

        public List<CursoDto> ObtenerCursosPorUsuario(string nombreUsuario)
        {
            List<CursoDto> listaCursos = new List<CursoDto>();
            DBContextUtility conexion = new DBContextUtility();
            conexion.Connect();

            string sql = @"SELECT C.IdCurso, C.NombreCurso, C.NumeroEstudiantes, C.IdUsuario, C.UsuarioCreador
               FROM APP.DBO.Curso C
               WHERE C.UsuarioCreador = @NombreUsuario";


            using (SqlCommand cmd = new SqlCommand(sql, conexion.CONN()))
            {
                cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CursoDto curso = new CursoDto()
                        {
                            IdCurso = Convert.ToInt32(reader["IdCurso"]),
                            NombreCurso = reader["NombreCurso"].ToString(),
                            NumeroEstudiantes = Convert.ToInt32(reader["NumeroEstudiantes"]),
                            IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                            UsuarioCreador = reader["UsuarioCreador"].ToString() // <-- Agregado aquí
                        };
                        listaCursos.Add(curso);
                    }
                }
            }

            conexion.Disconnect();
            return listaCursos;
        }

        public int ActualizarCurso(CursoDto curso)
        {
            int filas = 0;
            DBContextUtility connection = new DBContextUtility();
            connection.Connect();

            string sql = @"UPDATE APP.DBO.Curso
                   SET NombreCurso = @NombreCurso,
                       NumeroEstudiantes = @NumeroEstudiantes
                   WHERE IdCurso = @IdCurso";

            using (SqlCommand command = new SqlCommand(sql, connection.CONN()))
            {
                command.Parameters.AddWithValue("@NombreCurso", curso.NombreCurso);
                command.Parameters.AddWithValue("@NumeroEstudiantes", curso.NumeroEstudiantes);
                command.Parameters.AddWithValue("@IdCurso", curso.IdCurso);

                filas = command.ExecuteNonQuery();
            }

            connection.Disconnect();
            return filas;
        }
        public CursoDto EliminarCurso(int id)
        {
            DBContextUtility conexion = new DBContextUtility();
            conexion.Connect();

            CursoDto response = new CursoDto();

            string sql = @"DELETE FROM APP.DBO.Curso WHERE IdCurso = @IdCurso";

            using (SqlCommand cmd = new SqlCommand(sql, conexion.CONN()))
            {
                cmd.Parameters.AddWithValue("@IdCurso", id);

                int filasAfectadas = cmd.ExecuteNonQuery();

                response.Response = filasAfectadas > 0 ? 1 : 0;
                response.Message = filasAfectadas > 0 ? "Curso eliminado correctamente" : "Error al eliminar el curso";

                return response;
            }
        }
        // CursoRepositorio.cs
        public void AsignarEstudianteACurso(int idUsuario, int idCurso)
        {
            using (var conn = new SqlConnection(DBContextUtility.CadenaConexion))
            {
                conn.Open();
                var query = @"INSERT INTO Calificacion (IdUsuario, IdCurso, NotaMatematicas, NotaSociales, NotaCiencias, NotaArtes, NotaLenguas)
                      VALUES (@IdUsuario, @IdCurso, 0, 0, 0, 0, 0)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@IdCurso", idCurso);
                    cmd.ExecuteNonQuery();
                }
            }
        }








    }

}
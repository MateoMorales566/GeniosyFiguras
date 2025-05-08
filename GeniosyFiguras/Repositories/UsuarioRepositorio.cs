using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using GeniosyFiguras.Dtos;
using GeniosyFiguras.Utilities;
using System.Net.Mail;
using System.Net;

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

        public void EnviarCorreoConfirmacion(string destinatario, string nombre)
        {
            var correo = new MailMessage();
            correo.From = new MailAddress("geniosyfiguras@gmail.com");
            correo.To.Add(destinatario);
            correo.Subject = "Registro exitoso";
            correo.Body = $"Hola {nombre}, tu cuenta ha sido creada exitosamente.";
            correo.IsBodyHtml = false;

            var smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("geniosyfiguras566@gmail.com", "fazqkgfvlobnfext");
            smtp.EnableSsl = true;

            smtp.Send(correo);
        }

        public List<UsuarioConCalificacionDto> ObtenerUsuariosPorCursoConNotas(int idCurso)
        {
            var lista = new List<UsuarioConCalificacionDto>();

            using (var conexion = new SqlConnection(DBContextUtility.CadenaConexion))

            {
                conexion.Open();

                var cmd = new SqlCommand(@"
                SELECT u.IdUsuario, u.Nombres, u.Apellidos, u.Usuario, u.Contraseña,
                       c.NotaMatematicas, c.NotaSociales, c.NotaCiencias, c.NotaArtes, c.NotaLenguas,
                       c.IdCurso
                FROM Usuario u
                INNER JOIN Calificacion c ON u.IdUsuario = c.IdUsuario
                WHERE c.IdCurso = @IdCurso", conexion);

                cmd.Parameters.AddWithValue("@IdCurso", idCurso);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var dto = new UsuarioConCalificacionDto
                        {
                            Usuario = new UsuarioDto
                            {
                                IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                                Nombres = reader["Nombres"].ToString(),
                                Apellidos = reader["Apellidos"].ToString(),
                                NombreUsuario = reader["Usuario"].ToString(),
                                Contraseña = reader["Contraseña"].ToString()
                            },
                            NotaMatematicas = Convert.ToDecimal(reader["NotaMatematicas"]),
                            NotaSociales = Convert.ToDecimal(reader["NotaSociales"]),
                            NotaCiencias = Convert.ToDecimal(reader["NotaCiencias"]),
                            NotaArtes = Convert.ToDecimal(reader["NotaArtes"]),
                            NotaLenguas = Convert.ToDecimal(reader["NotaLenguas"]),
                            IdCurso = Convert.ToInt32(reader["IdCurso"])
                        };

                        lista.Add(dto);
                    }
                }
            }

            return lista;
        }
        public List<UsuarioDto> ObtenerTodos()
        {
            var lista = new List<UsuarioDto>();
            using (var conn = new SqlConnection(DBContextUtility.CadenaConexion))
            {
                conn.Open();
                var query = "SELECT IdUsuario, Nombres, Apellidos, Usuario, Contraseña FROM Usuario";
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new UsuarioDto
                        {
                            IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                            Nombres = reader["Nombres"].ToString(),
                            Apellidos = reader["Apellidos"].ToString(),
                            NombreUsuario = reader["Usuario"].ToString(),
                            Contraseña = reader["Contraseña"].ToString()
                        });
                    }
                }
            }
            return lista;
        }

        public int InsertarYDevolverId(UsuarioDto usuario)
        {
            int idGenerado = 0;

            using (var conn = new SqlConnection(DBContextUtility.CadenaConexion))
            {
                conn.Open();

                var query = @"INSERT INTO Usuario (Nombres, Apellidos, Usuario, Contraseña, IdRol)
                      OUTPUT INSERTED.IdUsuario
                      VALUES (@Nombres, @Apellidos, @Usuario, @Contraseña, @IdRol)";

                using (var cmd = new SqlCommand(query, conn))
                {

                    string hash = BCrypt.Net.BCrypt.HashPassword(usuario.Contraseña);

                    cmd.Parameters.AddWithValue("@Nombres", usuario.Nombres);
                    cmd.Parameters.AddWithValue("@Apellidos", usuario.Apellidos);
                    cmd.Parameters.AddWithValue("@Usuario", usuario.NombreUsuario);
                    cmd.Parameters.AddWithValue("@Contraseña", hash);
                    cmd.Parameters.AddWithValue("@IdRol", usuario.IdRol);

                    idGenerado = (int)cmd.ExecuteScalar();
                }
            }

            return idGenerado;
        }

        public void InsertarCalificacion(UsuarioConCalificacionDto modelo, int idUsuario)
        {
            using (var conn = new SqlConnection(DBContextUtility.CadenaConexion))
            {
                conn.Open();

                var query = @"INSERT INTO Calificacion 
                    (NotaMatematicas, NotaSociales, NotaCiencias, NotaArtes, NotaLenguas, IdUsuario, IdCurso)
                      VALUES (@Matematicas, @Sociales, @Ciencias, @Artes, @Lenguas, @IdUsuario, @IdCurso)";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Matematicas", modelo.NotaMatematicas);
                    cmd.Parameters.AddWithValue("@Sociales", modelo.NotaSociales);
                    cmd.Parameters.AddWithValue("@Ciencias", modelo.NotaCiencias);
                    cmd.Parameters.AddWithValue("@Artes", modelo.NotaArtes);
                    cmd.Parameters.AddWithValue("@Lenguas", modelo.NotaLenguas);
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@IdCurso", modelo.IdCurso);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        
        public void ActualizarUsuarioConNotas(UsuarioConCalificacionDto usuario)
        {
            using (var conexion = new SqlConnection(DBContextUtility.CadenaConexion))
            {
                conexion.Open();

                // Actualizar usuario
                var cmdUsuario = new SqlCommand("UPDATE Usuario SET Usuario = @Usuario WHERE IdUsuario = @Id", conexion);
                cmdUsuario.Parameters.AddWithValue("@Usuario", usuario.Usuario.NombreUsuario);
                cmdUsuario.Parameters.AddWithValue("@Id", usuario.Usuario.IdUsuario);
                cmdUsuario.ExecuteNonQuery();

                // Actualizar calificaciones
                var cmdNotas = new SqlCommand(@"
            UPDATE Calificacion SET 
                NotaMatematicas = @m,
                NotaSociales = @s,
                NotaCiencias = @c,
                NotaArtes = @a,
                NotaLenguas = @l
            WHERE IdUsuario = @Id", conexion);

                cmdNotas.Parameters.AddWithValue("@m", usuario.NotaMatematicas);
                cmdNotas.Parameters.AddWithValue("@s", usuario.NotaSociales);
                cmdNotas.Parameters.AddWithValue("@c", usuario.NotaCiencias);
                cmdNotas.Parameters.AddWithValue("@a", usuario.NotaArtes);
                cmdNotas.Parameters.AddWithValue("@l", usuario.NotaLenguas);
                cmdNotas.Parameters.AddWithValue("@Id", usuario.Usuario.IdUsuario);
                cmdNotas.ExecuteNonQuery();
            }
        }

        public void EliminarUsuarioYNotas(int idUsuario)
        {
            using (var conexion = new SqlConnection(DBContextUtility.CadenaConexion))
            {
                conexion.Open();

                var cmdNotas = new SqlCommand("DELETE FROM Calificacion WHERE IdUsuario = @Id", conexion);
                cmdNotas.Parameters.AddWithValue("@Id", idUsuario);
                cmdNotas.ExecuteNonQuery();

                var cmdUsuario = new SqlCommand("DELETE FROM Usuario WHERE IdUsuario = @Id", conexion);
                cmdUsuario.Parameters.AddWithValue("@Id", idUsuario);
                cmdUsuario.ExecuteNonQuery();
            }
        }

        public UsuarioConCalificacionDto ObtenerUsuarioConNotas(int idUsuario)
        {
            using (var conexion = new SqlConnection(DBContextUtility.CadenaConexion))
            {
                conexion.Open();

                var cmd = new SqlCommand(@"
            SELECT u.IdUsuario, u.Nombres, u.Apellidos, u.Usuario, u.Contraseña,
                   c.NotaMatematicas, c.NotaSociales, c.NotaCiencias, c.NotaArtes, c.NotaLenguas
            FROM Usuario u
            INNER JOIN Calificacion c ON u.IdUsuario = c.IdUsuario
            WHERE u.IdUsuario = @IdUsuario", conexion);

                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new UsuarioConCalificacionDto
                        {
                            Usuario = new UsuarioDto
                            {
                                IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                                Nombres = reader["Nombres"].ToString(),
                                Apellidos = reader["Apellidos"].ToString(),
                                NombreUsuario = reader["Usuario"].ToString(),
                                Contraseña = reader["Contraseña"].ToString()
                            },
                            NotaMatematicas = Convert.ToDecimal(reader["NotaMatematicas"]),
                            NotaSociales = Convert.ToDecimal(reader["NotaSociales"]),
                            NotaCiencias = Convert.ToDecimal(reader["NotaCiencias"]),
                            NotaArtes = Convert.ToDecimal(reader["NotaArtes"]),
                            NotaLenguas = Convert.ToDecimal(reader["NotaLenguas"])
                        };
                    }
                }
            }

            return null; 
        }





    }
}


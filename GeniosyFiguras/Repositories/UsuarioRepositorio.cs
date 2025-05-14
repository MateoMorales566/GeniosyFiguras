using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using GeniosyFiguras.Dtos;
using GeniosyFiguras.Utilities;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using System.Data;
using System.Diagnostics;

namespace GeniosyFiguras.Repositories
{
    public class UsuarioRepositorio
    {
        public int CrearUsuario(UsuarioDto usuario)
        {
            int comando = 0;

            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();
            Connection.CONN().FireInfoMessageEventOnUserErrors = true;

            using (SqlCommand command = new SqlCommand("sp_CrearUsuario", Connection.CONN()))
            {
                command.CommandType = CommandType.StoredProcedure;

                // 👉 Agrega este código para capturar mensajes desde SQL Server:
                command.Connection.InfoMessage += (sender, e) =>
                {
                    Debug.WriteLine("SQL Message: " + e.Message);
                    // También puedes usar Console.WriteLine si no estás en modo debug
                    Console.WriteLine("SQL Message: " + e.Message);
                };

                command.Parameters.AddWithValue("@Nombres", usuario.Nombres);
                command.Parameters.AddWithValue("@Apellidos", usuario.Apellidos);
                command.Parameters.AddWithValue("@Email", usuario.Email);
                command.Parameters.AddWithValue("@Telefono", usuario.Telefono);
                command.Parameters.AddWithValue("@Usuario", usuario.NombreUsuario);
                command.Parameters.AddWithValue("@Contraseña", usuario.Contraseña);
                command.Parameters.AddWithValue("@IdRol", usuario.IdRol);

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
            correo.Subject = "¡Bienvenido a Genios y Figuras!";

            string htmlBody = $@"
                <html>
                <body style='font-family: Arial, sans-serif; background-color: #f0f8ff; padding: 20px; color: #333;'>
                    <div style='max-width: 600px; margin: auto; background-color: white; border-radius: 10px; padding: 20px; box-shadow: 0 0 10px rgba(0,0,0,0.1);'>
                        <h2 style='color: #2c3e50;'>¡Hola {nombre}!</h2>
                        <p style='font-size: 16px;'>
                            Te damos la bienvenida a <strong>Genios y Figuras</strong>.<br/>
                            Aquí podrás explorar <em>Calabozos y Dragones</em> a partir de la educación.
                        </p>
                        <img src='cid:continuarImg' alt='Continuar' style='width: 100%; max-width: 100px; margin-top: 20px; border-radius: 8px;'/>
                    </div>
                </body>
                </html>";

            // Crear la vista HTML
            AlternateView vistaHtml = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);

            // Ruta física a la imagen
            string rutaImagen = @"C:\Users\ACER\Documents\Ing Software\GeniosyFiguras\GeniosyFiguras\Contenido\Imagenes\continuar.png";
            LinkedResource imagen = new LinkedResource(rutaImagen, MediaTypeNames.Image.Jpeg);
            imagen.ContentId = "continuarImg";
            imagen.TransferEncoding = TransferEncoding.Base64;

            // Agregar la imagen a la vista
            vistaHtml.LinkedResources.Add(imagen);

            // Agregar la vista al correo
            correo.AlternateViews.Add(vistaHtml);
            correo.IsBodyHtml = true;

            // SMTP
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

                
                var cmd = new SqlCommand("ObtenerUsuariosPorCursoConNotas", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
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

                var query = @"
                    DECLARE @InsertedIds TABLE (IdUsuario INT);
                    INSERT INTO Usuario (Nombres, Apellidos, Email, Telefono, Usuario, Contraseña, IdRol)
                    OUTPUT INSERTED.IdUsuario INTO @InsertedIds
                    VALUES (@Nombres, @Apellidos, @Email, @Telefono, @Usuario, @Contraseña, @IdRol);
                    SELECT IdUsuario FROM @InsertedIds;";

                using (var cmd = new SqlCommand(query, conn))
                {

                    string hash = BCrypt.Net.BCrypt.HashPassword(usuario.Contraseña);

                    cmd.Parameters.AddWithValue("@Nombres", usuario.Nombres);
                    cmd.Parameters.AddWithValue("@Apellidos", usuario.Apellidos);
                    cmd.Parameters.AddWithValue("@Usuario", usuario.NombreUsuario);
                    cmd.Parameters.AddWithValue("@Contraseña", hash);
                    cmd.Parameters.AddWithValue("@IdRol", usuario.IdRol);
                    cmd.Parameters.AddWithValue("@Email", usuario.Email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Telefono", usuario.Telefono ?? (object)DBNull.Value);


                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        idGenerado = Convert.ToInt32(result);
                        Console.WriteLine("✅ ID generado correctamente: " + idGenerado);
                    }
                    else
                    {
                        Console.WriteLine("❌ No se obtuvo ningún ID (resultado NULL).");
                    }
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


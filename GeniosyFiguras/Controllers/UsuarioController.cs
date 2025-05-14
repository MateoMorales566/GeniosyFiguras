using GeniosyFiguras.Dtos;
using GeniosyFiguras.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data.SqlClient;
using GeniosyFiguras.Dtos;
using GeniosyFiguras.Utilities;

namespace GeniosyFiguras.Controllers
{
        public class UsuarioController : Controller
        {
            // GET: User
            public ActionResult Index()
            {
                return View();
            }

        
            public ActionResult CrearUsuario()
            {
                UsuarioDto usuario = new UsuarioDto();
                return View(usuario);
            }
            public ActionResult ListaUsuarios()
            {
                UsuarioDto usuario = new UsuarioDto();
                UsuarioListaDto usuarioLista = new UsuarioListaDto();

                return View(usuario);
            }
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult CrearUsuario( UsuarioDto usuario)
            {
                if (usuario == null)
                {
                    throw new Exception("EL MODELO LLEGA NULL ");
                }

                if (!ModelState.IsValid)
                {
                    throw new Exception("EL MODELO ES INVÁLIDO ");
                }
                // Hashear la contraseña
                usuario.Contraseña = BCrypt.Net.BCrypt.HashPassword(usuario.Contraseña);
                UsuarioServicio usuarioServicio = new UsuarioServicio();
                UsuarioDto responseUsuario = usuarioServicio.CrearUsuario(usuario);

                return View(responseUsuario);
            }

            
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult InicioSesion(UsuarioDto usuario)
            {
                if (usuario == null)
                {
                    throw new Exception("El modelo es nulo");
                }

                if (!ModelState.IsValid)
                {
                    throw new Exception("El modelo es inválido");
                }

                DBContextUtility connection = new DBContextUtility();
                connection.Connect();

                // Solo buscar el usuario, no la contraseña
                string sql = "SELECT Usuario, Contraseña, Nombres, Apellidos, IdRol FROM APP.DBO.Usuario WHERE Usuario = @Usuario";

                using (SqlCommand command = new SqlCommand(sql, connection.CONN()))
                {
                    command.Parameters.AddWithValue("@Usuario", usuario.NombreUsuario);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string hashGuardado = reader["Contraseña"].ToString();

                            // Verificar la contraseña
                            bool passwordCorrecta = BCrypt.Net.BCrypt.Verify(usuario.Contraseña, hashGuardado);

                        if (passwordCorrecta)
                        {
                            // Login exitoso
                            Session["UsuarioNombre"] = reader["Usuario"].ToString();

                            Session["NombreCompleto"] = reader["Nombres"].ToString() + " " + reader["Apellidos"].ToString();
                            Session["IdRol"] = Convert.ToInt32(reader["IdRol"]);

                            connection.Disconnect();
                            return RedirectToAction("IndexProfesor", "Profesor");
                        }

                        else
                        {
                                // Contraseña incorrecta
                                ViewBag.MensajeError = "Contraseña incorrecta.";
                                connection.Disconnect();
                                return View("~/Views/Home/InicioSesion.cshtml"); ;
                            }
                        }
                        else
                        {
                            // Usuario no encontrado
                            ViewBag.MensajeError = "Usuario no encontrado.";
                            connection.Disconnect();
                            return View("~/Views/Home/InicioSesion.cshtml"); ;
                        }
                    }
                }


            }
            public ActionResult CerrarSesion()
            {
                Session.Clear();
                return RedirectToAction("Index", "Home");
            }





        }
}
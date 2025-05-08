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
        public class UserController : Controller
        {
            // GET: User
            public ActionResult Index()
            {
                return View();
            }

        
            public ActionResult CreateUsuario()
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
            public ActionResult CreateUsuario( UsuarioDto usuario)
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
                UsuarioDto responseUsuario = usuarioServicio.CreateUsuario(usuario);

                return View(responseUsuario);
            }

            /*[HttpPost]
            public ActionResult InicioSesion( UsuarioDto usuario)
            {
                DBContextUtility connection = new DBContextUtility();
                connection.Connect();

                string sql = "SELECT Usuario, Contraseña FROM APP.DBO.Usuario WHERE Usuario = @Usuario AND Contraseña = @Contraseña";

                using (SqlCommand command = new SqlCommand(sql, connection.CONN()))
                {
                    command.Parameters.AddWithValue("@Usuario", usuario.NombreUsuario);
                    command.Parameters.AddWithValue("@Contraseña", usuario.Contraseña);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Si encuentra el usuario
                        {
                            Session["UsuarioNombre"] = reader["Usuario"].ToString(); // Guarda el nombre en sesión
                            connection.Disconnect();
                            return RedirectToAction("IndexProfesor", "Profesor");
                        }
                        else
                        {
                            ViewBag.MensajeError = "Usuario no encontrado.";
                            connection.Disconnect();
                            return RedirectToAction("Index","Home");
                        }
                    }
                }
            }*/
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
                string sql = "SELECT Usuario, Contraseña FROM APP.DBO.Usuario WHERE Usuario = @Usuario";

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
                                //Session["UsuarioNombre"] = usuario.NombreUsuario;
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




        }
    }
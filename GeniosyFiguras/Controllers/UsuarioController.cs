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
using System.IO;

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
        public ActionResult CrearUsuario(UsuarioDto usuario)
        {
            if (usuario == null)
            {
                throw new Exception("EL MODELO LLEGA NULL ");
            }

            if (!ModelState.IsValid)
            {
                throw new Exception("EL MODELO ES INVÁLIDO ");
            }

            usuario.Contraseña = BCrypt.Net.BCrypt.HashPassword(usuario.Contraseña);
            UsuarioServicio usuarioServicio = new UsuarioServicio();
            UsuarioDto responseUsuario = usuarioServicio.CrearUsuario(usuario);

            if (responseUsuario.Response == 1)
            {
                TempData["MensajeExito"] = "Usuario creado exitosamente.";
                return RedirectToAction("InicioSesion", "Principal");
            }
            else
            {
                ModelState.AddModelError("", responseUsuario.Message);
                return View(usuario); // Vuelve al registro con mensaje de error
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InicioSesion(UsuarioDto usuario)
        {
            if (usuario == null) throw new Exception("El modelo es nulo");
            if (!ModelState.IsValid) throw new Exception("El modelo es inválido");

            if (string.IsNullOrWhiteSpace(usuario.NombreUsuario) || string.IsNullOrWhiteSpace(usuario.Contraseña))
            {
                ViewBag.MensajeError = "Debe ingresar el usuario y la contraseña.";
                return View("~/Views/Principal/InicioSesion.cshtml");
            }

            const string sql = @"
                SELECT Usuario, Contraseña, Nombres, Apellidos, IdRol 
                FROM APP.DBO.Usuario 
                WHERE Usuario = @Usuario";

            using (var conn = new SqlConnection(DBContextUtility.CadenaConexion))
            {
                conn.Open();

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Usuario", usuario.NombreUsuario);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            ViewBag.MensajeError = "Usuario no encontrado.";
                            return View("~/Views/Principal/InicioSesion.cshtml");
                        }

                        var hashGuardado = reader["Contraseña"].ToString();
                        bool passwordCorrecta = BCrypt.Net.BCrypt.Verify(usuario.Contraseña, hashGuardado);

                        if (!passwordCorrecta)
                        {
                            ViewBag.MensajeError = "Contraseña incorrecta.";
                            return View("~/Views/Principal/InicioSesion.cshtml");
                        }

                        // Login exitoso
                        Session["UsuarioNombre"] = reader["Usuario"].ToString();
                        Session["NombreCompleto"] = reader["Nombres"] + " " + reader["Apellidos"];
                        int idRol = Convert.ToInt32(reader["IdRol"]);
                        Session["IdRol"] = idRol;

                        // Redirección según el rol
                        switch (idRol)
                        {
                            case 1:
                                return RedirectToAction("PrincipalAdministrador", "Administrador");
                            case 2:
                                return RedirectToAction("IndexProfesor", "Profesor");
                            case 3:
                                return RedirectToAction("PrincipalEstudiante", "Estudiante");
                            default:
                                ViewBag.MensajeError = "Rol de usuario no válido.";
                                return View("~/Views/Principal/InicioSesion.cshtml");
                        }
                    }
                }
            }
        }


        public ActionResult CerrarSesion()
            {
                Session.Clear();
                return RedirectToAction("InicioSesion", "Principal");
            }
            public ActionResult EnvioCorreoContrasena()
            {
                return View();
            }
            private string RenderViewToString(ControllerContext context, string viewName, object model)
            {
                var viewResult = ViewEngines.Engines.FindPartialView(context, viewName);

                if (viewResult.View == null)
                    throw new FileNotFoundException("No se encontró la vista", viewName);

                using (var sw = new StringWriter())
                {
                    var viewContext = new ViewContext(context, viewResult.View, new ViewDataDictionary(model), new TempDataDictionary(), sw);
                    viewResult.View.Render(viewContext, sw);
                    return sw.ToString();
                }
            }



            [HttpPost]
            public ActionResult EnvioCorreoContrasena(string email)
            {
                UsuarioServicio usuarioServicio = new UsuarioServicio();
                var usuario = usuarioServicio.ObtenerPorCorreo(email);

                if (usuario != null)
                {
                    string link = Url.Action("CambioDeContrasena", "Usuario", new { correo = usuario.Email }, protocol: Request.Url.Scheme);

                    var model = new CambioContrasenaDto
                    {
                        NombreUsuario = usuario.NombreUsuario,
                        Link = link
                    };

                    string body = RenderViewToString(ControllerContext, "CorreoCambioContrasena", model);

                    CorreoUtil.EnviarCorreo(usuario.Email, "Recuperación de contraseña", body);

                    ViewBag.Mensaje = "Se ha enviado un enlace de recuperación a tu correo.";
                }
                else
                {
                    ViewBag.Mensaje = "El correo no está registrado.";
                }

                return View();
            }
            [HttpGet]
            public ActionResult CambioDeContrasena(string correo)
            {
                return View((object)correo);
            }

        [HttpPost]
            public ActionResult CambiarContrasena(string correo, string NuevaContrasena, string ConfirmarContrasena)
            {
                if (NuevaContrasena != ConfirmarContrasena)
                {
                    ViewBag.Mensaje = "Las contraseñas no coinciden.";
                    return View("CambioDeContrasena", model:correo);
                }

                UsuarioServicio usuarioServicio = new UsuarioServicio();
                var usuario = usuarioServicio.ObtenerPorCorreo(correo);

                if (usuario != null)
                {
                    usuario.Contraseña = BCrypt.Net.BCrypt.HashPassword(NuevaContrasena);

                    Console.WriteLine("HASH NUEVO: " + usuario.Contraseña);

                    usuarioServicio.Actualizar(usuario); // Método que tú ya debes tener

                    ViewBag.Mensaje = "La contraseña se actualizó correctamente.";
                    return RedirectToAction("InicioSesion", "Principal");

            }

            ViewBag.Mensaje = "Usuario no encontrado.";
                return View("CambioDeContrasena", correo);
            }

    







        }
}
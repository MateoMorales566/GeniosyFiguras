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
        /*public ActionResult ListaUsuarios()

        {
            List<string> listaUsuarios = new List<string>();
            listaUsuarios.Add("mateo");
            listaUsuarios.Add("Luis");
            ViewBag.ListaUsuarios = listaUsuarios;
            return View(listaUsuarios);
        }*/
        //[HttpPost]
        /*public ActionResult ListaUsuarios(string selUsuarios)

        {
            ViewBag.Nombre = selUsuarios;
            return View(selUsuarios);
        }*/
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

        [HttpPost]
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
        }



    }
}
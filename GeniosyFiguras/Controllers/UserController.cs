using GeniosyFiguras.Dtos;
using GeniosyFiguras.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult CreateUsuario(UsuarioDto usuario)
        {
            UsuarioServicio usuarioServicio = new UsuarioServicio();
            UsuarioDto responseUsuario = usuarioServicio.CreateUsuario(usuario);

            return View(responseUsuario);
        }


    }
}
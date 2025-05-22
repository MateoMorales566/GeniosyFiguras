using GeniosyFiguras.Utilities;
using GeniosyFiguras.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeniosyFiguras.Controllers
{
    public class AtributosController : Controller
    {
        // GET: Atributos
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AtributosEstudiante(int idCalificacion)
        {
            // 1) Intento leer desde la tabla Atributo_Poder
            var repo = new AtributoPoderRepositorio();

            // 2) Siempre intento regenerar los atributos desde Calificacion
            using (var conexion = new SqlConnection(DBContextUtility.CadenaConexion))
            {
                conexion.Open();
                var cmd = new SqlCommand(
                    "SELECT NotaMatematicas, NotaSociales, NotaCiencias, NotaArtes, NotaLenguas " +
                    "FROM Calificacion WHERE IdCalificacion = @IdCalificacion", conexion);
                cmd.Parameters.AddWithValue("@IdCalificacion", idCalificacion);

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        return HttpNotFound();

                    var atributos = new Atributo_Poder
                    {
                        Fuerza = Convert.ToDecimal(reader["NotaMatematicas"]),
                        Velocidad = Convert.ToDecimal(reader["NotaSociales"]),
                        Durabilidad = Convert.ToDecimal(reader["NotaCiencias"]),
                        Inteligencia = Convert.ToDecimal(reader["NotaArtes"]),
                        Mana = Convert.ToDecimal(reader["NotaLenguas"]),
                        Salud = 5.0m,
                        IdCalificacion = idCalificacion,
                        Tipo = 1
                    };

                    // 3) Ahora siempre insertamos (eliminar + insertar)
                    repo.InsertarAtributoPoder(atributos);

                    // 4) Y devolvemos la vista con ese objeto
                    return View(atributos);
                }
            }
        }







    }
}
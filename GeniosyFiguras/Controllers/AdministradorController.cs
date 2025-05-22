using GeniosyFiguras.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeniosyFiguras.Controllers
{
    public class AdministradorController : Controller
    {
        // GET: Administrador
        public ActionResult PrincipalAdministrador()
        {
            return View();
        }
        private readonly AdministradorServicio _servicio = new AdministradorServicio();

        public ActionResult ReporteProfesores()
        {
            var profesores = _servicio.ObtenerProfesores();
            var ms = new MemoryStream();

            var document = new Document(PageSize.A4, 40f, 40f, 60f, 40f);
            var writer = PdfWriter.GetInstance(document, ms);
            writer.CloseStream = false;

            document.Open();

            // Fuente para el título
            var fontTitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.BLACK);
            var titulo = new Paragraph("Reporte de Profesores", fontTitulo)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 20f
            };
            document.Add(titulo);

            // Tabla con 3 columnas
            var tabla = new PdfPTable(3)
            {
                WidthPercentage = 100f,
                SpacingBefore = 10f
            };
            tabla.SetWidths(new float[] { 1f, 1f, 1f });

            // Colores y fuentes
            var colorFondo = new BaseColor(0x00, 0xaa, 0xbb); // #00aabb
            var fontEncabezado = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.WHITE);
            var fontCelda = FontFactory.GetFont(FontFactory.HELVETICA, 11, BaseColor.BLACK);

            // Encabezados con fondo color
            string[] headers = { "Nombre", "Apellido", "Usuario" };
            foreach (var header in headers)
            {
                var celdaHeader = new PdfPCell(new Phrase(header, fontEncabezado))
                {
                    BackgroundColor = colorFondo,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 8
                };
                tabla.AddCell(celdaHeader);
            }

            // Celdas de contenido
            foreach (var profe in profesores)
            {
                tabla.AddCell(new PdfPCell(new Phrase(profe.Nombres, fontCelda)) { Padding = 6 });
                tabla.AddCell(new PdfPCell(new Phrase(profe.Apellidos, fontCelda)) { Padding = 6 });
                tabla.AddCell(new PdfPCell(new Phrase(profe.NombreUsuario, fontCelda)) { Padding = 6 });
            }

            document.Add(tabla);
            document.Close();

            ms.Position = 0;
            return File(ms, "application/pdf", "ReporteProfesores.pdf");
        }






    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace GeniosyFiguras.Utilities
{
    public static class CorreoUtil
    {
        public static void EnviarCorreo(string destinatario, string asunto, string cuerpoHtml)
        {
            var mensaje = new MailMessage();
            mensaje.To.Add(destinatario);
            mensaje.Subject = asunto;
            mensaje.Body = cuerpoHtml;
            mensaje.IsBodyHtml = true;

            mensaje.From = new MailAddress("geniosyfiguras566@gmail.com");

            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential("geniosyfiguras566@gmail.com", "fazqkgfvlobnfext");
                smtp.EnableSsl = true;
                smtp.Send(mensaje);
            }
        }
    }
}
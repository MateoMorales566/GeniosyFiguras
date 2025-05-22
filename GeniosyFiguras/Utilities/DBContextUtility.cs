using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Web;

namespace GeniosyFiguras.Utilities
{
    public class DBContextUtility
    {
        static string SERVER = "LAPTOP-CORO87NM";
        static string DB_NAME = "APP";
        static string DB_USER = "mateo";
        static string DB_PASSWORD = "12345";

        static string Conn = "server=" + SERVER + ";dataBase=" + DB_NAME+ ";user id=" +DB_USER+ ";password="+ DB_PASSWORD +";MultipleActiveResultSets=true;";
        //Mi conexion
        SqlConnection Con = new SqlConnection(Conn);
        public static string CadenaConexion => Conn;
        //Procedimiento que abre la conexion sqlserver
        public void Connect()
        {
            try
            {
                Con.Open();                    
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        //procedimiento que cierra con la conexion sqlserver
        public void Disconnect()
        {
            Con.Close();
        }
        //funcion que devuelve la conexion sqlserver
        public SqlConnection CONN()
        {
            return Con;
        }

        internal static IDisposable GetConnection()
        {
            throw new NotImplementedException();
        }
    }
}
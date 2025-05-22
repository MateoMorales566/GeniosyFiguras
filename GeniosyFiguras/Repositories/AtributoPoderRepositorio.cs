using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GeniosyFiguras.Repositories.Models;
using GeniosyFiguras.Utilities;
using System.Diagnostics;

namespace GeniosyFiguras.Repositories.Models
{
    public class AtributoPoderRepositorio
    {
      
       
        public void InsertarAtributoPoder(Atributo_Poder atributo)
        {
            // 1) Creamos la conexión localmente y la envolvemos en using
            using (var conexion = new SqlConnection(DBContextUtility.CadenaConexion))
            {
                conexion.Open();  // Si falla aquí, lanzará excepción clara



                string deleteQuery = "DELETE FROM Atributo_Poder WHERE IdCalificacion = @IdCalificacion";
                using (var cmdDelete = new SqlCommand(deleteQuery, conexion))
                {
                    cmdDelete.Parameters.AddWithValue("@IdCalificacion", atributo.IdCalificacion);
                    int filasEliminadas = cmdDelete.ExecuteNonQuery();
                    System.Diagnostics.Debug.WriteLine("Filas eliminadas: " + filasEliminadas);
                }

                // 2) Preparamos el INSERT también en using
                string query = @"
                     INSERT INTO Atributo_Poder 
                    (Fuerza, Velocidad, Durabilidad, Inteligencia, Mana, Salud, IdCalificacion, Tipo)
                    VALUES
                    (@Fuerza, @Velocidad, @Durabilidad, @Inteligencia, @Mana, @Salud, @IdCalificacion, @Tipo)
                    ";

                using (var cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@Fuerza", atributo.Fuerza);
                    cmd.Parameters.AddWithValue("@Velocidad", atributo.Velocidad);
                    cmd.Parameters.AddWithValue("@Durabilidad", atributo.Durabilidad);
                    cmd.Parameters.AddWithValue("@Inteligencia", atributo.Inteligencia);
                    cmd.Parameters.AddWithValue("@Mana", atributo.Mana);
                    cmd.Parameters.AddWithValue("@Salud", atributo.Salud);
                    cmd.Parameters.AddWithValue("@IdCalificacion", atributo.IdCalificacion);
                    cmd.Parameters.AddWithValue("@Tipo", atributo.Tipo);
                    cmd.ExecuteNonQuery();  // Ejecuta y si hay problema explota aquí
                }
                // La conexión se cierra automáticamente al salir del using
            }

        }
        public Atributo_Poder ObtenerPorCalificacion(int idCalificacion)
        {
            using (var conexion = new SqlConnection(DBContextUtility.CadenaConexion))
            {
                conexion.Open();
                string sql = @"
            SELECT Fuerza, Velocidad, Durabilidad, Inteligencia, Mana, Salud, IdCalificacion, Tipo
            FROM Atributo_Poder
            WHERE IdCalificacion = @IdCalificacion";

                using (var cmd = new SqlCommand(sql, conexion))
                {
                    cmd.Parameters.AddWithValue("@IdCalificacion", idCalificacion);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read()) return null;

                        return new Atributo_Poder
                        {
                            Fuerza = Convert.ToDecimal(reader["Fuerza"]),
                            Velocidad = Convert.ToDecimal(reader["Velocidad"]),
                            Durabilidad = Convert.ToDecimal(reader["Durabilidad"]),
                            Inteligencia = Convert.ToDecimal(reader["Inteligencia"]),
                            Mana = Convert.ToDecimal(reader["Mana"]),
                            Salud = Convert.ToDecimal(reader["Salud"]),
                            IdCalificacion = Convert.ToInt32(reader["IdCalificacion"]),
                            Tipo = Convert.ToInt32(reader["Tipo"])
                        };

                    }
                }
            }
        }



    }
}



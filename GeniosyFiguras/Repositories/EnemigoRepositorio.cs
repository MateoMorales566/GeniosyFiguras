using GeniosyFiguras.Dtos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using GeniosyFiguras.Repositories.Models;
using System.Linq;
using System.Web;
using GeniosyFiguras.Utilities;
using System.Data;


namespace GeniosyFiguras.Repositories
{
    public class EnemigoRepositorio
    {
        private readonly string cadenaConexion = DBContextUtility.CadenaConexion;


        public void Crear(EnemigoAtributo model)
        {
            using (SqlConnection connection = new SqlConnection(cadenaConexion))
            {
                connection.Open();

                // Insertar en Atributo_Poder
                SqlCommand cmd = new SqlCommand(@"
            INSERT INTO Atributo_Poder 
            (Fuerza, Velocidad, Durabilidad, Inteligencia, Mana, Salud, Tipo, IdCalificacion)
            VALUES 
            (@Fuerza, @Velocidad, @Durabilidad, @Inteligencia, @Mana, @Salud, 2, NULL);
            SELECT SCOPE_IDENTITY();", connection);

                cmd.Parameters.AddWithValue("@Fuerza", model.Atributos.Fuerza);
                cmd.Parameters.AddWithValue("@Velocidad", model.Atributos.Velocidad);
                cmd.Parameters.AddWithValue("@Durabilidad", model.Atributos.Durabilidad);
                cmd.Parameters.AddWithValue("@Inteligencia", model.Atributos.Inteligencia);
                cmd.Parameters.AddWithValue("@Mana", model.Atributos.Mana);
                cmd.Parameters.AddWithValue("@Salud", model.Atributos.Salud);

                int idAtributoPoder = Convert.ToInt32(cmd.ExecuteScalar());

                // Validar campos de Enemigo
                if (string.IsNullOrWhiteSpace(model.Enemigo.Nombre))
                {
                    throw new Exception("El campo Nombre está vacío o nulo.");
                }

                // Insertar en Enemigo
                SqlCommand cmd2 = new SqlCommand(@"
            INSERT INTO Enemigo 
            (Nombre, Descripcion, Imagen, IdAtributoPoder)
            VALUES 
            (@Nombre, @Descripcion, @Imagen, @IdAtributoPoder);", connection);

                cmd2.Parameters.Add("@Nombre", SqlDbType.VarChar, 80).Value = model.Enemigo.Nombre;
                cmd2.Parameters.Add("@Descripcion", SqlDbType.VarChar, 200).Value = model.Enemigo.Descripcion;
                cmd2.Parameters.Add("@Imagen", SqlDbType.NVarChar, 300).Value = (object)model.Enemigo.Imagen ?? DBNull.Value;
                cmd2.Parameters.AddWithValue("@IdAtributoPoder", idAtributoPoder);

                cmd2.ExecuteNonQuery();
            }
        }

        public List<EnemigoAtributo> ObtenerTodos()
        {
            List<EnemigoAtributo> lista = new List<EnemigoAtributo>();

            using (SqlConnection connection = new SqlConnection(cadenaConexion))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("sp_ObtenerEnemigos", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        EnemigoDto enemigo = new EnemigoDto
                        {
                            IdEnemigo = Convert.ToInt32(reader["IdEnemigo"]),
                            Nombre = reader["Nombre"].ToString(),
                            Descripcion = reader["Descripcion"].ToString(),
                            Imagen = reader["Imagen"] != DBNull.Value ? reader["Imagen"].ToString() : null
                        };

                        Atributo_Poder atributos = new Atributo_Poder
                        {
                            Fuerza = Convert.ToInt32(reader["Fuerza"]),
                            Velocidad = Convert.ToInt32(reader["Velocidad"]),
                            Durabilidad = Convert.ToInt32(reader["Durabilidad"]),
                            Inteligencia = Convert.ToInt32(reader["Inteligencia"]),
                            Mana = Convert.ToInt32(reader["Mana"]),
                            Salud = Convert.ToInt32(reader["Salud"])
                        };

                        lista.Add(new EnemigoAtributo
                        {
                            Enemigo = enemigo,
                            Atributos = atributos
                        });
                    }
                }
            }

            return lista;
        }


    }

}
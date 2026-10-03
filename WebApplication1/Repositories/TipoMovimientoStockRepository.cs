using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class TipoMovimientoStockRepository
    {
        private string ObtenerConnectionString()
        {
            return ConfigurationManager
                .ConnectionStrings["FerreteriaConnection"]
                .ConnectionString;
        }

        public List<TipoMovimientoStock> ObtenerActivos()
        {
            List<TipoMovimientoStock> tipos =
                new List<TipoMovimientoStock>();

            string connectionString =
                ObtenerConnectionString();

            string sql = @"SELECT
                               Id,
                               Codigo,
                               Descripcion,
                               Activo
                           FROM TipoMovimientoStock
                           WHERE Activo = 1
                           ORDER BY Descripcion";

            using (SqlConnection conexion =
                new SqlConnection(connectionString))
            {
                SqlCommand comando =
                    new SqlCommand(sql, conexion);

                conexion.Open();

                using (SqlDataReader reader =
                    comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TipoMovimientoStock tipo =
                            new TipoMovimientoStock();

                        tipo.Id = (int)reader["Id"];
                        tipo.Codigo =
                            reader["Codigo"].ToString();
                        tipo.Descripcion =
                            reader["Descripcion"].ToString();
                        tipo.Activo =
                            (bool)reader["Activo"];

                        tipos.Add(tipo);
                    }
                }
            }

            return tipos;
        }
        public TipoMovimientoStock ObtenerPorId(int id)
        {
            string connectionString =
                ObtenerConnectionString();

            string sql = @"SELECT
                       Id,
                       Codigo,
                       Descripcion,
                       Activo
                   FROM TipoMovimientoStock
                   WHERE Id = @Id";

            using (SqlConnection conexion =
                new SqlConnection(connectionString))
            {
                SqlCommand comando =
                    new SqlCommand(sql, conexion);

                comando.Parameters.Add(
                    "@Id",
                    System.Data.SqlDbType.Int
                ).Value = id;

                conexion.Open();

                using (SqlDataReader reader =
                    comando.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        TipoMovimientoStock tipo =
                            new TipoMovimientoStock();

                        tipo.Id = (int)reader["Id"];
                        tipo.Codigo = reader["Codigo"].ToString();
                        tipo.Descripcion = reader["Descripcion"].ToString();
                        tipo.Activo = (bool)reader["Activo"];

                        return tipo;
                    }
                }
            }

            return null;
        }
    }
}
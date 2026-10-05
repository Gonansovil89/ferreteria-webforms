using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class MotivoMovimientoStockRepository
    {
        private string ObtenerConnectionString()
        {
            return ConfigurationManager
                .ConnectionStrings["FerreteriaConnection"]
                .ConnectionString;
        }

        public List<MotivoMovimientoStock> ObtenerActivos()
        {
            List<MotivoMovimientoStock> motivos =
                new List<MotivoMovimientoStock>();

            string connectionString =
                ObtenerConnectionString();

            string sql = @"SELECT
                               Id,
                               Codigo,
                               Descripcion,
                               Activo
                           FROM MotivoMovimientoStock
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
                        MotivoMovimientoStock motivo =
                            new MotivoMovimientoStock();

                        motivo.Id = (int)reader["Id"];
                        motivo.Codigo =
                            reader["Codigo"].ToString();
                        motivo.Descripcion =
                            reader["Descripcion"].ToString();
                        motivo.Activo =
                            (bool)reader["Activo"];

                        motivos.Add(motivo);
                    }
                }
            }

            return motivos;
        }
        public MotivoMovimientoStock ObtenerPorId(int id)
        {
            string connectionString =
                ObtenerConnectionString();

            string sql = @"SELECT
                                Id,
                                Codigo,
                                Descripcion,
                                Activo
                            FROM MotivoMovimientoStock
                            WHERE Id = @Id";

            using (SqlConnection conexion =
                new SqlConnection(connectionString))
            {
                SqlCommand comando =
                    new SqlCommand(sql, conexion);

                comando.Parameters.AddWithValue(
                    "@Id",
                    id);

                conexion.Open();

                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        MotivoMovimientoStock motivo =
                            new MotivoMovimientoStock();

                        motivo.Id = (int)reader["Id"];

                        motivo.Codigo = reader["Codigo"].ToString();

                        motivo.Descripcion = reader["Descripcion"].ToString();

                        motivo.Activo = (bool)reader["Activo"];

                        return motivo;
                    }
                }
            }

            return null;
        }
    }
}
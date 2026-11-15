using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class ProveedorRepository
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["FerreteriaConnection"].ConnectionString;

        public List<Proveedor> ObtenerActivos()
        {
            List<Proveedor> proveedores = new List<Proveedor>();

            string sql = @"SELECT Id, RazonSocial, CUIT, Activo
                           FROM Proveedor
                           WHERE Activo = 1
                           ORDER BY RazonSocial";

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                using (SqlCommand comando = new SqlCommand(sql, conexion))
                {
                    conexion.Open();
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Acá vamos a procesar cada proveedor
                            Proveedor proveedor = new Proveedor();
                            proveedor.Id = (int)reader["Id"];
                            proveedor.RazonSocial = (string)reader["RazonSocial"];
                            proveedor.Activo = (bool)reader["Activo"];
                            proveedor.CUIT = reader["CUIT"] == System.DBNull.Value ? null: (string)reader["CUIT"];
                            proveedores.Add(proveedor);
                        }
                    }
                }
            }

            return proveedores;
        }
    }
}
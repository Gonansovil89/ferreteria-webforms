using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class ProductoRepository
    {
        private string ObtenerConnectionString()
        {
            return ConfigurationManager
                .ConnectionStrings["FerreteriaConnection"]
                .ConnectionString;
        }
        public List<Producto> ObtenerProductos(bool activo)
        {
            string connectionString = ObtenerConnectionString();

            List<Producto> productos = new List<Producto>();

            string sql = @"SELECT Id, Codigo, Nombre, Precio, Stock, Activo
                   FROM Producto
                   WHERE Activo = @Activo
                   ORDER BY Id";

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                SqlCommand comando = new SqlCommand(sql, conexion);

                comando.Parameters.Add(
                    "@Activo",
                    System.Data.SqlDbType.Bit
                ).Value = activo;

                conexion.Open();

                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Producto producto = new Producto();

                        producto.Id = (int)reader["Id"];
                        producto.Codigo = reader["Codigo"].ToString();
                        producto.Nombre = reader["Nombre"].ToString();
                        producto.Precio = (decimal)reader["Precio"];
                        producto.Stock = (int)reader["Stock"];
                        producto.Activo = (bool)reader["Activo"];

                        productos.Add(producto);
                    }
                }
            }

            return productos;
        }

        public bool ExisteCodigo(string codigo)
        {
            string connectionString = ObtenerConnectionString();

            string sql = @"SELECT COUNT(*)
                   FROM Producto
                   WHERE Codigo = @Codigo";

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                SqlCommand comando = new SqlCommand(sql, conexion);

                comando.Parameters.Add(
                    "@Codigo",
                    System.Data.SqlDbType.VarChar,
                    50
                ).Value = codigo;

                conexion.Open();

                int cantidad = (int)comando.ExecuteScalar();

                return cantidad > 0;
            }
        }
        public bool ExisteCodigoInactivo(string codigo)
        {
            string connectionString = ObtenerConnectionString();

            string sql = @"SELECT COUNT(*)
                   FROM Producto
                   WHERE Codigo = @Codigo
                     AND Activo = 0";

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                SqlCommand comando = new SqlCommand(sql, conexion);

                comando.Parameters.Add(
                    "@Codigo",
                    System.Data.SqlDbType.VarChar, 50).Value = codigo;

                conexion.Open();

                int cantidad = (int)comando.ExecuteScalar();

                return cantidad > 0;
            }
        }
        public void InsertarProducto(Producto producto)
        {
            string connectionString = ObtenerConnectionString();

            string sql = @"INSERT INTO Producto
                   (Codigo, Nombre, Precio, Stock)
                   VALUES
                   (@Codigo, @Nombre, @Precio, @Stock)";

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                SqlCommand comando = new SqlCommand(sql, conexion);

                comando.Parameters.Add("@Codigo", System.Data.SqlDbType.VarChar, 50)
                                  .Value = producto.Codigo;
                comando.Parameters.Add("@Nombre", System.Data.SqlDbType.VarChar, 150)
                                  .Value = producto.Nombre;
                SqlParameter parametroPrecio = comando.Parameters.Add("@Precio", System.Data.SqlDbType.Decimal);
                parametroPrecio.Precision = 18;
                parametroPrecio.Scale = 2;
                parametroPrecio.Value = producto.Precio;
                comando.Parameters.Add("@Stock", System.Data.SqlDbType.Int)
                                  .Value = producto.Stock;

                conexion.Open();

                comando.ExecuteNonQuery();
            }
        }
        public void ActualizarProducto(int id, string nombre, decimal precio)
        {
            string connectionString = ObtenerConnectionString();

            string sql = @"UPDATE Producto
                   SET Nombre = @Nombre,
                       Precio = @Precio
                   WHERE Id = @Id";

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                SqlCommand comando = new SqlCommand(sql, conexion);

                comando.Parameters.Add("@Id", System.Data.SqlDbType.Int)
                                  .Value = id;

                comando.Parameters.Add("@Nombre", System.Data.SqlDbType.VarChar, 150)
                                  .Value = nombre;

                SqlParameter parametroPrecio =
                    comando.Parameters.Add("@Precio", System.Data.SqlDbType.Decimal);

                parametroPrecio.Precision = 18;
                parametroPrecio.Scale = 2;
                parametroPrecio.Value = precio;

                conexion.Open();

                comando.ExecuteNonQuery();
            }
        }
        public void DesactivarProducto(int id)
        {
            string connectionString = ObtenerConnectionString();

            string sql = @"UPDATE Producto
                   SET Activo = 0
                   WHERE Id = @Id";

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                SqlCommand comando = new SqlCommand(sql, conexion);

                comando.Parameters.Add(
                    "@Id",
                    System.Data.SqlDbType.Int
                ).Value = id;

                conexion.Open();

                comando.ExecuteNonQuery();
            }
        }
        public void ReactivarProducto(int id)
        {
            string connectionString = ObtenerConnectionString();

            string sql = @"UPDATE Producto
                   SET Activo = 1
                   WHERE Id = @Id";

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                SqlCommand comando = new SqlCommand(sql, conexion);

                comando.Parameters.Add(
                    "@Id",
                    System.Data.SqlDbType.Int
                ).Value = id;

                conexion.Open();

                comando.ExecuteNonQuery();
            }
        }
        public Producto ObtenerPorId(int id)
        {
            string connectionString = ObtenerConnectionString();

            string sql = @"SELECT
                       Id,
                       Codigo,
                       Nombre,
                       Precio,
                       Stock,
                       Activo
                   FROM Producto
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
                        Producto producto = new Producto();

                        producto.Id = (int)reader["Id"];
                        producto.Codigo =
                            reader["Codigo"].ToString();
                        producto.Nombre =
                            reader["Nombre"].ToString();
                        producto.Precio =
                            (decimal)reader["Precio"];
                        producto.Stock =
                            (int)reader["Stock"];
                        producto.Activo =
                            (bool)reader["Activo"];

                        return producto;
                    }
                }
            }

            return null;
        }
    }
}
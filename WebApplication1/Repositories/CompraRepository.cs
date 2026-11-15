using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using WebApplication1.Models;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication1.Repositories
{
    public class CompraRepository
    {
        //conexión configurada en Web.config con el nombre FerreteriaConnection
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["FerreteriaConnection"].ConnectionString;

        public void InsertarCompra(Compra compra)
        {
            if (compra == null || compra.Detalles == null || compra.Detalles.Count != 1)
            {
                throw new ArgumentException("La compra debe contener exactamente un producto.");
            }

            CompraDetalle detalle = compra.Detalles[0];

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();

                using (SqlCommand comando = new SqlCommand("InsertarCompra",conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;

                    comando.Parameters.Add("@ProveedorId", SqlDbType.Int).Value = compra.ProveedorId;
                    comando.Parameters.Add("@ProductoId", SqlDbType.Int).Value = detalle.ProductoId;
                    comando.Parameters.Add("@Cantidad", SqlDbType.Int).Value = detalle.Cantidad;

                    SqlParameter parametroPrecio = comando.Parameters.Add("@PrecioUnitario", SqlDbType.Decimal);
                    parametroPrecio.Precision = 18;
                    parametroPrecio.Scale = 2;
                    parametroPrecio.Value = detalle.PrecioUnitario;

                    comando.Parameters.Add("@Observacion", SqlDbType.VarChar, 250).Value = (object)compra.Observacion ?? DBNull.Value;
                    comando.ExecuteNonQuery();
                }
            }
        }

        public List<CompraHistorial> ObtenerHistorial()
        {
            List<CompraHistorial> compras = new List<CompraHistorial>();

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                using (SqlCommand comando = new SqlCommand("Compra_ObtenerHistorial", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;

                    conexion.Open();
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CompraHistorial compra = new CompraHistorial();

                            compra.CompraId = (int)reader["CompraId"];
                            compra.Fecha = (DateTime)reader["Fecha"];
                            compra.RazonSocial = (string)reader["RazonSocial"];
                            compra.CodigoProducto = (string)reader["CodigoProducto"];
                            compra.Producto = (string)reader["Producto"];
                            compra.Cantidad = (int)reader["Cantidad"];
                            compra.PrecioUnitario = (decimal)reader["PrecioUnitario"];
                            compra.SubTotal = (decimal)reader["SubTotal"];
                            compra.Total = (decimal)reader["Total"];

                            compras.Add(compra);
                        }
                    }
                }
            }
            return compras;
        }
    }
}
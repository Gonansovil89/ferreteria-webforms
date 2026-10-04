using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class MovimientoStockRepository
    {
        private string ObtenerConnectionString()
        {
            return ConfigurationManager
                .ConnectionStrings["FerreteriaConnection"]
                .ConnectionString;
        }

        public void RegistrarMovimiento(MovimientoStock movimiento)
        {
            string connectionString = ObtenerConnectionString();

            using (SqlConnection conexion =
                new SqlConnection(connectionString))
            {
                conexion.Open();

                SqlTransaction transaccion =
                    conexion.BeginTransaction();

                try
                {
                    ActualizarStockProducto(
                        movimiento.ProductoId,
                        movimiento.StockAnterior,
                        movimiento.StockPosterior,
                        conexion,
                        transaccion);

                    InsertarMovimiento(
                        movimiento,
                        conexion,
                        transaccion);

                    transaccion.Commit();
                }
                catch
                {
                    transaccion.Rollback();
                    throw;
                }
            }
        }

        private void InsertarMovimiento(
            MovimientoStock movimiento,
            SqlConnection conexion,
            SqlTransaction transaccion)
        {
            string sql = @"
                INSERT INTO MovimientoStock
                (
                    ProductoId,
                    TipoMovimientoStockId,
                    MotivoMovimientoStockId,
                    Cantidad,
                    StockAnterior,
                    StockPosterior,
                    Observacion
                )
                VALUES
                (
                    @ProductoId,
                    @TipoMovimientoStockId,
                    @MotivoMovimientoStockId,
                    @Cantidad,
                    @StockAnterior,
                    @StockPosterior,
                    @Observacion
                )";

            SqlCommand comando =
                new SqlCommand(
                    sql,
                    conexion,
                    transaccion);

            comando.Parameters.Add(
                "@ProductoId",
                SqlDbType.Int
            ).Value = movimiento.ProductoId;

            comando.Parameters.Add(
                "@TipoMovimientoStockId",
                SqlDbType.Int
            ).Value = movimiento.TipoMovimientoStockId;

            comando.Parameters.Add(
                "@MotivoMovimientoStockId",
                SqlDbType.Int
            ).Value = movimiento.MotivoMovimientoStockId;

            comando.Parameters.Add(
                "@Cantidad",
                SqlDbType.Int
            ).Value = movimiento.Cantidad;

            comando.Parameters.Add(
                "@StockAnterior",
                SqlDbType.Int
            ).Value = movimiento.StockAnterior;

            comando.Parameters.Add(
                "@StockPosterior",
                SqlDbType.Int
            ).Value = movimiento.StockPosterior;

            comando.Parameters.Add(
                "@Observacion",
                SqlDbType.VarChar,
                250
            ).Value =
                string.IsNullOrWhiteSpace(movimiento.Observacion)
                    ? (object)DBNull.Value
                    : movimiento.Observacion;

            comando.ExecuteNonQuery();
        }

        private void ActualizarStockProducto(
            int productoId,
            int stockAnterior,
            int stockPosterior,
            SqlConnection conexion,
            SqlTransaction transaccion)
        {
            string sql = @"
                UPDATE Producto
                SET Stock = @Stock
                WHERE Id = @ProductoId
                  AND Stock = @StockAnterior
                  AND Activo = 1";

            SqlCommand comando =
                new SqlCommand(
                    sql,
                    conexion,
                    transaccion);

            comando.Parameters.Add(
                "@Stock",
                SqlDbType.Int
            ).Value = stockPosterior;

            comando.Parameters.Add(
                "@ProductoId",
                SqlDbType.Int
            ).Value = productoId;

            comando.Parameters.Add(
                "@StockAnterior",
                SqlDbType.Int
            ).Value = stockAnterior;

            int filasAfectadas =
                comando.ExecuteNonQuery();

            if (filasAfectadas != 1)
            {
                throw new InvalidOperationException(
                    "No se registró el movimiento: el stock cambió o el producto ya no está disponible. Vuelva a intentar con los datos actualizados.");
            }
        }
    }
}

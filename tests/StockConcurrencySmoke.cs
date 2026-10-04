using System;
using System.Configuration;
using System.Data.SqlClient;
using WebApplication1.Models;
using WebApplication1.Repositories;
using WebApplication1.Services;

// Ejecutar contra Ferreteria con una copia de la configuración de conexión.
// Usa un producto exclusivo de prueba y lo elimina en finally.
class StockConcurrencySmoke
{
    static object Sql(string sql)
    {
        using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["FerreteriaConnection"].ConnectionString))
        using (var command = new SqlCommand(sql, connection))
        {
            connection.Open();
            return command.ExecuteScalar();
        }
    }

    static void Assert(bool condition, string message)
    {
        if (!condition) throw new Exception(message);
    }

    static void Main()
    {
        int id = 0;
        try
        {
            int tipo = Convert.ToInt32(Sql("SELECT Id FROM TipoMovimientoStock WHERE Codigo='SALIDA' AND Activo=1"));
            int motivo = Convert.ToInt32(Sql("SELECT TOP (1) Id FROM MotivoMovimientoStock WHERE Activo=1 ORDER BY Id"));
            Assert(tipo > 0 && motivo > 0, "Faltan catálogos activos.");
            id = Convert.ToInt32(Sql("INSERT Producto (Codigo,Nombre,Precio,Stock) OUTPUT INSERTED.Id VALUES ('TEST-' + CONVERT(varchar(36),NEWID()),'Prueba concurrencia',1,100)"));
            var repository = new MovimientoStockRepository();
            Func<int, int, MovimientoStock> movement = (before, after) => new MovimientoStock {
                ProductoId=id, TipoMovimientoStockId=tipo, MotivoMovimientoStockId=motivo,
                Cantidad=before-after, StockAnterior=before, StockPosterior=after
            };
            Action<int, int> verify = (stock, count) => {
                Assert(Convert.ToInt32(Sql("SELECT Stock FROM Producto WHERE Id=" + id)) == stock, "Saldo inesperado.");
                Assert(Convert.ToInt32(Sql("SELECT COUNT(*) FROM MovimientoStock WHERE ProductoId=" + id)) == count, "Historial inesperado.");
            };

            // Dos cálculos basados en la misma lectura; se persisten en orden.
            repository.RegistrarMovimiento(movement(100,90));
            bool conflict = false;
            try { repository.RegistrarMovimiento(movement(100,80)); }
            catch (InvalidOperationException) { conflict = true; }
            Assert(conflict, "No se detectó el saldo desactualizado.");
            verify(90,1);
            repository.RegistrarMovimiento(movement(90,70));
            verify(70,2);
            Console.WriteLine("OK: movimiento normal, conflicto y reintento.");

            var invalid = movement(70,60);
            invalid.MotivoMovimientoStockId = -1;
            bool failedInsert = false;
            try { repository.RegistrarMovimiento(invalid); }
            catch (SqlException) { failedInsert = true; }
            Assert(failedInsert, "No se rechazó el motivo inexistente.");
            verify(70,2);
            Console.WriteLine("OK: rollback del UPDATE cuando falla el INSERT.");

            bool insufficient = false;
            try { new MovimientoStockService().RegistrarMovimiento(id,tipo,motivo,71,null); }
            catch (Exception ex) { insufficient = ex.Message.Contains("stock suficiente"); }
            Assert(insufficient, "No se rechazó la salida excesiva.");
            verify(70,2);
            Sql("UPDATE Producto SET Activo=0 WHERE Id=" + id);
            bool inactive = false;
            try { repository.RegistrarMovimiento(movement(70,60)); }
            catch (InvalidOperationException) { inactive = true; }
            Assert(inactive, "No se rechazó el producto inactivo.");
            verify(70,2);
            Console.WriteLine("OK: stock insuficiente y producto inactivo.");
        }
        finally
        {
            if (id > 0) Sql("DELETE MovimientoStock WHERE ProductoId=" + id + "; DELETE Producto WHERE Id=" + id);
        }
        Console.WriteLine("Pruebas finalizadas; producto e historial de prueba eliminados.");
    }
}

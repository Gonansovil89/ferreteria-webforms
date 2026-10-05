CREATE PROCEDURE MovimientoStock_ObtenerHistorial
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ms.Id,
        ms.Fecha,
        p.Codigo AS ProductoCodigo,
        p.Nombre AS ProductoNombre,
        t.Descripcion AS TipoMovimiento,
        m.Descripcion AS MotivoMovimiento,
        ms.Cantidad,
        ms.StockAnterior,
        ms.StockPosterior,
        ms.Observacion
    FROM MovimientoStock ms
    INNER JOIN Producto p
        ON p.Id = ms.ProductoId
    INNER JOIN TipoMovimientoStock t
        ON t.Id = ms.TipoMovimientoStockId
    INNER JOIN MotivoMovimientoStock m
        ON m.Id = ms.MotivoMovimientoStockId
    ORDER BY ms.Fecha DESC;
END

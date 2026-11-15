CREATE PROCEDURE Compra_ObtenerHistorial
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
		c.Id as CompraId,
		c.Fecha as Fecha,
		p.RazonSocial as RazonSocial,
		pro.Codigo as CodigoProducto,
		pro.Nombre as Producto,
		cd.Cantidad as Cantidad,
		cd.PrecioUnitario as PrecioUnitario,
		cd.Cantidad * cd.PrecioUnitario as SubTotal,
		c.Total as Total
	FROM Compra c
		join CompraDetalle cd on cd.CompraId = c.Id
		join Proveedor p on p.Id = c.ProveedorId
		join Producto pro on pro.Id = cd.ProductoId
	ORDER BY c.Fecha desc, c.Id desc
END;
ALTER PROC InsertarCompra
(
@ProveedorId int,
@ProductoId int,
@Cantidad int,
@PrecioUnitario decimal(18,2),
@Observacion varchar(250)
)

AS

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN

DECLARE @CompraId INT;

IF @Cantidad IS NULL OR @Cantidad <= 0
    BEGIN
        ;THROW 50001, 'La cantidad debe ser mayor a cero.', 1;
    END;

IF @PrecioUnitario IS NULL OR @PrecioUnitario <= 0
    BEGIN
        ;THROW 50002, 'El precio unitario debe ser mayor a cero.', 1;
    END;
IF NOT EXISTS (
    SELECT 1
    FROM Proveedor
    WHERE Id = @ProveedorId
      AND Activo = 1
)
	BEGIN
		;THROW 50006, 'El proveedor no existe o esta inactivo.', 1;
	END;
IF NOT EXISTS (
    SELECT 1
    FROM Producto
    WHERE Id = @ProductoId AND Activo = 1
)
	BEGIN
		;THROW 50007, 'El producto no existe o esta inactivo.', 1;
	END;

	BEGIN TRY

		BEGIN TRANSACTION;

			INSERT INTO Compra
			(
				ProveedorId,
				Fecha,
				Total,
				Observacion
			)
			VALUES
			(
				@ProveedorId,
				GETDATE(),
				@Cantidad * @PrecioUnitario,
				@Observacion
			);

			SET @CompraId = CAST(SCOPE_IDENTITY() AS INT);

			INSERT INTO CompraDetalle
			(
				CompraId,
				ProductoId,
				Cantidad,
				PrecioUnitario
			)
			VALUES
			(
				@CompraId,
				@ProductoId,
				@Cantidad,
				@PrecioUnitario
			);

			DECLARE @StockAnterior INT;

			SELECT @StockAnterior = Stock
			FROM Producto
			WHERE Id = @ProductoId;

			DECLARE @StockPosterior INT;

			SET @StockPosterior = @StockAnterior + @Cantidad;

			UPDATE Producto
			SET Stock = @StockPosterior
			WHERE Id = @ProductoId
			  AND Stock = @StockAnterior
			  AND Activo = 1;

			IF @@ROWCOUNT <> 1
			BEGIN
				;THROW 50003, 'No se pudo actualizar el stock del producto.', 1;
			END;

			DECLARE @TipoMovimientoStockId INT;
			DECLARE @MotivoMovimientoStockId INT;

			SELECT @TipoMovimientoStockId = Id
			FROM TipoMovimientoStock
			WHERE Codigo = 'ENTRADA'
			  AND Activo = 1;

			SELECT @MotivoMovimientoStockId = Id
			FROM MotivoMovimientoStock
			WHERE Codigo = 'COMPRA'
			  AND Activo = 1;

			IF @TipoMovimientoStockId IS NULL
			BEGIN
				;THROW 50004, 'No existe un tipo de movimiento ENTRADA activo.', 1;
			END;

			IF @MotivoMovimientoStockId IS NULL
			BEGIN
				;THROW 50005, 'No existe un motivo de movimiento COMPRA activo.', 1;
			END;

			INSERT INTO MovimientoStock
			(ProductoId,
			TipoMovimientoStockId,
			MotivoMovimientoStockId,
			Cantidad,
			StockAnterior,
			StockPosterior,
			Observacion)
			values
			(
			@ProductoId,
			@TipoMovimientoStockId,
			@MotivoMovimientoStockId,
			@Cantidad,
			@StockAnterior,
			@StockPosterior,
			'INGRESO POR COMPRA'
			)
		
			COMMIT TRANSACTION;
	END TRY

	BEGIN CATCH

		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
		THROW;

	END CATCH;

END;
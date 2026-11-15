CREATE PROCEDURE Producto_Insertar
    @Codigo VARCHAR(50),
    @Nombre VARCHAR(150),
    @Precio DECIMAL(18, 2),
    @Stock INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY

        BEGIN TRANSACTION;

        INSERT INTO Producto
        (
            Codigo,
            Nombre,
            Precio,
            Stock
        )
        VALUES
        (
            @Codigo,
            @Nombre,
            @Precio,
            @Stock
        );

        DECLARE @ProductoId INT;

        SET @ProductoId =
            CAST(SCOPE_IDENTITY() AS INT);

        IF @Stock > 0
        BEGIN
            DECLARE @TipoMovimientoId INT;
            DECLARE @MotivoMovimientoId INT;

            SELECT @TipoMovimientoId = Id
            FROM TipoMovimientoStock
            WHERE Codigo = 'ENTRADA'
              AND Activo = 1;

            SELECT @MotivoMovimientoId = Id
            FROM MotivoMovimientoStock
            WHERE Codigo = 'STOCK_INICIAL'
              AND Activo = 1;

            IF @TipoMovimientoId IS NULL
            BEGIN
                RAISERROR(
                    'No existe un tipo de movimiento ENTRADA activo.',
                    16,
                    1
                );
            END;

            IF @MotivoMovimientoId IS NULL
            BEGIN
                RAISERROR(
                    'No existe un motivo STOCK_INICIAL activo.',
                    16,
                    1
                );
            END;

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
                @TipoMovimientoId,
                @MotivoMovimientoId,
                @Stock,
                0,
                @Stock,
                'Stock inicial del producto'
            );
        END;

        COMMIT TRANSACTION;

        SELECT @ProductoId AS ProductoId;

    END TRY
    BEGIN CATCH

        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;

    END CATCH;
END;
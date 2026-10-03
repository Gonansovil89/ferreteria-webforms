# Development State

Última actualización: 2026-10-03

## Estado

CHECKPOINT FUNCIONAL

El proyecto compila con 0 errores y el flujo básico de movimientos
de stock fue probado contra SQL Server.

---

# Infraestructura

## Aplicación

ASP.NET Web Forms sobre .NET Framework.

IDE:

    Visual Studio 2022

## Base de datos

SQL Server LocalDB:

    (localdb)\MSSQLLocalDB

Base:

    Ferreteria

Connection string:

    FerreteriaConnection

## Control de versiones

Git inicializado.

Git Flow configurado.

Ramas principales:

    master
    develop

Rama actual al crear este checkpoint:

    develop

Todavía pendiente subir el repositorio remoto a GitHub.

---

# Productos

Implementado:

- alta
- consulta
- edición
- activación
- desactivación lógica
- validación de código único
- precio mayor a cero
- stock mayor o igual a cero
- persistencia SQL Server

`Producto.Stock` representa el saldo actual.

El stock dejó de ser editable desde el GridView de productos.

El stock inicial todavía puede indicarse durante el alta.

---

# Persistencia de productos

Se utiliza:

    ProductoRepository

Responsabilidades actuales:

- obtener productos
- obtener producto por Id
- comprobar existencia de código
- comprobar códigos de productos inactivos
- insertar
- actualizar
- desactivar
- reactivar

El acceso a datos utiliza ADO.NET.

---

# Inventario

Tablas implementadas:

    TipoMovimientoStock
    MotivoMovimientoStock
    MovimientoStock

## TipoMovimientoStock

Valores iniciales:

    ENTRADA
    SALIDA

## MotivoMovimientoStock

Valores iniciales:

    STOCK_INICIAL
    COMPRA
    VENTA
    AJUSTE

---

# Pantalla MovimientosStock

Implementado:

- selección de producto
- selección de tipo
- selección de motivo
- cantidad
- observación
- validaciones básicas
- carga de DropDownLists desde SQL Server

Los productos se muestran mediante:

    Producto.DescripcionCompleta

con formato conceptual:

    CODIGO - Nombre

---

# MovimientoStockService

Implementado:

- obtención del producto
- validación de producto existente
- validación de producto activo
- obtención del tipo de movimiento
- validación del tipo
- validación de cantidad
- interpretación de ENTRADA / SALIDA
- cálculo de StockAnterior
- cálculo de StockPosterior
- prevención de stock negativo
- construcción del modelo MovimientoStock

---

# MovimientoStockRepository

Implementado:

    RegistrarMovimiento(MovimientoStock movimiento)

Utiliza una `SqlTransaction`.

Dentro de la transacción se ejecutan:

    INSERT MovimientoStock
    UPDATE Producto.Stock

Ante éxito:

    COMMIT

Ante error:

    ROLLBACK

---

# Última prueba funcional confirmada

Fecha:

    2026-10-03

Producto:

    ProductoId = 1003

Operación:

    TipoMovimientoStockId = 2
    MotivoMovimientoStockId = 4
    Cantidad = 10
    Observacion = "prueba de resta"

Resultado:

    StockAnterior = 100
    StockPosterior = 90

Registro generado:

    MovimientoStock.Id = 3

Fecha registrada por SQL Server:

    2026-10-03 19:40:24.460

Se verificó:

1. mensaje correcto desde la aplicación
2. Producto.Stock actualizado a 90
3. registro correcto en MovimientoStock
4. StockAnterior = 100
5. StockPosterior = 90

Resultado:

    PRUEBA OK

---

# Riesgo técnico conocido

Existe una ventana de concurrencia.

Actualmente `MovimientoStockService` obtiene el producto y su stock
antes de que `MovimientoStockRepository` inicie la transacción.

Flujo actual:

    SELECT Producto.Stock
             |
             v
    cálculo StockPosterior
             |
             v
    BEGIN TRANSACTION
    INSERT MovimientoStock
    UPDATE Producto.Stock
    COMMIT

Dos operaciones concurrentes podrían leer el mismo stock inicial y
producir una actualización perdida.

---

# Próximo objetivo

Estudiar y resolver concurrencia en movimientos de stock.

Conceptos a trabajar:

- transacciones
- concurrencia
- lost update
- locking
- consistencia
- alternativas de concurrencia optimista/pesimista

La solución debe preservar:

- reglas de negocio en una ubicación coherente
- acceso a datos encapsulado
- trazabilidad de movimientos
- atomicidad
- stock no negativo

No implementar la solución sin analizar primero las alternativas.

---

# Pendientes posteriores

Una vez resuelta la concurrencia, evaluar incrementalmente:

- Post/Redirect/Get en MovimientosStock para evitar duplicados por F5
- historial visual de movimientos
- stock inicial registrado como MovimientoStock
- compras
- ventas
- clientes
- reportes
- stock bajo

Estos puntos todavía no deben considerarse implementados.
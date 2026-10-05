# Development State

Última actualización: 2026-10-04

## Estado

CONCURRENCIA OPTIMISTA IMPLEMENTADA Y VALIDADA

# Validación final de concurrencia

El 2026-10-04 se completaron las validaciones pendientes.

## Prueba simultánea con dos sesiones SQL

Producto de prueba:

    ProductoId = 6002
    Codigo = TEST-CONC-REAL
    Stock inicial = 100

Sesión A intentó actualizar de 100 a 90 y afectó 1 fila.

Sesión B intentó actualizar de 100 a 80 mientras A mantenía la
transacción abierta. Luego del COMMIT de A, B continuó y afectó 0 filas
porque el stock ya era 90.

Resultado final:

    Stock = 90

La actualización perdida fue evitada correctamente.

## Prueba desde UI

Desde `MovimientosStock.aspx` se registró:

    ProductoId = 6002
    TipoMovimientoStockId = 2
    MotivoMovimientoStockId = 4
    Cantidad = 10
    Observacion = "prueba UI concurrencia"

Resultado:

    StockAnterior = 90
    StockPosterior = 80
    MovimientoStock.Id = 2002

Se verificó el saldo final del producto y el historial generado.

Resultado:

    VALIDACIÓN OK

# Próximo objetivo

La validación funcional y de concurrencia de esta feature está completa.
El siguiente paso es cerrar el Pull Request e integrar la feature en `develop`.

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

Rama actual:

    feature/stock-concurrency

El checkpoint funcional previo se creó en `develop`.
El remoto `origin` está configurado como:

    https://github.com/Gonansovil89/ferreteria-webforms.git

No se verificó en esta revisión si los commits están publicados en GitHub.
El árbol de trabajo estaba limpio antes de esta actualización documental.

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

    UPDATE Producto.Stock condicionado a StockAnterior y Activo = 1
    INSERT MovimientoStock

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

# Control de concurrencia implementado

La lectura del Service sigue fuera de la transacción, pero el Repository
ahora rechaza saldos desactualizados antes de insertar el historial.

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
    UPDATE Producto.Stock WHERE Stock = @StockAnterior AND Activo = 1
    comprobar que afectó una fila
    INSERT MovimientoStock
    COMMIT

Dos operaciones pueden leer el mismo saldo; si la primera lo cambia,
la segunda se rechaza y hace rollback. También se rechaza un producto
que dejó de estar activo. Se conserva el límite de comparar saldos:
no se detectan cambios intermedios que regresan al mismo valor.

Pruebas de integración del 2026-10-04 (`tests/StockConcurrencySmoke.cs`):

- salida 100 → 90 registrada;
- segunda salida basada en 100 rechazada; saldo 90 e historial de una fila;
- reintento desde 90 → 70 registrado;
- motivo inexistente provoca fallo del INSERT y rollback del saldo;
- salida de 71 desde 70 rechazada por el Service;
- producto inactivo rechazado al persistir.

El producto temporal y sus movimientos se eliminaron al finalizar.
La prueba reproduce una lectura desactualizada con persistencias secuenciales;
no sustituye la prueba simultánea. La primera versión del arnés usó una
transacción externa que interfería con el rollback; se corrigió antes
de obtener los resultados anteriores.

---

# Próximo objetivo

Verificar el flujo en UI y ejecutar dos sesiones simultáneas para completar
la validación de concurrencia optimista antes de cerrar la feature.

Conceptos a trabajar:

- transacciones
- concurrencia
- lost update
- locking
- consistencia
- actualización condicional y detección de conflictos

La solución debe preservar:

- reglas de negocio en una ubicación coherente
- acceso a datos encapsulado
- trazabilidad de movimientos
- atomicidad
- stock no negativo

El enfoque implementado es actualizar el producto solamente si su stock
todavía coincide con `StockAnterior`, verificando las filas afectadas.
Si otro movimiento cambió el saldo, se debe rechazar la operación y hacer
ROLLBACK de toda la transacción, sin dejar un movimiento registrado.
El usuario debe recibir un mensaje que permita volver a intentar con
el saldo actualizado; no se prevén reintentos automáticos inicialmente.

Antes de modificar código, repasar el flujo y el resultado esperado.
Después, verificar movimientos normales, saldo insuficiente, conflicto
entre dos operaciones que leyeron el mismo stock y rollback sin efectos
parciales. La compilación, UI y persistencia deben volver a comprobarse.

La decisión y sus límites se describen en `docs/ARCHITECTURE.md`.

---

# Limitaciones adicionales observadas

- El Service no valida que el motivo exista y esté activo.
- No hay scripts SQL versionados para recrear la base y verificar sus
  restricciones a partir del repositorio.
- Se comprobó el rechazo SQL de un motivo inexistente, pero no se auditó
  el conjunto completo de restricciones de SQL Server.

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

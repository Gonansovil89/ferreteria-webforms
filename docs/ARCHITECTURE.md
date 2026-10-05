# Arquitectura

## Objetivo

Este documento registra las principales decisiones arquitectónicas
del proyecto Ferretería.

No pretende definir una arquitectura definitiva.

La aplicación evolucionará incrementalmente a medida que aparezcan
necesidades reales.

---

# Arquitectura general

Actualmente:

    Browser
       |
       | HTTP GET / POST
       v
    ASP.NET Web Forms
       |
       | eventos / binding
       v
    Code Behind
       |
       v
    Services
       |
       v
    Repositories
       |
       | ADO.NET
       v
    SQL Server

---

# ASP.NET Web Forms

Las páginas `.aspx` contienen la interfaz.

Los archivos `.aspx.cs` contienen el Code Behind.

Los controles con:

    runat="server"

son procesados por ASP.NET en el servidor.

El navegador finalmente recibe HTML.

Los PostBack representan nuevas solicitudes HTTP.

Se utiliza `IsPostBack` para diferenciar la carga inicial de las
solicitudes posteriores generadas por controles Web Forms.

---

# Models

Los modelos actuales incluyen:

    Producto
    TipoMovimientoStock
    MotivoMovimientoStock
    MovimientoStock

Los modelos representan datos utilizados por la aplicación.

Actualmente no se utilizan navigation properties ni ORM.

---

# Repositories

La aplicación utiliza ADO.NET directamente.

Los repositories encapsulan el acceso a SQL Server.

Operaciones utilizadas:

    ExecuteReader()
    ExecuteScalar()
    ExecuteNonQuery()

Las consultas utilizan parámetros SQL.

Ejemplo conceptual:

    UI
     |
     v
    ProductoRepository
     |
     v
    SqlConnection
     |
     v
    SqlCommand
     |
     v
    SQL Server

---

# Services

Los Services representan operaciones y reglas de negocio.

Ejemplo:

    MovimientoStockService

El Service determina actualmente:

- si el producto existe
- si está activo
- si el tipo de movimiento existe
- si está activo
- si la cantidad es válida
- si una operación es ENTRADA o SALIDA
- el stock posterior
- si una salida produciría stock negativo

---

# Diseño de stock

Se decidió mantener dos conceptos separados.

## Producto.Stock

Representa:

    saldo actual

Permite consultar rápidamente cuánto stock existe actualmente.

## MovimientoStock

Representa:

    historial / ledger

Permite explicar cómo se llegó al saldo actual.

Ejemplo:

    Stock inicial             100
    SALIDA                     10
                              ---
    Stock actual               90

El movimiento almacena:

    StockAnterior = 100
    Cantidad      = 10
    StockPosterior = 90

---

# Modificación de stock

El stock inicial puede establecerse durante el alta de un producto.

Después del alta, el stock no debe editarse directamente desde el CRUD.

Los cambios deben realizarse mediante operaciones de inventario.

Esto permite conservar trazabilidad.

---

# Catálogos de stock

Se utilizan:

    TipoMovimientoStock
    MotivoMovimientoStock

`TipoMovimientoStock` describe el efecto general.

Actualmente:

    ENTRADA
    SALIDA

`MotivoMovimientoStock` describe la causa.

Actualmente:

    STOCK_INICIAL
    COMPRA
    VENTA
    AJUSTE

Los IDs son claves técnicas.

La lógica funcional debe utilizar códigos estables cuando necesita
interpretar el significado de un catálogo.

Por ejemplo:

    ENTRADA
    SALIDA

No asumir:

    Id 1 = ENTRADA
    Id 2 = SALIDA

porque los IDs pertenecen a la persistencia y podrían variar.

---

# Transacciones de stock

Registrar un movimiento requiere dos cambios relacionados:

    INSERT MovimientoStock
    UPDATE Producto.Stock

Actualmente ambos se realizan dentro de una misma `SqlTransaction`.

Flujo:

    BEGIN TRANSACTION

        UPDATE Producto con saldo esperado y producto activo
        INSERT MovimientoStock

    COMMIT

Ante una excepción:

    ROLLBACK

Esto garantiza atomicidad:

    se realizan ambos cambios

o:

    no se realiza ninguno

---

# Problema de concurrencia de la implementación anterior

La implementación anterior tenía una ventana de concurrencia.

Flujo anterior:

    Service
       |
       | SELECT Producto.Stock
       v
    calcula StockPosterior
       |
       v
    Repository
       |
       | BEGIN TRANSACTION
       | INSERT MovimientoStock
       | UPDATE Producto
       | COMMIT

La lectura inicial del stock ocurre antes de la transacción.

Ejemplo:

    Stock = 100

    Usuario A lee 100
    Usuario B lee 100

    A calcula 90
    B calcula 80

    A escribe 90
    B escribe 80

El resultado correcto, si A retiró 10 y B retiró 20, debería ser:

    70

Este problema corresponde a una actualización perdida
(lost update).

El control implementado verifica que el stock utilizado para calcular
un movimiento siga siendo válido al persistirlo.

El 2026-10-04 se eligió e implementó concurrencia optimista.

---

# Decisión: concurrencia optimista para stock

La solución conserva los cálculos y validaciones de negocio en
el Service y el SQL en el Repository. Al persistir, el Repository comprueba
que el saldo utilizado por el Service sigue vigente mediante un UPDATE
condicional con parámetros:

    UPDATE Producto
    SET Stock = @StockPosterior
    WHERE Id = @ProductoId AND Stock = @StockAnterior AND Activo = 1

El UPDATE debe afectar exactamente una fila. Cero filas significa que
el saldo esperado ya no coincide o que el producto no está disponible; la operación
no debe confirmarse. El UPDATE y el INSERT del historial deben permanecer
dentro de la misma transacción, con ROLLBACK ante un conflicto o error.

Flujo implementado:

    Service lee stock y calcula movimiento
    BEGIN TRANSACTION
        UPDATE condicional y comprobación de filas afectadas
        INSERT MovimientoStock
    COMMIT

Ante un conflicto, se lanza `InvalidOperationException` y la UI muestra
el mensaje para volver a intentar con datos actualizados. No hay
reintentos automáticos.

Ejemplo: A y B leen 100. A retira 10 y confirma 90. El UPDATE de B, que
esperaba 100 para retirar 20, afecta cero filas y su operación se revierte.
Al volver a intentar desde 90, B puede registrar una salida y dejar 70.

Ventajas: cambio localizado, sin nuevas tecnologías ni cambios iniciales
de esquema, y sin mantener un bloqueo desde la lectura del Service.
El UPDATE sí adquiere los bloqueos normales de SQL Server durante la
transacción; concurrencia optimista no significa ausencia de bloqueos.

Desventajas y límites: el usuario puede necesitar repetir una operación.
Comparar el saldo detecta diferencias de stock, pero no detecta cambios
intermedios que devuelvan el saldo al mismo valor ni cambios de otros
atributos. Si se necesita detectar cualquier modificación de la fila,
se deberá evaluar un token `rowversion`. El estado activo se comprueba
en el UPDATE para rechazar productos desactivados después de la lectura.

Se verificaron contra LocalDB movimientos normales, conflicto por saldo
desactualizado, reintento, rollback por fallo del INSERT, salida excesiva
y producto inactivo. Además, se completó una prueba real con dos sesiones
SQL: A actualizó 100 → 90 y B, que esperaba 100, afectó cero filas después
del COMMIT de A. El saldo final fue 90 y se evitó la actualización perdida.
Desde UI se registró después una salida 90 → 80 y se verificaron el saldo
y el historial en SQL. La feature quedó validada funcionalmente; los
detalles de estas pruebas se registran en `docs/DEV-STATE.md`.

---

# Principios actuales

1. Mantener responsabilidades claras.
2. Evitar SQL en Code Behind.
3. Mantener reglas de negocio fuera de la UI cuando sea posible.
4. Utilizar restricciones de base de datos como última línea de defensa.
5. Mantener trazabilidad de operaciones de stock.
6. Utilizar transacciones para operaciones compuestas.
7. No introducir complejidad sin una necesidad real.
8. Refactorizar cuando el problema justifique el cambio.

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

        INSERT MovimientoStock
        UPDATE Producto

    COMMIT

Ante una excepción:

    ROLLBACK

Esto garantiza atomicidad:

    se realizan ambos cambios

o:

    no se realiza ninguno

---

# Problema de concurrencia conocido

La implementación actual todavía tiene una ventana de concurrencia.

Actualmente:

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

La próxima evolución del diseño debe garantizar que el stock utilizado
para calcular un movimiento siga siendo válido al persistirlo.

Se analizarán mecanismos de concurrencia y transacciones antes de elegir
la implementación definitiva.

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
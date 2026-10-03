# Ferretería - ASP.NET Web Forms

Proyecto de aprendizaje y desarrollo de un sistema de gestión para una
ferretería utilizando ASP.NET Web Forms, .NET Framework, C# y SQL Server.

El objetivo del proyecto no es solamente construir una aplicación funcional,
sino utilizarla para profundizar conocimientos de desarrollo web,
arquitectura de aplicaciones, C#, ADO.NET, SQL Server, Git y buenas prácticas
de desarrollo asistido por IA.

## Tecnologías

- ASP.NET Web Forms
- .NET Framework
- C#
- SQL Server LocalDB
- ADO.NET
- HTML / CSS
- Visual Studio 2022
- Git
- Git Flow

## Base de datos

Instancia local:

    (localdb)\MSSQLLocalDB

Base:

    Ferreteria

La aplicación utiliza una cadena de conexión llamada:

    FerreteriaConnection

definida en `Web.config`.

## Arquitectura actual

La aplicación está organizada principalmente en las siguientes capas:

    Web Forms / Code Behind
              |
              v
           Services
              |
              v
         Repositories
              |
              v
          SQL Server

### Models

Representan los datos utilizados por la aplicación.

Ejemplos:

- Producto
- TipoMovimientoStock
- MotivoMovimientoStock
- MovimientoStock

### Repositories

Contienen el acceso a SQL Server mediante ADO.NET.

Son responsables de:

- conexiones
- comandos SQL
- parámetros
- SqlDataReader
- ExecuteScalar
- ExecuteNonQuery
- transacciones

### Services

Contienen reglas y operaciones de negocio que no corresponden directamente
a la interfaz de usuario ni al acceso a datos.

## Funcionalidades implementadas

### Productos

- Alta de productos.
- Consulta de productos.
- Edición de nombre y precio.
- Código único.
- Activación y desactivación lógica.
- Validaciones de datos.
- Persistencia en SQL Server.
- Stock no editable directamente después del alta.

### Stock

Catálogos:

- TipoMovimientoStock.
- MotivoMovimientoStock.

Registro histórico:

- MovimientoStock.

Actualmente se pueden registrar entradas y salidas de stock.

Cada movimiento registra:

- producto
- tipo
- motivo
- cantidad
- stock anterior
- stock posterior
- observación
- fecha

La actualización de `Producto.Stock` y el INSERT de `MovimientoStock`
se realizan dentro de una transacción SQL.

## Principio de stock

`Producto.Stock` representa el saldo actual.

`MovimientoStock` representa el historial que explica cómo se llegó
a ese saldo.

Una vez creado un producto, su stock no debe modificarse directamente
desde el CRUD de productos.

Los cambios posteriores deben realizarse mediante movimientos de stock.

## Estado del proyecto

Consultar:

    docs/DEV-STATE.md

para conocer el último checkpoint funcional y el próximo objetivo.

Consultar:

    docs/ARCHITECTURE.md

para las decisiones arquitectónicas.

## Forma de trabajo

El proyecto se desarrolla incrementalmente.

Cada funcionalidad importante sigue aproximadamente este flujo:

1. Comprender el problema.
2. Diseñar la solución.
3. Implementar de forma incremental.
4. Depurar.
5. Validar en aplicación y base de datos.
6. Revisar cambios con Git.
7. Crear un checkpoint mediante commit.
8. Actualizar la documentación cuando corresponda.

El proyecto también se utiliza para aprender buenas prácticas de
desarrollo asistido por IA. Las reglas para asistentes están documentadas
en `AGENTS.md`.
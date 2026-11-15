# AGENTS.md

## Propósito

Este repositorio es simultáneamente:

1. Una aplicación de gestión para una ferretería.
2. Un proyecto de aprendizaje de desarrollo de software.
3. Un entorno para aprender buenas prácticas de desarrollo asistido por IA.

Los asistentes de IA que trabajen sobre este repositorio deben preservar
estos tres objetivos.

---

## Stack tecnológico

El proyecto utiliza:

- ASP.NET Web Forms
- .NET Framework
- C#
- SQL Server
- ADO.NET
- Visual Studio 2022

No introducir frameworks o tecnologías adicionales sin una razón concreta
y una decisión explícita.

En particular, no reemplazar ADO.NET por Entity Framework automáticamente.

ADO.NET se está utilizando deliberadamente como parte del aprendizaje.

---

## Arquitectura actual

El flujo principal es:

    Web Forms / Code Behind
              |
              v
           Service
              |
              v
          Repository
              |
              v
          SQL Server

### Code Behind

Responsabilidades principales:

- recibir eventos de la UI
- obtener valores de controles
- validaciones básicas de entrada
- binding de controles
- mostrar resultados al usuario

Evitar colocar SQL directamente en Code Behind.

### Services

Responsabilidades principales:

- reglas de negocio
- cálculos
- validaciones de dominio
- coordinación de operaciones

### Repositories

Responsabilidades principales:

- acceso a datos
- ADO.NET
- SqlConnection
- SqlCommand
- SqlDataReader
- transacciones
- ejecución de SQL

### Models

Representan las entidades y datos utilizados por la aplicación.

---

## Reglas de desarrollo

### Cambios mínimos

No realizar refactors generales cuando el requerimiento puede resolverse
con un cambio localizado.

No modificar comportamiento existente que no esté relacionado con
la tarea actual.

No introducir abstracciones solamente para reducir líneas de código.

Las abstracciones deben responder a una necesidad concreta.

### Base de datos

Utilizar parámetros SQL.

No concatenar entrada del usuario dentro de consultas SQL.

Mantener las restricciones de integridad también en la base cuando
corresponda:

- PRIMARY KEY
- FOREIGN KEY
- UNIQUE
- CHECK
- DEFAULT

Las reglas de negocio que involucren múltiples operaciones relacionadas
deben analizarse desde el punto de vista transaccional.

### Stock

`Producto.Stock` es el saldo actual.

`MovimientoStock` es el historial de movimientos.

Después del alta del producto, el stock no debe modificarse directamente
desde el CRUD de productos.

Todo cambio posterior de stock debe quedar trazado mediante
`MovimientoStock`.

Una operación de stock no debe dejar actualizado solamente el producto
o solamente el historial.

Ambos cambios deben conservar consistencia.

---

## Forma de colaboración con IA

El usuario está aprendiendo y debe conservar participación activa en
la implementación.

No generar automáticamente grandes cantidades de código cuando el cambio
contiene un concepto que conviene aprender.

Para conceptos nuevos:

1. explicar el problema
2. mostrar el flujo
3. pedir o permitir que el usuario razone el resultado esperado
4. implementar incrementalmente
5. depurar o inspeccionar valores
6. verificar el resultado

El asistente puede generar directamente código repetitivo o mecánico
cuando el concepto ya fue comprendido.

Antes de cambios arquitectónicos importantes, explicar:

- problema actual
- alternativa propuesta
- ventajas
- desventajas
- impacto

No introducir patrones de diseño únicamente por sofisticación.

---

## Git

El repositorio utiliza Git Flow.

Ramas principales:

    master
    develop

Prefijos configurados:

    feature/
    bugfix/
    release/
    hotfix/
    support/

Las nuevas funcionalidades deben desarrollarse en una feature cuando
sea razonable.

Antes de un commit importante:

1. comprobar compilación
2. realizar las pruebas correspondientes
3. revisar `git status`
4. revisar el diff
5. actualizar `DEV-STATE.md` si cambia el checkpoint del proyecto

Los mensajes de commit deben describir el cambio realizado.

---

## Documentación

`README.md`

Describe el proyecto y permite comprenderlo desde cero.

`docs/ARCHITECTURE.md`

Registra arquitectura y decisiones técnicas relevantes.

`docs/DEV-STATE.md`

Representa el checkpoint actual del desarrollo.

Debe indicar claramente:

- qué funciona
- qué fue probado
- qué limitaciones se conocen
- cuál es el próximo objetivo

Cuando una sesión de desarrollo termine en un estado funcional relevante,
actualizar `DEV-STATE.md`.

---

## Criterio de finalización

No considerar una funcionalidad terminada únicamente porque compila.

Según corresponda, verificar:

- compilación
- comportamiento en UI
- cambios en SQL Server
- validaciones
- efectos secundarios
- persistencia
- transacciones
- regresiones básicas

Si algo no fue probado, indicarlo explícitamente.
# Documento RUP técnico

## 1. Propósito técnico

Este documento define una guía técnica para el desarrollo del proyecto Fenix Engine bajo el enfoque de RUP, tomando como base la solución actual y su arquitectura modular ya observada en los proyectos del repositorio.

El objetivo es convertir la solución actual en una plataforma desarrollada con disciplina de ingeniería, trazabilidad y evolución incremental.

---

## 2. Diagnóstico técnico del proyecto actual

La solución actual muestra una organización por capas y módulos:

- aplicación principal MAUI en FenixEngine;
- integración con servicios de IA en FenixEngine.Agents;
- orquestación del motor en FenixEngine.AppCore;
- servicios comunes en FenixEngine.Shared;
- persistencia con SQLite en FenixEngine.DataBase;
- servicios funcionales en FenixEngine.Services.

Esto representa una base sólida para una arquitectura limpia y extensible. El proyecto no se parece a una aplicación simple, sino a un conjunto de bloques de infraestructura que trabajan juntos para automatizar procesos complejos.

---

## 3. Principios arquitectónicos recomendados

### 3.1 Separación de responsabilidades
Cada proyecto debe responder a una responsabilidad clara:

- UI: experiencia del usuario y entrada de datos.
- Orquestación: motor de decisiones y ejecución.
- IA: acceso a proveedores, prompts y modelos.
- Servicios compartidos: archivos, terminal, logging, utilidades.
- Persistencia: manejo de datos locales o de negocio.
- Dominio y workflow: lógica de proceso y decisión del sistema.

### 3.2 Inversión de dependencias
Los componentes deben depender de abstracciones y no de implementaciones concretas. Esto favorece pruebas, sustitución de proveedores y evolución del sistema.

### 3.3 Modularidad
Cada módulo debe encapsular una capacidad. Por ejemplo:

- generación de prompts;
- gestión de modelos IA;
- acceso a archivos;
- ejecución de procesos del sistema;
- almacenamiento local.

### 3.4 Trazabilidad
Cada requisito debe relacionarse con una pieza funcional del sistema, una decisión técnica y una salida verificable.

---

## 4. Fases RUP aplicadas a la solución actual

### Fase de concepción
Objetivo: validar la viabilidad del sistema y determinar qué problema resuelve.

Se responde a estas preguntas:

- ¿Qué tipo de automatización necesita el usuario?
- ¿Qué tareas deben digitalizarse y orquestarse?
- ¿Qué proveedores de IA serán compatibles?
- ¿Cuál es el alcance mínimo viable?

### Fase de elaboración
Objetivo: definir la base arquitectónica del sistema.

Debe establecerse:

- el flujo de trabajo principal;
- los servicios necesarios por capa;
- la relación entre IA, contexto y ejecución;
- la estrategia de persistencia;
- la forma de validar y documentar.

### Fase de construcción
Objetivo: producir incrementos de software funcional.

Se deben priorizar entregables como:

- flujo base de ejecución;
- registro de contexto;
- integración con IA;
- servicios transversales;
- base de datos local operativa;
- validación por caso de uso.

### Fase de transición
Objetivo: estabilizar la solución para uso real.

Incluye:

- validación de escenarios reales;
- revisión de errores críticos;
- documentación para operación;
- preparación para mantenimiento.

---

## 5. Modelado técnico de dominio

El dominio del proyecto se puede modelar a partir de estas ideas:

- Caso de uso o solicitud del usuario.
- Contexto de ejecución con variables y bitácora.
- Pipeline o flujo de trabajo.
- Piezas software ejecutables.
- Servicios de infraestructura.
- Proveedores de IA.
- Salida del proceso, ya sea archivo, proyecto, documentación o resultado estructurado.

Esta estructura encaja con la lógica que ya se observa en el archivo de contexto de ejecución, donde se guarda información por clave y se registra la bitácora del proceso.

---

## 6. Arquitectura recomendada por capas

### Capa de interfaz
Responsable de la interacción del usuario.

Ejemplos:
- app MAUI;
- pantallas;
- controles y navegación;
- entrada de requisitos y comandos.

### Capa de orquestación
Responsable del flujo y de la decisión de ejecución.

Incluye:
- contexto de ejecución;
- pipeline de pasos;
- decisiones por piezas;
- flujo de tareas automatizadas.

### Capa de servicios
Responsable de la implementación reutilizable del sistema.

Incluye:
- servicios de archivos;
- logger;
- terminal;
- API HTTP;
- prompts;
- IA y modelos.

### Capa de persistencia
Responsable de la información estructurada.

Incluye:
- SQLite local;
- entidades y acceso a datos;
- configuración de base de datos.

---

## 7. Requisitos técnicos funcionales

El sistema debería cubrir al menos estos requisitos:

- registrar el caso de uso que se desea atender;
- crear contexto de ejecución con parámetros y estado;
- ejecutar pasos secuenciales o condicionales;
- manejar errores sin detener todo el flujo;
- documentar los eventos de cada ejecución;
- invocar modelos de IA según el tipo de tarea;
- producir artefactos técnicos o de negocio.

---

## 8. Requisitos técnicos no funcionales

- modularidad y baja dependencia entre servicios;
- seguridad en acceso a terminales y archivos;
- capacidad de extensión con nuevos proveedores IA;
- estabilidad ante fallos de red o de proveedores externos;
- facilidad de pruebas por cada componente;
- mantenibilidad y documentación clara;
- compatibilidad con la plataforma MAUI y sus dependencias.

---

## 9. Estrategia de pruebas

### Pruebas unitarias
Se deben cubrir:

- lógica del contexto;
- validación de parámetros;
- utilidades;
- lógica de servicios aislados.

### Pruebas de integración
Se deben validar:

- invocación a proveedores IA;
- flujo de servicio con persistencia;
- interacción entre módulos de orquestación y servicios.

### Pruebas por casos de uso
Se recomienda probar de forma end-to-end algunos escenarios:

- ejecución de flujo simple;
- flujo con error parcial;
- generación de documento;
- creación de proyecto desde especificación;
- acceso a terminal y archivos.

---

## 10. Gestión de configuración

Para evitar caos en el proyecto, se recomienda:

- trabajar por módulos y ramas temáticas;
- documentar cada decisión arquitectónica;
- revisar cambios antes de fusionarlos;
- mantener un backlog con requisitos y prioridades;
- evidenciar qué cambios impactan a qué módulos.

---

## 11. Observaciones sobre la solución existente

La solución actual presenta puntos positivos importantes:

- separación de proyectos por responsabilidad;
- uso de inyección de dependencias;
- servicios reutilizables para archivos, terminal y logging;
- acceso a múltiples proveedores de IA;
- enfoque de flujo y contexto de ejecución;
- estructura apta para orquestación.

Sin embargo, también hay oportunidades de mejora:

- unificar nombres y namespaces para mejorar claridad;
- revisar la lógica de Gemini para evitar duplicación con Anthropic;
- definir contratos de servicios más estrictos;
- formalizar reglas y validaciones por módulo;
- identificar qué piezas de software corresponden realmente a casos de uso del negocio.

---

## 12. Recomendación de implementación incremental

La plataforma debería crecer en estas etapas:

### Etapa 1: base estable
- arranque del sistema;
- servicios compartidos;
- contexto y pipeline;
- logging y control de errores.

### Etapa 2: proveedores IA
- integración con OpenAI, Ollama, Anthropic y Gemini;
- normalización de llamadas;
- manejo de errores y fallbacks.

### Etapa 3: persistencia y estado
- almacenamiento local de contexto y procesos;
- historial de ejecuciones;
- metadatos del sistema.

### Etapa 4: automatización real
- casos de uso complejos de generación;
- creación de proyectos y documentación;
- validación del flujo end-to-end.

---

## 13. Conclusión técnica

A nivel técnico, el proyecto ya tiene una base apropiada para ser construido bajo una metodología RUP. La organización por módulos y la separación de servicios permiten crecer sin perder claridad. El siguiente paso no es simplemente escribir más código, sino consolidar la arquitectura, formalizar los flujos y asegurar que cada desarrollo tenga propósito, trazabilidad y validación.

Cuando el equipo resuelva la parte de consistencia arquitectónica y defina con mayor rigor los casos de uso del negocio, el proyecto podrá evolucionar hacia una plataforma más madura, robusta y fácil de mantener.

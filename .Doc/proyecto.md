## Fenix Engine 
Entorno de Desarrollo para automatizar desarrollo apartir de especificaciones de diseño (SDD). Tambien debe ser capas de recibir feedback

### Objetivo General del proyecto
Agilizar el desarrollo apartir del diseño de un ser humano con conocimientos de arquitectura de software y tecnicas de programación.


### Objetivo Especifico
* Diseñar y desarrollar nuevos features, fixes y refactors para sistemas legados asi tambien nuevos desarrollos apartir de un test unit.
* Crear y actualizar la documentacion final de cada proyecto y desarrollo que se va agregando.
* Controlar el flujo de trabajo apartir de las especificaciones del arquitecto y del analista programador bajo el dominio de las apis y comandos de exposicion del MCP.
* Tener la capacidad de crear proyectos de software de tipo moviles, webs, Apis, consolas y librerias en primer grado con el ecosistema de .NET 


## Estructura basica de arbol de decisiones 

📁 MiProyectoMaui/
├── 📁 Enums/
│   └── PiezaId.cs
├── 📁 Models/
│   └── ContextoEjecucion.cs
├── 📁 Interfaces/
│   └── IPiezaSoftware.cs
├── 📁 Services/
│   ├── DecisionTreeService.cs
│   └── PipelineOrchestrator.cs
├── 📁 Handlers/
│   ├── Pieza1_ValidarEntornos.cs
│   ├── Pieza2_CrearEstructura.cs
│   └── Pieza7_GenerarDocumentacion.cs
├── 📁 ViewModels/
│   └── MainViewModel.cs
└── MauiProgram.cs

## Flujo Resumido
	1.	El usuario presiona un botón en la UI que invoca MainViewModel.EjecutarOpcionUsuarioAsync("CrearProyecto").
	2.	El PipelineOrchestrator consulta a DecisionTreeService, que responde con la lista [ValidarEntornos, CrearEstructura, GenerarDocumentacion].
	3.	El PipelineOrchestrator toma la Pieza1_ValidarEntornos, la ejecuta pasando el ContextoEjecucion, luego pasa a la Pieza2_CrearEstructura, e inserta datos requeridos por la Pieza7_GenerarDocumentacion.
	4.	El ViewModel recibe el contexto final con la bitácora completa para mostrar los resultados en la interfaz nativa.

# FenixEngine

## 1. Descripción

FenixEngine es una aplicación multiplataforma construida con .NET MAUI para asistir el desarrollo de software a partir de especificaciones de diseño, prompts y flujos de trabajo. Su objetivo es permitir que un arquitecto o desarrollador interactúe con agentes de IA locales y servicios de IA en la nube para crear proyectos, generar documentación, analizar actividades y administrar tareas.

La aplicación está orientada a trabajar con especificaciones SDD/DDD, automatizar actividades repetitivas del ciclo de desarrollo y conservar la información de proyectos en una base SQLite local.

## 2. Objetivos

### Objetivo general

Acelerar el desarrollo de software a partir de las decisiones y especificaciones de un profesional, manteniendo trazabilidad de proyectos, actividades, tareas y resultados generados por IA.

### Objetivos específicos

- Crear proyectos y estructuras iniciales para soluciones .NET.
- Ejecutar prompts contra agentes locales compatibles con Ollama y APIs compatibles con OpenAI.
- Generar documentos RUP y artefactos de arquitectura.
- Gestionar proyectos, actividades y tareas desde una interfaz MAUI.
- Mantener datos locales mediante SQLite y Entity Framework Core.
- Permitir la configuración de proveedores cloud y agentes locales.
- Servir como base para automatizar features, fixes, refactors y documentación técnica.

## 3. Alcance actual

La versión actual contiene:

- Inicio de sesión local para pruebas.
- Dashboard de proyectos.
- Consulta de actividades por proyecto.
- Búsqueda y filtrado de actividades.
- Listado de tareas agrupadas por proyecto.
- Configuración de proveedores cloud y agentes locales.
- Persistencia SQLite mediante EF Core.
- Repositorios para proyectos, actividades, tareas, usuarios y configuración.
- Servicios de ejecución de prompts, generación RUP y generación de estructura de proyectos.
- Organización de la UI mediante MVVM.

La aplicación se encuentra en fase de prototipo funcional. Algunas operaciones de escritura y configuración visual todavía deben completarse.

## 4. Arquitectura de la solución

La solución está dividida en cuatro proyectos principales:

### FenixEngine

Aplicación ejecutable .NET MAUI para Android, iOS, MacCatalyst y Windows cuando el entorno lo permite.

Responsabilidades:

- Arranque de la aplicación.
- Registro final de dependencias.
- Páginas MAUI XAML.
- ViewModels.
- Navegación.
- Inicialización de SQLite.

La UI está organizada en:

```text
FenixEngine/
├── Src/
│   ├── Components/
│   │   └── pages/
│   │       ├── LoginPage.xaml
│   │       ├── MainPage.xaml
│   │       ├── WorkStationPage.xaml
│   │       ├── TaskPage.xaml
│   │       └── SettingsPage.xaml
│   └── ModelView/
│       ├── ViewModelBase.cs
│       ├── LoginPageViewModel.cs
│       ├── MainPageViewModel.cs
│       ├── WorkStationPageViewModel.cs
│       ├── TaskPageViewModel.cs
│       └── SettingsPageViewModel.cs
├── MauiProgram.cs
└── App.xaml.cs
```

### FenixEngine.Shared

Contiene modelos comunes, contratos, servicios básicos, DbContext y repositorios.

Responsabilidades:

- Entidades de usuario, proyecto, actividad, tarea y configuración.
- `ServiceDbContext`.
- Inicialización y recreación de la base SQLite.
- Repositorios de acceso a datos.
- Servicios de archivos, carpetas, terminal y logging.

### FenixEngine.Services

Contiene integraciones y servicios técnicos para agentes de IA.

Responsabilidades:

- Cliente HTTP.
- Constructor de prompts.
- Agentes de IA.
- Ejecución de prompts.
- Generación de documentos RUP.
- Generación de estructuras de proyectos.

### FenixEngine.AppCore

Contiene los controladores y servicios de aplicación. Esta capa coordina la UI con Shared y Services.

Responsabilidades:

- `ProjectPortfolioController`.
- `WorkflowController`.
- `ProjectDataService`.
- `SettingsDataService`.
- `AuthenticationService`.
- DTOs usados por la presentación.

## 5. Patrón MVVM

La UI sigue una separación MVVM:

- **Model:** entidades y DTOs definidos en Shared y AppCore.
- **View:** páginas XAML ubicadas en `FenixEngine/Src/Components/pages`.
- **ViewModel:** clases ubicadas en `FenixEngine/Src/ModelView`.

Las páginas resuelven sus ViewModels mediante inyección de dependencias y asignan `BindingContext`. La carga de datos, autenticación, filtros y agrupaciones no debe volver al code-behind salvo para navegación o interacción estrictamente visual.

## 6. Navegación

El flujo actual comienza en `LoginPage` y continúa hacia `MainPage` después de autenticarse.

Desde el dashboard se puede navegar a:

- `WorkStationPage`.
- `TaskPage`.
- `SettingsPage`.

La navegación actual utiliza `NavigationPage`. La migración completa a rutas Shell puede realizarse en una fase posterior si se requiere navegación declarativa.

## 7. Persistencia

La aplicación usa SQLite en el directorio de datos de la aplicación.

Entidades principales:

- `UserArchitect`.
- `GeneralProyects`.
- `ProyectType`.
- `UserActivity`.
- `ProjectTask`.
- `AiApiToken`.
- `LocalAgentConfiguration`.

En configuración `DEBUG`, la base se recrea al iniciar para facilitar las pruebas y aplicar el modelo actual. En `RELEASE`, el inicializador utiliza una estrategia no destructiva para conservar datos existentes.

> La recreación automática de la base en `DEBUG` elimina los datos locales de prueba en cada arranque.

## 8. Credenciales de prueba

```text
Usuario: admin@fenixengine.local
Contraseña: fenix123
```

Estas credenciales son únicamente para el entorno de pruebas. La autenticación debe evolucionar hacia almacenamiento seguro de credenciales, gestión de sesión y una política de contraseñas adecuada.

## 9. Servicios de IA

El flujo de prompt utiliza un endpoint compatible con OpenAI y puede apuntar a Ollama local.

Valores de desarrollo actuales:

```text
Base URL: http://127.0.0.1:11434/v1/
Modelo: llama3
```

Los servicios disponibles son:

- Ejecución de prompts.
- Generación de documentos RUP.
- Generación de estructura de proyecto.
- Creación de carpetas y archivos.
- Registro de actividad mediante logging.

## 10. Requisitos técnicos

- .NET 10.
- .NET MAUI.
- Entity Framework Core.
- SQLite.
- C# con nullable reference types habilitado.
- SDK y workloads MAUI instalados para el destino elegido.
- Ollama o un proveedor compatible cuando se ejecuten prompts locales.

## 11. Compilación

Ejemplo para MacCatalyst:

```bash
dotnet build -t:Build \\
  -p:Configuration=Debug \\
  -f net10.0-maccatalyst \\
  -r maccatalyst-arm64 \\
  -p:MauiDevFlowEnabled=true \\
  FenixEngine/FenixEngine.csproj
```

La compilación MacCatalyst ha sido validada correctamente. Persisten warnings externos relacionados con la vulnerabilidad conocida de `SQLitePCLRaw`.

## 12. Estado funcional

### Implementado

- Arquitectura por capas.
- DI entre proyectos.
- SQLite y EF Core.
- Repositorios de lectura y escritura básica.
- UI MAUI.
- MVVM.
- Login local de pruebas.
- Dashboard, actividades, tareas y settings.
- Workflow de prompts, RUP y arquitectura.

### Pendiente

- CRUD visual completo de proyectos.
- Edición y persistencia de switches de Settings.
- Gestión completa de actividades.
- Migraciones EF Core formales para producción.
- Sesión persistente y cierre de sesión.
- Seguridad de credenciales con PBKDF2, BCrypt o equivalente.
- Pruebas unitarias y de integración.
- Validación visual automatizada mediante MauiDevFlow.
- Actualización de la dependencia SQLite vulnerable.

## 13. Convenciones de desarrollo

- La UI solo debe depender directamente de AppCore y de sus propios ViewModels.
- No consultar DbContext desde páginas XAML.
- Usar `CollectionView` para listados y evitar `ListView`.
- Mantener las operaciones de persistencia en repositorios.
- Mantener reglas de aplicación en AppCore.
- Mantener integraciones externas en Services.
- Mantener entidades y contratos compartidos en Shared.
- Usar nombres de archivos y namespaces coherentes con la estructura actual.

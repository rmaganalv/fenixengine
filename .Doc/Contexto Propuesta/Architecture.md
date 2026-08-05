### Arquitectura del sistema ERP

### Objetivos de arquitectura
* Garantizar la separación de responsabilidades entre los componentes de la plataforma para facilitar el desarrollo, el mantenimiento y la cobertura de los casos de uso del negocio.
* Dividir la plataforma en módulos independientes que representen áreas de especialización, con el fin de evitar acoplamientos y procesos cruzados entre módulos.
* Diseñar un flujo de trabajo estable a través de conexiones a una base de datos relacional empresarial para centralizar la gestión de la información crítica del usuario final.
* Garantizar procesos auditables para los casos de uso críticos de la plataforma mediante registros de logs y mecanismos de seguridad estándar basados en OWASP Top 10 2022.
* Facilitar la escalabilidad de la plataforma para soportar el crecimiento del sistema, la incorporación de nuevas funcionalidades y la corrección de errores.
* Mantener la estabilidad del sistema mediante la optimización de recursos y la adopción de arquitecturas de software que sirvan como base sólida para la integración de nuevos componentes.
 



El ERP "Kaisen" esta fragmentado en 3 partes principales para ser identidicado rapidamente durante el desarrollo, estan construidos bajo el ecosistema de .NET para garantizar la robustez. cada proyecto tiene su propia arquitectura de software. 

1. Kaisen.WebApi
    * Esta capa estará construida sobre una arquitectura MVC como base, y contará con soporte de desarrollo basado en CQRS para promover una arquitectura limpia y escalable.
    * Su función principal será actuar como proveedor de información y administrador de la base de datos del ERP, exponiendo los servicios necesarios para que puedan ser consumidos por Kaisen.Web y Kaisen.Mobile.

2. Kaisen.Web
    * Esta aplicación estará construida sobre una arquitectura MVVM como base y consumirá los servicios expuestos por Kaisen.WebApi. 
    * Su función principal será ofrecer la interfaz web del ERP para que el usuario final interactúe con el sistema desde un navegador.

3. Kaisen.Mobile
    * Esta aplicación también estará construida sobre una arquitectura MVVM y consumirá la API de Kaisen.WebApi.
    * Su función principal será ofrecer una interfaz alternativa del ERP para que eßl usuario final interactúe desde dispositivos móviles. Debido a su naturaleza como vista alternativa, estará diseñada para ofrecer funcionalidades similares a la versión web, aunque contará con su propia base de datos local para aprovechar el almacenamiento del dispositivo y permitir el trabajo sin depender completamente del acceso a la información en producción.





Estructura de carpetas, No representa la estructura final a productivo, sin embargo representa una estructura que se mantendra como una arquitectura que desacopla los componentes del sistema


Kaisen.sln  (Archivo de Solución Global)
│
├── 📁 apps/  (Aplicaciones ejecutables y puntos de entrada)
│   └── 🚀 Kaisen.Erp.WebAPI/
│       ├── 📁 Controllers/         (Endpoints REST tradicionales)
│       ├── 📁 gRPC/                (Servicios gRPC que exponen el ERP a plugins de Java/C++)
│       │   └── 📄 FinanzasService.cs
│       ├── 📁 Extensions/          (Configuración de DI, Swagger, JWT, etc.)
│       ├── 📄 Program.cs           (Configura y arranca TODOS los módulos en un solo proceso)
│       └── 📄 appsettings.json
│
├── 📁 building-blocks/  (Código compartido e infraestructura base inmutable)
│   ├── 📦 Kaisen.BuildingBlocks.Domain/       (Interfaces base como IAggregateRoot, Entity, ValueObject)
│   ├── 📦 Kaisen.BuildingBlocks.Application/  (Abstracciones de CQRS, ICommand, IQuery, EventBus)
│   └── 📦 Kaisen.BuildingBlocks.Infrastructure/ (Configuraciones base de EF Core, gRPC Helpers)
│
└── 📁 modules/  (El núcleo de tu negocio separado por módulos independientes)
    │
    ├── 📦 Finanzas/  (Ejemplo de Módulo Completo con Clean Architecture + CQRS)
    │   │
    │   ├── 🔵 Kaisen.Modules.Finanzas.Domain/
    │   │   ├── 📁 Aggregates/
    │   │   │   └── 📁 FacturaAggregate/
    │   │   │       ├── 📄 Factura.cs         (Entidad Principal / Raíz del Agregado)
    │   │   │       ├── 📄 LineaFactura.cs    (Entidad Secundaria)
    │   │   │       └── 📄 FacturaId.cs       (Value Object)
    │   │   ├── 📁 ValueObjects/              (Ej. Moneda.cs, RFC.cs)
    │   │   ├── 📁 Events/                    (Domain Events: ej. FacturaCreadaDomainEvent.cs)
    │   │   ├── 📁 Exceptions/                (Ej. FacturaInvalidaException.cs)
    │   │   └── 📁 Repositories/              (Interfaces puras: ej. IFacturaRepository.cs)
    │   │
    │   ├── 🟢 Kaisen.Modules.Finanzas.Application/
    │   │   ├── 📁 Abstracciones/             (Interfaces locales del módulo)
    │   │   ├── 📁 Contracts/                 (DTOs públicos que otros módulos pueden ver)
    │   │   ├── 📁 Features/                  (Casos de Uso divididos por CQRS)
    │   │   │   └── 📁 Facturas/
    │   │   │       ├── 📁 Commands/          (Escrituras)
    │   │   │       │   ├── 📄 CrearFacturaCommand.cs
    │   │   │       │   ├── 📄 CrearFacturaCommandHandler.cs
    │   │   │       │   └── 📄 CrearFacturaCommandValidator.cs (FluentValidation)
    │   │   │       └── 📁 Queries/           (Lecturas optimizadas)
    │   │   │           ├── 📄 GetFacturaByIdQuery.cs
    │   │   │           ├── 📄 GetFacturaByIdQueryHandler.cs
    │   │   │           └── 📄 FacturaDto.cs
    │   │   └── 📁 EventHandlers/             (Manejadores de eventos de integración o dominio)
    │   │
    │   └── 🟡 Kaisen.Modules.Finanzas.Infrastructure/
    │       ├── 📁 Database/
    │       │   ├── 📄 FinanzasDbContext.cs    (EF Core DbContext exclusivo de este módulo)
    │       │   ├── 📁 Configurations/         (Mapeo de tablas Fluent API, ej. FacturaConfig.cs)
    │       │   └── 📁 Migrations/             (Migraciones SQL aisladas para este módulo)
    │       ├── 📁 Repositories/               (Implementación de interfaces de dominio)
    │       │   └── 📄 FacturaRepository.cs
    │       ├── 📁 Services/                   (Clientes de APIs externas, ej. SatFacturacionService.cs)
    │       └── 📄 FinanzasStartup.cs          (Clase de extensión para inicializar e inyectar el módulo)
    │
    ├── 📦 Inventarios/ (Sigue exactamente la misma estructura de 3 capas que Finanzas)
    │   ├── 🔵 Kaisen.Modules.Inventarios.Domain/
    │   ├── 🟢 Kaisen.Modules.Inventarios.Application/
    │   └── 🟡 Kaisen.Modules.Inventarios.Infrastructure/
    │
    └── 📁 Protos/  (Ubicación global de los contratos gRPC para extensiones Java, C++, etc.)
        ├── 📄 common.proto
        ├── 📄 finanzas.proto                  (Define qué comandos/queries pueden invocar desde fuera)
        └── 📄 inventarios.proto


* 1. La aplicación ejecutable (apps/Kaisen.WebAPI)
Es el único proyecto que "arranca". En su archivo Program.cs, invoca los métodos de inicialización de cada módulo (ej. builder.Services.AddFinanzasModule(configuration);). El programador del módulo de Inventarios no tiene por qué tocar este proyecto, manteniendo el aislamiento.
* 2. Los Bloques de Construcción (building-blocks/)
Para evitar duplicar código común (como la lógica para manejar IDs, auditorías de fechas de creación, o la estructura base de los comandos de MediatR), creas estos proyectos base. Todos los módulos los heredan, garantizando un estándar de programación unificado en todo tu ERP.
* 3. CQRS encapsulado por Características (Features/)
En lugar de tener una carpeta gigante de "Commands" y otra de "Queries" entremezcladas, la estructura se organiza por Feature (Característica de Negocio). Dentro de la carpeta Facturas, tienes agrupado todo lo relacionado con esa acción: su comando, su manejador (Handler) y su validador. Es sumamente fácil de mantener y navegar.
* 4. Capa de Lectura Directa en las Queries
Dentro de Queries/GetFacturaByIdQueryHandler.cs, tienes la libertad arquitectónica de no usar EF Core. Puedes inyectar una conexión SQL pura (usando Dapper) y ejecutar un SELECT * FROM Finanzas.Facturas WHERE Id = @Id, mapeándolo directamente a tu FacturaDto. Velocidad pura para las pantallas del ERP.
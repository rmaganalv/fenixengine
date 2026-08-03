                  ┌──────────────────────────────────────────────┐
                  │          App .NET MAUI (UI)                  │
                  └──────────────────────┬───────────────────────┘
                                         │
                                         ▼
                  ┌──────────────────────────────────────────────┐
                  │    AGENTE MAESTRO / ORQUESTADOR             │
                  │   (Microsoft.AI.Foundry.Local + MAUI UI)    │
                  │  - Ejecución 100% Offline / Privada          │
                  │  - Evalúa la intención del usuario           │
                  │  - Decide si delega o supervisa              │
                  └──────┬───────────────────────────────┬───────┘
                         │                               │
       (Llamadas locales │                               │ (Llamadas API / Cloud / Rest)
        in-process / NPU)│                               │
                         ▼                               ▼
  ┌──────────────────────────────┐               ┌──────────────────────────────┐
  │ AGENTES / MODELOS ESPECIALES │               │ AGENTES REMOTOS / EN LA NUBE │
  │        (Locales)             │               │   (API / Cloud Orchestration)│
  │ - Modelo A: Llama / Phi      │               │ - Agente Cloud: GPT-4o /     │
  │ - Descargados por el usuario │               │   DeepSeek / Azure OpenAI    │
  │ - Tareas offline y privadas  │               │ - Tareas de alta capacidad,  │
  └──────────────────────────────┘               │   búsqueda web, RAG pesado   │
                                                 └──────────────────────────────┘
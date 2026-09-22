// Copyright (C) 2026 Ruben Magaña Alvarado
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using Microsoft.EntityFrameworkCore;
using FenixEngine.Shared.Src.Models;
using Microsoft.Maui.Storage;


namespace FenixEngine.Shared.Src.Services.DataBase
{
    public class ServiceDbContext : DbContext
    {
        // 1. Quitar 'static' y registrar todas las tablas requeridas
        public DbSet<UserArchitect> UserArchitects { get; set; } = null!;
        public DbSet<GeneralProyects> GeneralProyects { get; set; } = null!;
        public DbSet<ProyectType> ProyectTypes { get; set; } = null!;
        public DbSet<UserActivity> UserActivities { get; set; } = null!;
        public DbSet<ProjectTask> ProjectTasks { get; set; } = null!;
        public DbSet<AiApiToken> AiApiTokens { get; set; } = null!;
        public DbSet<LocalAgentConfiguration> LocalAgentConfigurations { get; set; } = null!;

        // Constructor para soportar Inyección de Dependencias
        public ServiceDbContext(DbContextOptions<ServiceDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string databasePath = Path.Combine(FileSystem.AppDataDirectory, "fenix-engine.db3");
                optionsBuilder.UseSqlite($"Data Source={databasePath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserActivity>()
                .HasKey(ua => ua.ActivityId);

            modelBuilder.Entity<ProyectType>().HasData(
                new ProyectType { PTypeId = "type-local", PTypeName = "Local", PTypeCreation = new DateTime(2026, 1, 1, 8, 0, 0, DateTimeKind.Utc) },
                new ProyectType { PTypeId = "type-external", PTypeName = "External", PTypeCreation = new DateTime(2026, 1, 1, 8, 0, 0, DateTimeKind.Utc) }
            );

            modelBuilder.Entity<UserArchitect>().HasData(
                new UserArchitect
                {
                    UserId = "user-big-boss",
                    UserName = "Big Boss",
                    Email = "admin@fenixengine.local",
                    PasswordHash = "cdded2397c42dda1e1f9cf3881f12bad2311d1c78735d052951a644da4fb0d07",
                    UserBirth = new DateTime(1988, 8, 7),
                    UserCreation = new DateTime(2026, 1, 1, 8, 0, 0, DateTimeKind.Utc),
                    UserEdit = null
                }
            );

            modelBuilder.Entity<GeneralProyects>().HasData(
                new GeneralProyects
                {
                    GProyectId = "project-fenixcore",
                    GPtypeId = "type-local",
                    GProyectName = "FenixCore",
                    GProyectComment = "Proyecto base de la plataforma",
                    GProyectCreate = new DateTime(2026, 1, 2, 9, 30, 0, DateTimeKind.Utc),
                    GProyectEdit = new DateTime(2026, 1, 10, 10, 15, 0, DateTimeKind.Utc),
                    GProyectClose = null
                },
                new GeneralProyects
                {
                    GProyectId = "project-atlas-ia",
                    GPtypeId = "type-external",
                    GProyectName = "Atlas IA",
                    GProyectComment = "Integración con modelos externos",
                    GProyectCreate = new DateTime(2026, 1, 5, 8, 0, 0, DateTimeKind.Utc),
                    GProyectEdit = new DateTime(2026, 1, 12, 16, 45, 0, DateTimeKind.Utc),
                    GProyectClose = null
                },
                new GeneralProyects
                {
                    GProyectId = "project-finops",
                    GPtypeId = "type-local",
                    GProyectName = "FinOps Planner",
                    GProyectComment = "Alineación financiera y analítica",
                    GProyectCreate = new DateTime(2026, 1, 7, 9, 0, 0, DateTimeKind.Utc),
                    GProyectEdit = new DateTime(2026, 1, 14, 11, 30, 0, DateTimeKind.Utc),
                    GProyectClose = null
                }
            );

            modelBuilder.Entity<UserActivity>().HasData(
                new UserActivity
                {
                    ActivityId = "activity-fenixcore-1",
                    UserId = "user-big-boss",
                    ProyectId = "project-fenixcore",
                    UActivityCreation = new DateTime(2026, 1, 14, 9, 15, 0, DateTimeKind.Utc)
                },
                new UserActivity
                {
                    ActivityId = "activity-atlas-1",
                    UserId = "user-big-boss",
                    ProyectId = "project-atlas-ia",
                    UActivityCreation = new DateTime(2026, 1, 13, 12, 0, 0, DateTimeKind.Utc)
                },
                new UserActivity
                {
                    ActivityId = "activity-finops-1",
                    UserId = "user-big-boss",
                    ProyectId = "project-finops",
                    UActivityCreation = new DateTime(2026, 1, 12, 15, 30, 0, DateTimeKind.Utc)
                }
            );

            modelBuilder.Entity<ProjectTask>().HasData(
                new ProjectTask
                {
                    TaskId = "task-fenixcore-1",
                    ProjectId = "project-fenixcore",
                    Title = "Configurar motor de validación",
                    Description = "Preparar las reglas iniciales del proyecto.",
                    State = "Pending",
                    Priority = 1,
                    CreatedAt = new DateTime(2026, 1, 14, 10, 0, 0, DateTimeKind.Utc)
                },
                new ProjectTask
                {
                    TaskId = "task-atlas-1",
                    ProjectId = "project-atlas-ia",
                    Title = "Preparar servicio de nube",
                    Description = "Configurar el proveedor de IA externo.",
                    State = "InProgress",
                    Priority = 2,
                    CreatedAt = new DateTime(2026, 1, 13, 14, 0, 0, DateTimeKind.Utc)
                },
                new ProjectTask
                {
                    TaskId = "task-finops-1",
                    ProjectId = "project-finops",
                    Title = "Migración de datos",
                    Description = "Revisar el esquema inicial de persistencia.",
                    State = "Completed",
                    Priority = 1,
                    CreatedAt = new DateTime(2026, 1, 12, 16, 0, 0, DateTimeKind.Utc),
                    CompletedAt = new DateTime(2026, 1, 14, 16, 0, 0, DateTimeKind.Utc)
                }
            );

            modelBuilder.Entity<AiApiToken>().HasData(
                new AiApiToken { Id = 1, ProviderName = "Azure OpenAI", Token = "", SubscriptionType = ApiSubscriptionType.Paid, ProjectName = "OpenAI", IsActive = false, CreatedAt = new DateTime(2026, 1, 1, 8, 0, 0, DateTimeKind.Utc) },
                new AiApiToken { Id = 2, ProviderName = "Claude", Token = "", SubscriptionType = ApiSubscriptionType.Paid, ProjectName = "Anthropic", IsActive = false, CreatedAt = new DateTime(2026, 1, 1, 8, 0, 0, DateTimeKind.Utc) },
                new AiApiToken { Id = 3, ProviderName = "Vertex AI", Token = "", SubscriptionType = ApiSubscriptionType.Paid, ProjectName = "Google Gemini", IsActive = false, CreatedAt = new DateTime(2026, 1, 1, 8, 0, 0, DateTimeKind.Utc) }
            );

            modelBuilder.Entity<LocalAgentConfiguration>().HasData(
                new LocalAgentConfiguration { AgentId = "agent-ollama", Name = "Ollama Core", Endpoint = "http://127.0.0.1:11434", IsEnabled = true, UpdatedAt = new DateTime(2026, 1, 1, 8, 0, 0, DateTimeKind.Utc) },
                new LocalAgentConfiguration { AgentId = "agent-mistral", Name = "Mistral Local", Endpoint = "http://127.0.0.1:11435", IsEnabled = false, UpdatedAt = new DateTime(2026, 1, 1, 8, 0, 0, DateTimeKind.Utc) }
            );
        }

    }


}
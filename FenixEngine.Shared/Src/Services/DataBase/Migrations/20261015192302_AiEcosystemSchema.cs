using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FenixEngine.Shared.Src.Services.DataBase.Migrations;

public partial class AiEcosystemSchema : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            CREATE TABLE IF NOT EXISTS "AiProviders" (
                "Id" TEXT NOT NULL CONSTRAINT "PK_AiProviders" PRIMARY KEY,
                "Name" TEXT NOT NULL,
                "ProviderType" TEXT NOT NULL,
                "BaseUrl" TEXT NOT NULL,
                "IsLocal" INTEGER NOT NULL,
                "IsEnabled" INTEGER NOT NULL,
                "CreatedAt" TEXT NOT NULL,
                "UpdatedAt" TEXT NOT NULL
            );
            CREATE UNIQUE INDEX IF NOT EXISTS "IX_AiProviders_Name" ON "AiProviders" ("Name");
            CREATE INDEX IF NOT EXISTS "IX_AiProviders_ProviderType" ON "AiProviders" ("ProviderType");

            CREATE TABLE IF NOT EXISTS "AgentTeams" (
                "Id" TEXT NOT NULL CONSTRAINT "PK_AgentTeams" PRIMARY KEY,
                "Name" TEXT NOT NULL,
                "Description" TEXT NOT NULL
            );
            CREATE UNIQUE INDEX IF NOT EXISTS "IX_AgentTeams_Name" ON "AgentTeams" ("Name");

            CREATE TABLE IF NOT EXISTS "LlmModels" (
                "Id" TEXT NOT NULL CONSTRAINT "PK_LlmModels" PRIMARY KEY,
                "ProviderId" TEXT NOT NULL,
                "Name" TEXT NOT NULL,
                "ModelIdentifier" TEXT NOT NULL,
                "ContextWindow" INTEGER NULL,
                "IsEnabled" INTEGER NOT NULL,
                CONSTRAINT "FK_LlmModels_AiProviders_ProviderId"
                    FOREIGN KEY ("ProviderId") REFERENCES "AiProviders" ("Id") ON DELETE RESTRICT
            );
            CREATE UNIQUE INDEX IF NOT EXISTS "IX_LlmModels_ProviderId_ModelIdentifier"
                ON "LlmModels" ("ProviderId", "ModelIdentifier");

            CREATE TABLE IF NOT EXISTS "AiAgents" (
                "Id" TEXT NOT NULL CONSTRAINT "PK_AiAgents" PRIMARY KEY,
                "Name" TEXT NOT NULL,
                "Description" TEXT NOT NULL,
                "Role" TEXT NOT NULL,
                "ModelId" TEXT NOT NULL,
                "SystemPrompt" TEXT NOT NULL,
                "IsEnabled" INTEGER NOT NULL,
                CONSTRAINT "FK_AiAgents_LlmModels_ModelId"
                    FOREIGN KEY ("ModelId") REFERENCES "LlmModels" ("Id") ON DELETE RESTRICT
            );
            CREATE UNIQUE INDEX IF NOT EXISTS "IX_AiAgents_Name" ON "AiAgents" ("Name");
            CREATE INDEX IF NOT EXISTS "IX_AiAgents_ModelId" ON "AiAgents" ("ModelId");

            CREATE TABLE IF NOT EXISTS "AgentTeamMembers" (
                "TeamId" TEXT NOT NULL,
                "AgentId" TEXT NOT NULL,
                CONSTRAINT "PK_AgentTeamMembers" PRIMARY KEY ("TeamId", "AgentId"),
                CONSTRAINT "FK_AgentTeamMembers_AgentTeams_TeamId"
                    FOREIGN KEY ("TeamId") REFERENCES "AgentTeams" ("Id") ON DELETE CASCADE,
                CONSTRAINT "FK_AgentTeamMembers_AiAgents_AgentId"
                    FOREIGN KEY ("AgentId") REFERENCES "AiAgents" ("Id") ON DELETE CASCADE
            );
            CREATE INDEX IF NOT EXISTS "IX_AgentTeamMembers_AgentId" ON "AgentTeamMembers" ("AgentId");
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            DROP TABLE IF EXISTS "AgentTeamMembers";
            DROP TABLE IF EXISTS "AiAgents";
            DROP TABLE IF EXISTS "LlmModels";
            DROP TABLE IF EXISTS "AgentTeams";
            DROP TABLE IF EXISTS "AiProviders";
            """);
    }
}
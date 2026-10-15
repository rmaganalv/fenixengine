using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FenixEngine.Shared.Src.Services.DataBase.Migrations;

public partial class DevelopmentProjectsSchema : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            CREATE TABLE IF NOT EXISTS "DevelopmentProjects" (
                "Id" TEXT NOT NULL CONSTRAINT "PK_DevelopmentProjects" PRIMARY KEY,
                "Name" TEXT COLLATE NOCASE NOT NULL,
                "Description" TEXT NOT NULL,
                "BusinessGoal" TEXT NOT NULL,
                "FunctionalRequirements" TEXT NOT NULL,
                "NonFunctionalRequirements" TEXT NOT NULL,
                "ArchitectureStyle" TEXT NOT NULL,
                "TechnologyStack" TEXT NOT NULL,
                "Status" INTEGER NOT NULL,
                "CreatedAt" TEXT NOT NULL,
                "UpdatedAt" TEXT NOT NULL,
                "IsActive" INTEGER NOT NULL
            );
            CREATE UNIQUE INDEX IF NOT EXISTS "IX_DevelopmentProjects_Name"
                ON "DevelopmentProjects" ("Name");
            CREATE INDEX IF NOT EXISTS "IX_DevelopmentProjects_Status"
                ON "DevelopmentProjects" ("Status");
            CREATE INDEX IF NOT EXISTS "IX_DevelopmentProjects_IsActive"
                ON "DevelopmentProjects" ("IsActive");

            CREATE TABLE IF NOT EXISTS "DevelopmentProjectTeams" (
                "ProjectId" TEXT NOT NULL,
                "TeamId" TEXT NOT NULL,
                CONSTRAINT "PK_DevelopmentProjectTeams" PRIMARY KEY ("ProjectId", "TeamId"),
                CONSTRAINT "FK_DevelopmentProjectTeams_DevelopmentProjects_ProjectId"
                    FOREIGN KEY ("ProjectId") REFERENCES "DevelopmentProjects" ("Id") ON DELETE CASCADE,
                CONSTRAINT "FK_DevelopmentProjectTeams_AgentTeams_TeamId"
                    FOREIGN KEY ("TeamId") REFERENCES "AgentTeams" ("Id") ON DELETE CASCADE
            );
            CREATE INDEX IF NOT EXISTS "IX_DevelopmentProjectTeams_TeamId"
                ON "DevelopmentProjectTeams" ("TeamId");

            CREATE TABLE IF NOT EXISTS "ProjectArtifacts" (
                "Id" TEXT NOT NULL CONSTRAINT "PK_ProjectArtifacts" PRIMARY KEY,
                "ProjectId" TEXT NOT NULL,
                "ArtifactType" INTEGER NOT NULL,
                "Title" TEXT NOT NULL,
                "Description" TEXT NOT NULL,
                "FilePath" TEXT NOT NULL,
                "CreatedAt" TEXT NOT NULL,
                CONSTRAINT "FK_ProjectArtifacts_DevelopmentProjects_ProjectId"
                    FOREIGN KEY ("ProjectId") REFERENCES "DevelopmentProjects" ("Id") ON DELETE CASCADE
            );
            CREATE INDEX IF NOT EXISTS "IX_ProjectArtifacts_ProjectId_CreatedAt"
                ON "ProjectArtifacts" ("ProjectId", "CreatedAt");
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            DROP TABLE IF EXISTS "ProjectArtifacts";
            DROP TABLE IF EXISTS "DevelopmentProjectTeams";
            DROP TABLE IF EXISTS "DevelopmentProjects";
            """);
    }
}
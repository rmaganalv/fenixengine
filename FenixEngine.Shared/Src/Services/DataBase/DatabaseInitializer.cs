using Microsoft.EntityFrameworkCore;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.Shared.Src.Services.DataBase;

public static class DatabaseInitializer
{
    public static void Recreate(ServiceDbContext dbContext)
    {
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
    }

    public static void Initialize(ServiceDbContext dbContext)
    {
        dbContext.Database.EnsureCreated();

        using var connection = dbContext.Database.GetDbConnection();
        connection.Open();

        EnsureColumn(connection, "UserArchitects", "Email", "TEXT NULL");
        EnsureColumn(connection, "UserArchitects", "PasswordHash", "TEXT NULL");

        Execute(connection, """
            CREATE TABLE IF NOT EXISTS ProjectTasks (
                TaskId TEXT NOT NULL CONSTRAINT PK_ProjectTasks PRIMARY KEY,
                ProjectId TEXT NOT NULL,
                Title TEXT NOT NULL,
                Description TEXT NULL,
                State TEXT NOT NULL,
                Priority INTEGER NOT NULL,
                CreatedAt TEXT NOT NULL,
                DueAt TEXT NULL,
                CompletedAt TEXT NULL
            );
            """);

        Execute(connection, """
            CREATE TABLE IF NOT EXISTS AiApiTokens (
                Id INTEGER NOT NULL CONSTRAINT PK_AiApiTokens PRIMARY KEY AUTOINCREMENT,
                ProviderName TEXT NOT NULL,
                Token TEXT NOT NULL,
                ProjectName TEXT NULL,
                SubscriptionType INTEGER NOT NULL,
                IsActive INTEGER NOT NULL,
                CreatedAt TEXT NOT NULL,
                LastUsedAt TEXT NULL,
                ExpiresAt TEXT NULL
            );
            """);

        Execute(connection, """
            CREATE TABLE IF NOT EXISTS LocalAgentConfigurations (
                AgentId TEXT NOT NULL CONSTRAINT PK_LocalAgentConfigurations PRIMARY KEY,
                Name TEXT NOT NULL,
                Endpoint TEXT NOT NULL,
                IsEnabled INTEGER NOT NULL,
                UpdatedAt TEXT NOT NULL
            );
            """);

        Execute(connection, """
            INSERT OR IGNORE INTO UserArchitects
                (UserId, UserName, Email, PasswordHash, UserBirth, UserCreation, UserEdit)
            VALUES
                ('user-big-boss', 'Big Boss', 'admin@fenixengine.local',
                 'cdded2397c42dda1e1f9cf3881f12bad2311d1c78735d052951a644da4fb0d07',
                 '1988-08-07 00:00:00', '2026-01-01 08:00:00', NULL);
            """);
    }

    private static void EnsureColumn(System.Data.Common.DbConnection connection, string tableName, string columnName, string definition)
    {
        using var command = connection.CreateCommand();
        command.CommandText = $"PRAGMA table_info([{tableName}]);";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            if (string.Equals(reader.GetString(1), columnName, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
        }

        reader.Close();
        Execute(connection, $"ALTER TABLE [{tableName}] ADD COLUMN [{columnName}] {definition};");
    }

    private static void Execute(System.Data.Common.DbConnection connection, string sql)
    {
        using var command = connection.CreateCommand();
        command.CommandText = sql;
        command.ExecuteNonQuery();
    }
}
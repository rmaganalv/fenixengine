using FenixEngine.Shared.Src.Services;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.Services.Src.Workflow;

public class ArchitectureProjectService : IArchitectureProjectService
{
    private readonly IFileService _fileService;
    private readonly IFolderService _folderService;
    private readonly ITerminalService _terminalService;
    private readonly ILoggerService _loggerService;

    public ArchitectureProjectService(
        IFileService fileService,
        IFolderService folderService,
        ITerminalService terminalService,
        ILoggerService loggerService)
    {
        _fileService = fileService;
        _folderService = folderService;
        _terminalService = terminalService;
        _loggerService = loggerService;
    }

    public async Task<string> GenerateProjectStructureAsync(string projectName, string stack, string prompt, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(projectName)) throw new ArgumentException("Project name is required.", nameof(projectName));

        var rootPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FenixEngine", "Projects", projectName);
        await _folderService.CreateFolder(rootPath);
        await _folderService.CreateFolder(Path.Combine(rootPath, "src"));
        await _folderService.CreateFolder(Path.Combine(rootPath, "docs"));
        await _folderService.CreateFolder(Path.Combine(rootPath, "tests"));

        var readmePath = Path.Combine(rootPath, "README.md");
        await _fileService.CreateFile(readmePath);
        await _fileService.EditFile(readmePath, $"# {projectName}\n\nStack: {stack}\n\nDescripción: {prompt}\n");

        _loggerService.Info($"Estructura generada para '{projectName}' en '{rootPath}'");
        return rootPath;
    }
}

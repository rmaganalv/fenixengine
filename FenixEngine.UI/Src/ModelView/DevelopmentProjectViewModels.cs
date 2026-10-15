using System.Collections.ObjectModel;
using System.Windows.Input;
using FenixEngine.AppCore.Src.Models;
using FenixEngine.AppCore.Src.Services;
using FenixEngine.Shared.Src.Models;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.UI.ModelView;

public interface IDevelopmentProjectRoutePage
{
    Task LoadRouteAsync(string? routeId);
}

public static class DevelopmentProjectNavigation
{
    public static async Task NavigateAsync(IServiceProvider services, INavigation navigation, Type pageType, string? routeId = null)
    {
        var page = services.GetRequiredService(pageType) as Page
            ?? throw new InvalidOperationException($"No hay una página registrada para {pageType.Name}.");
        if (page is IDevelopmentProjectRoutePage routePage)
            await routePage.LoadRouteAsync(routeId);
        await navigation.PushAsync(page);
    }
}

public abstract class DevelopmentProjectViewModelBase : ViewModelBase
{
    private bool _isBusy;
    private string _statusMessage = string.Empty;

    public bool IsBusy { get => _isBusy; protected set => SetProperty(ref _isBusy, value); }
    public string StatusMessage { get => _statusMessage; protected set => SetProperty(ref _statusMessage, value); }
    public Func<Type, string?, Task>? NavigateAsync { get; set; }

    protected async Task ExecuteAsync(Func<Task> action)
    {
        if (IsBusy) return;
        IsBusy = true;
        StatusMessage = string.Empty;
        try
        {
            await action();
        }
        catch (Exception exception)
        {
            StatusMessage = exception.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    protected Task NavigateToAsync(Type pageType, string? routeId = null) =>
        NavigateAsync?.Invoke(pageType, routeId) ?? Task.CompletedTask;
}

public sealed record ProjectStatusOption(string Label, ProjectStatus? Value);

public sealed class ProjectsPageViewModel : DevelopmentProjectViewModelBase
{
    private readonly DevelopmentProjectService _service;
    private string _searchText = string.Empty;
    private ProjectStatusOption _selectedStatus;

    public ObservableCollection<DevelopmentProjectListItemDto> Projects { get; } = new();
    public IReadOnlyList<ProjectStatusOption> StatusOptions { get; } =
        new[] { new ProjectStatusOption("Todos los estados", null) }
            .Concat(Enum.GetValues<ProjectStatus>().Select(status => new ProjectStatusOption(status.ToString(), status)))
            .ToArray();
    public ICommand SearchCommand { get; }
    public ICommand CreateCommand { get; }
    public ICommand OpenDetailsCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand TeamsCommand { get; }
    public ICommand ArtifactsCommand { get; }

    public string SearchText { get => _searchText; set => SetProperty(ref _searchText, value); }
    public ProjectStatusOption SelectedStatus
    {
        get => _selectedStatus;
        set
        {
            if (SetProperty(ref _selectedStatus, value)) _ = LoadAsync();
        }
    }

    public ProjectsPageViewModel(DevelopmentProjectService service)
    {
        _service = service;
        _selectedStatus = StatusOptions[0];
        SearchCommand = new Command(async () => await LoadAsync());
        CreateCommand = new Command(async () => await NavigateToAsync(typeof(ProjectCreatePage)));
        OpenDetailsCommand = new Command<DevelopmentProjectListItemDto>(async project => await NavigateToAsync(typeof(ProjectDetailsPage), project.Id));
        EditCommand = new Command<DevelopmentProjectListItemDto>(async project => await NavigateToAsync(typeof(ProjectCreatePage), project.Id));
        TeamsCommand = new Command<DevelopmentProjectListItemDto>(async project => await NavigateToAsync(typeof(ProjectTeamsPage), project.Id));
        ArtifactsCommand = new Command<DevelopmentProjectListItemDto>(async project => await NavigateToAsync(typeof(ProjectArtifactsPage), project.Id));
    }

    public async Task LoadAsync() => await ExecuteAsync(async () =>
    {
        var projects = await _service.GetProjectsAsync(SearchText, SelectedStatus.Value, isActive: null);
        Projects.Clear();
        foreach (var project in projects) Projects.Add(project);
        StatusMessage = $"{Projects.Count} proyectos";
    });
}

public sealed class ProjectTeamSelectionViewModel : ViewModelBase
{
    private bool _isSelected;
    public string Id { get; }
    public string Name { get; }
    public string AgentSummary { get; }
    public string ModelSummary { get; }
    public bool IsSelected { get => _isSelected; set => SetProperty(ref _isSelected, value); }

    public ProjectTeamSelectionViewModel(DevelopmentProjectTeamDto team)
    {
        Id = team.Id;
        Name = team.Name;
        AgentSummary = team.AgentNames.Count == 0 ? "Sin agentes" : string.Join(", ", team.AgentNames);
        ModelSummary = team.AgentModelNames.Count == 0 ? "Sin modelos" : string.Join(", ", team.AgentModelNames);
        IsSelected = team.IsAssigned;
    }
}

public sealed class ProjectCreatePageViewModel : DevelopmentProjectViewModelBase
{
    private readonly DevelopmentProjectService _service;
    private string? _projectId;
    private string _name = string.Empty;
    private string _description = string.Empty;
    private string _businessGoal = string.Empty;
    private string _functionalRequirements = string.Empty;
    private string _nonFunctionalRequirements = string.Empty;
    private string _architectureStyle = string.Empty;
    private string _technologyStack = string.Empty;
    private ProjectStatus _status = ProjectStatus.Draft;
    private bool _isActive = true;

    public ObservableCollection<ProjectTeamSelectionViewModel> Teams { get; } = new();
    public IReadOnlyList<ProjectStatus> Statuses { get; } = Enum.GetValues<ProjectStatus>();
    public ICommand SaveCommand { get; }

    public string Name { get => _name; set => SetProperty(ref _name, value); }
    public string Description { get => _description; set => SetProperty(ref _description, value); }
    public string BusinessGoal { get => _businessGoal; set => SetProperty(ref _businessGoal, value); }
    public string FunctionalRequirements { get => _functionalRequirements; set => SetProperty(ref _functionalRequirements, value); }
    public string NonFunctionalRequirements { get => _nonFunctionalRequirements; set => SetProperty(ref _nonFunctionalRequirements, value); }
    public string ArchitectureStyle { get => _architectureStyle; set => SetProperty(ref _architectureStyle, value); }
    public string TechnologyStack { get => _technologyStack; set => SetProperty(ref _technologyStack, value); }
    public ProjectStatus Status { get => _status; set => SetProperty(ref _status, value); }
    public bool IsActive { get => _isActive; set => SetProperty(ref _isActive, value); }
    public string PageTitle => _projectId is null ? "Nuevo proyecto" : "Editar proyecto";

    public ProjectCreatePageViewModel(DevelopmentProjectService service)
    {
        _service = service;
        SaveCommand = new Command(async () => await SaveAsync());
    }

    public async Task LoadAsync(string? projectId = null) => await ExecuteAsync(async () =>
    {
        _projectId = projectId;
        Teams.Clear();
        if (projectId is null)
        {
            foreach (var team in await _service.GetAvailableTeamsAsync()) Teams.Add(new ProjectTeamSelectionViewModel(team));
            OnPropertyChanged(nameof(PageTitle));
            return;
        }

        var project = await _service.GetProjectAsync(projectId);
        Name = project.Name;
        Description = project.Description;
        BusinessGoal = project.BusinessGoal;
        FunctionalRequirements = project.FunctionalRequirements;
        NonFunctionalRequirements = project.NonFunctionalRequirements;
        ArchitectureStyle = project.ArchitectureStyle;
        TechnologyStack = project.TechnologyStack;
        Status = project.Status;
        IsActive = project.IsActive;
        foreach (var team in await _service.GetAvailableTeamsAsync(projectId)) Teams.Add(new ProjectTeamSelectionViewModel(team));
        OnPropertyChanged(nameof(PageTitle));
    });

    private async Task SaveAsync() => await ExecuteAsync(async () =>
    {
        var teamIds = Teams.Where(team => team.IsSelected).Select(team => team.Id).ToArray();
        var saved = await _service.SaveProjectAsync(new DevelopmentProjectInput(
            _projectId, Name, Description, BusinessGoal, FunctionalRequirements,
            NonFunctionalRequirements, ArchitectureStyle, TechnologyStack, Status, IsActive, teamIds));
        StatusMessage = "Proyecto guardado.";
        await NavigateToAsync(typeof(ProjectDetailsPage), saved.Id);
    });
}

public sealed class ProjectDetailsPageViewModel : DevelopmentProjectViewModelBase
{
    private readonly DevelopmentProjectService _service;
    private DevelopmentProjectDetailsDto? _project;
    private string _contextSummary = string.Empty;
    private string? _projectId;

    public DevelopmentProjectDetailsDto? Project { get => _project; private set => SetProperty(ref _project, value); }
    public string ContextSummary { get => _contextSummary; private set => SetProperty(ref _contextSummary, value); }
    public ICommand EditCommand { get; }
    public ICommand TeamsCommand { get; }
    public ICommand ArtifactsCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand PrepareContextCommand { get; }
    public Func<Task>? GoToProjectsAsync { get; set; }
    public Func<Task<bool>>? ConfirmDeleteAsync { get; set; }

    public ProjectDetailsPageViewModel(DevelopmentProjectService service)
    {
        _service = service;
        EditCommand = new Command(async () => await NavigateToAsync(typeof(ProjectCreatePage), Project?.Id));
        TeamsCommand = new Command(async () => await NavigateToAsync(typeof(ProjectTeamsPage), Project?.Id));
        ArtifactsCommand = new Command(async () => await NavigateToAsync(typeof(ProjectArtifactsPage), Project?.Id));
        DeleteCommand = new Command(async () => await DeleteAsync());
        PrepareContextCommand = new Command(async () => await PrepareContextAsync());
    }

    public async Task LoadAsync(string? projectId) => await ExecuteAsync(async () =>
    {
        if (string.IsNullOrWhiteSpace(projectId)) throw new ArgumentException("Falta el identificador del proyecto.");
        _projectId = projectId;
        Project = await _service.GetProjectAsync(projectId);
    });

    private async Task PrepareContextAsync() => await ExecuteAsync(async () =>
    {
        if (Project is null) return;
        var context = await _service.BuildProjectContextAsync(Project.Id);
        ContextSummary = $"Contexto listo: {context.Teams.Count} equipos, {context.Teams.Sum(team => team.Agents.Count)} agentes, {context.Teams.SelectMany(team => team.Agents).Select(agent => agent.ModelId).Distinct().Count()} modelos.";
    });

    private async Task DeleteAsync() => await ExecuteAsync(async () =>
    {
        if (Project is null) return;
        if (ConfirmDeleteAsync is not null && !await ConfirmDeleteAsync()) return;
        await _service.DeleteProjectAsync(Project.Id);
        StatusMessage = "Proyecto eliminado.";
        if (GoToProjectsAsync is not null) await GoToProjectsAsync();
    });

    public async Task RefreshAsync() => await LoadAsync(_projectId);
}

public sealed class ProjectTeamsPageViewModel : DevelopmentProjectViewModelBase
{
    private readonly DevelopmentProjectService _service;
    private string? _projectId;
    private string _projectName = string.Empty;
    public ObservableCollection<ProjectTeamSelectionViewModel> Teams { get; } = new();
    public string ProjectName { get => _projectName; private set => SetProperty(ref _projectName, value); }
    public ICommand SaveCommand { get; }

    public ProjectTeamsPageViewModel(DevelopmentProjectService service)
    {
        _service = service;
        SaveCommand = new Command(async () => await SaveAsync());
    }

    public async Task LoadAsync(string? projectId) => await ExecuteAsync(async () =>
    {
        _projectId = projectId ?? throw new ArgumentException("Falta el identificador del proyecto.");
        var project = await _service.GetProjectAsync(_projectId);
        ProjectName = project.Name;
        Teams.Clear();
        foreach (var team in await _service.GetAvailableTeamsAsync(_projectId)) Teams.Add(new ProjectTeamSelectionViewModel(team));
    });

    private async Task SaveAsync() => await ExecuteAsync(async () =>
    {
        if (_projectId is null) throw new InvalidOperationException("No se ha cargado el proyecto.");
        await _service.AssignTeamsAsync(_projectId, Teams.Where(team => team.IsSelected).Select(team => team.Id));
        StatusMessage = "Equipos asignados.";
    });
}

public sealed class ProjectArtifactsPageViewModel : DevelopmentProjectViewModelBase
{
    private readonly DevelopmentProjectService _service;
    private string? _projectId;
    private string _projectName = string.Empty;
    private string _title = string.Empty;
    private string _description = string.Empty;
    private string _filePath = string.Empty;
    private ArtifactType _artifactType = ArtifactType.Requirement;

    public ObservableCollection<ProjectArtifactDto> Artifacts { get; } = new();
    public IReadOnlyList<ArtifactType> ArtifactTypes { get; } = Enum.GetValues<ArtifactType>();
    public string ProjectName { get => _projectName; private set => SetProperty(ref _projectName, value); }
    public string Title { get => _title; set => SetProperty(ref _title, value); }
    public string Description { get => _description; set => SetProperty(ref _description, value); }
    public string FilePath { get => _filePath; set => SetProperty(ref _filePath, value); }
    public ArtifactType ArtifactType { get => _artifactType; set => SetProperty(ref _artifactType, value); }
    public ICommand CreateCommand { get; }

    public ProjectArtifactsPageViewModel(DevelopmentProjectService service)
    {
        _service = service;
        CreateCommand = new Command(async () => await CreateAsync());
    }

    public async Task LoadAsync(string? projectId) => await ExecuteAsync(async () =>
    {
        _projectId = projectId ?? throw new ArgumentException("Falta el identificador del proyecto.");
        ProjectName = (await _service.GetProjectAsync(_projectId)).Name;
        await RefreshAsync();
    });

    private async Task CreateAsync() => await ExecuteAsync(async () =>
    {
        if (_projectId is null) throw new InvalidOperationException("No se ha cargado el proyecto.");
        await _service.CreateArtifactAsync(new ProjectArtifactInput(_projectId, ArtifactType, Title, Description, FilePath));
        Title = string.Empty;
        Description = string.Empty;
        FilePath = string.Empty;
        await RefreshAsync();
        StatusMessage = "Artefacto registrado.";
    });

    private async Task RefreshAsync()
    {
        Artifacts.Clear();
        foreach (var artifact in await _service.GetArtifactsAsync(_projectId!)) Artifacts.Add(artifact);
    }
}
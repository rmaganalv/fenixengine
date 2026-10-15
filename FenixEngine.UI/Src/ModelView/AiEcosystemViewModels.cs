using System.Collections.ObjectModel;
using System.Windows.Input;
using FenixEngine.AppCore.Src.Models;
using FenixEngine.AppCore.Src.Services;
using FenixEngine.Services.Src.Providers;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.UI.ModelView;

public abstract class AiEcosystemViewModelBase : ViewModelBase
{
    private bool _isBusy;
    private string _statusMessage = string.Empty;

    public bool IsBusy
    {
        get => _isBusy;
        private set => SetProperty(ref _isBusy, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        protected set => SetProperty(ref _statusMessage, value);
    }

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
}

public sealed class ProvidersPageViewModel : AiEcosystemViewModelBase
{
    private readonly AiEcosystemService _service;
    private AiProviderDto? _selectedProvider;
    private string _name = string.Empty;
    private string _providerType = AiProviderTypes.OpenAiCompatible;
    private string _baseUrl = string.Empty;
    private string _apiKey = string.Empty;
    private bool _isLocal;
    private bool _isEnabled = true;
    private bool _clearApiKey;

    public ObservableCollection<AiProviderDto> Providers { get; } = new();
    public IReadOnlyList<string> ProviderTypes { get; } = [AiProviderTypes.OpenAiCompatible, AiProviderTypes.Gemini, AiProviderTypes.Ollama];
    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand TestConnectionCommand { get; }
    public ICommand NewCommand { get; }

    public AiProviderDto? SelectedProvider
    {
        get => _selectedProvider;
        set
        {
            if (!SetProperty(ref _selectedProvider, value) || value is null) return;
            Name = value.Name;
            ProviderType = value.ProviderType;
            BaseUrl = value.BaseUrl;
            IsLocal = value.IsLocal;
            IsEnabled = value.IsEnabled;
            ApiKey = string.Empty;
            ClearApiKey = false;
            ApiKeyConfigured = value.ApiKeyConfigured;
        }
    }

    private bool _apiKeyConfigured;
    public bool ApiKeyConfigured { get => _apiKeyConfigured; private set => SetProperty(ref _apiKeyConfigured, value); }
    public string Name { get => _name; set => SetProperty(ref _name, value); }
    public string ProviderType { get => _providerType; set => SetProperty(ref _providerType, value); }
    public string BaseUrl { get => _baseUrl; set => SetProperty(ref _baseUrl, value); }
    public string ApiKey { get => _apiKey; set => SetProperty(ref _apiKey, value); }
    public bool IsLocal { get => _isLocal; set => SetProperty(ref _isLocal, value); }
    public bool IsEnabled { get => _isEnabled; set => SetProperty(ref _isEnabled, value); }
    public bool ClearApiKey { get => _clearApiKey; set => SetProperty(ref _clearApiKey, value); }

    public ProvidersPageViewModel(AiEcosystemService service)
    {
        _service = service;
        SaveCommand = new Command(async () => await SaveAsync());
        DeleteCommand = new Command(async () => await DeleteAsync());
        TestConnectionCommand = new Command(async () => await TestConnectionAsync());
        NewCommand = new Command(ClearForm);
    }

    public async Task LoadAsync() => await ExecuteAsync(async () =>
    {
        Providers.Clear();
        foreach (var provider in await _service.GetProvidersAsync()) Providers.Add(provider);
        StatusMessage = $"{Providers.Count} proveedores";
    });

    private async Task SaveAsync() => await ExecuteAsync(async () =>
    {
        var saved = await _service.SaveProviderAsync(new AiProviderInput(
            SelectedProvider?.Id, Name, ProviderType, BaseUrl, IsLocal, IsEnabled, ApiKey, ClearApiKey));
        StatusMessage = $"Proveedor guardado: {saved.Name}";
        await RefreshAsync(saved.Id);
    });

    private async Task DeleteAsync() => await ExecuteAsync(async () =>
    {
        if (SelectedProvider is null) throw new InvalidOperationException("Selecciona un proveedor.");
        var id = SelectedProvider.Id;
        await _service.DeleteProviderAsync(id);
        StatusMessage = "Proveedor eliminado.";
        ClearForm();
        await RefreshAsync();
    });

    private async Task TestConnectionAsync() => await ExecuteAsync(async () =>
    {
        if (SelectedProvider is null) throw new InvalidOperationException("Guarda y selecciona un proveedor antes de probarlo.");
        await _service.TestProviderAsync(SelectedProvider.Id);
        StatusMessage = "Conexión correcta.";
    });

    private async Task RefreshAsync(string? selectedId = null)
    {
        var providers = await _service.GetProvidersAsync();
        Providers.Clear();
        foreach (var provider in providers) Providers.Add(provider);
        SelectedProvider = selectedId is null ? null : Providers.FirstOrDefault(provider => provider.Id == selectedId);
    }

    private void ClearForm()
    {
        SelectedProvider = null;
        Name = string.Empty;
        ProviderType = AiProviderTypes.OpenAiCompatible;
        BaseUrl = string.Empty;
        ApiKey = string.Empty;
        IsLocal = false;
        IsEnabled = true;
        ClearApiKey = false;
        ApiKeyConfigured = false;
    }
}

public sealed class ModelsPageViewModel : AiEcosystemViewModelBase
{
    private readonly AiEcosystemService _service;
    private AiProviderDto? _selectedProvider;
    private LlmModelDto? _selectedModel;
    private string _name = string.Empty;
    private string _modelIdentifier = string.Empty;
    private string _contextWindow = string.Empty;
    private bool _isEnabled = true;
    private bool _loadingProviderSelection;

    public ObservableCollection<AiProviderDto> Providers { get; } = new();
    public ObservableCollection<LlmModelDto> Models { get; } = new();
    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand SynchronizeCommand { get; }
    public ICommand NewCommand { get; }

    public AiProviderDto? SelectedProvider
    {
        get => _selectedProvider;
        set
        {
            if (SetProperty(ref _selectedProvider, value) && value is not null && !_loadingProviderSelection)
                _ = LoadModelsAsync(value.Id);
        }
    }

    public LlmModelDto? SelectedModel
    {
        get => _selectedModel;
        set
        {
            if (!SetProperty(ref _selectedModel, value) || value is null) return;
            Name = value.Name;
            ModelIdentifier = value.ModelIdentifier;
            ContextWindow = value.ContextWindow?.ToString() ?? string.Empty;
            IsEnabled = value.IsEnabled;
            SelectedProvider = Providers.FirstOrDefault(provider => provider.Id == value.ProviderId);
        }
    }

    public string Name { get => _name; set => SetProperty(ref _name, value); }
    public string ModelIdentifier { get => _modelIdentifier; set => SetProperty(ref _modelIdentifier, value); }
    public string ContextWindow { get => _contextWindow; set => SetProperty(ref _contextWindow, value); }
    public bool IsEnabled { get => _isEnabled; set => SetProperty(ref _isEnabled, value); }

    public ModelsPageViewModel(AiEcosystemService service)
    {
        _service = service;
        SaveCommand = new Command(async () => await SaveAsync());
        DeleteCommand = new Command(async () => await DeleteAsync());
        SynchronizeCommand = new Command(async () => await SynchronizeAsync());
        NewCommand = new Command(ClearForm);
    }

    public async Task LoadAsync() => await ExecuteAsync(async () =>
    {
        Providers.Clear();
        foreach (var provider in await _service.GetProvidersAsync()) Providers.Add(provider);
        _loadingProviderSelection = true;
        SelectedProvider = Providers.FirstOrDefault();
        _loadingProviderSelection = false;
        if (SelectedProvider is null)
            Models.Clear();
        else
            await PopulateModelsAsync(SelectedProvider.Id);
    });

    private async Task LoadModelsAsync(string providerId) => await ExecuteAsync(async () =>
    {
        await PopulateModelsAsync(providerId);
    });

    private async Task PopulateModelsAsync(string providerId)
    {
        Models.Clear();
        foreach (var model in (await _service.GetModelsAsync()).Where(model => model.ProviderId == providerId)) Models.Add(model);
    }

    private async Task SynchronizeAsync() => await ExecuteAsync(async () =>
    {
        if (SelectedProvider is null) throw new InvalidOperationException("Selecciona un proveedor.");
        var count = await _service.SynchronizeModelsAsync(SelectedProvider.Id);
        await RefreshAsync(SelectedProvider.Id);
        StatusMessage = $"Sincronización terminada: {count} modelos recibidos.";
    });

    private async Task SaveAsync() => await ExecuteAsync(async () =>
    {
        if (SelectedProvider is null) throw new InvalidOperationException("Selecciona un proveedor.");
        if (!string.IsNullOrWhiteSpace(ContextWindow) && !int.TryParse(ContextWindow, out _))
            throw new ArgumentException("La ventana de contexto debe ser un número entero.");
        int? contextWindow = int.TryParse(ContextWindow, out var parsed) ? parsed : null;
        var saved = await _service.SaveModelAsync(new LlmModelInput(
            SelectedModel?.Id, SelectedProvider.Id, Name, ModelIdentifier, contextWindow, IsEnabled));
        StatusMessage = $"Modelo guardado: {saved.Name}";
        await RefreshAsync(SelectedProvider.Id, saved.Id);
    });

    private async Task DeleteAsync() => await ExecuteAsync(async () =>
    {
        if (SelectedModel is null) throw new InvalidOperationException("Selecciona un modelo.");
        var providerId = SelectedModel.ProviderId;
        await _service.DeleteModelAsync(SelectedModel.Id);
        ClearForm();
        await RefreshAsync(providerId);
        StatusMessage = "Modelo eliminado.";
    });

    private async Task RefreshAsync(string providerId, string? selectedModelId = null)
    {
        var models = await _service.GetModelsAsync();
        Models.Clear();
        foreach (var model in models.Where(model => model.ProviderId == providerId)) Models.Add(model);
        SelectedModel = selectedModelId is null ? null : Models.FirstOrDefault(model => model.Id == selectedModelId);
    }

    private void ClearForm()
    {
        SelectedModel = null;
        Name = string.Empty;
        ModelIdentifier = string.Empty;
        ContextWindow = string.Empty;
        IsEnabled = true;
    }
}

public sealed class AgentsPageViewModel : AiEcosystemViewModelBase
{
    private readonly AiEcosystemService _service;
    private AiAgentDto? _selectedAgent;
    private LlmModelDto? _selectedModel;
    private string _name = string.Empty;
    private string _description = string.Empty;
    private string _role = string.Empty;
    private string _systemPrompt = string.Empty;
    private bool _isEnabled = true;

    public ObservableCollection<AiAgentDto> Agents { get; } = new();
    public ObservableCollection<LlmModelDto> Models { get; } = new();
    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand NewCommand { get; }

    public AiAgentDto? SelectedAgent
    {
        get => _selectedAgent;
        set
        {
            if (!SetProperty(ref _selectedAgent, value) || value is null) return;
            Name = value.Name;
            Description = value.Description;
            Role = value.Role;
            SystemPrompt = value.SystemPrompt;
            IsEnabled = value.IsEnabled;
            SelectedModel = Models.FirstOrDefault(model => model.Id == value.ModelId);
        }
    }

    public LlmModelDto? SelectedModel { get => _selectedModel; set => SetProperty(ref _selectedModel, value); }
    public string Name { get => _name; set => SetProperty(ref _name, value); }
    public string Description { get => _description; set => SetProperty(ref _description, value); }
    public string Role { get => _role; set => SetProperty(ref _role, value); }
    public string SystemPrompt { get => _systemPrompt; set => SetProperty(ref _systemPrompt, value); }
    public bool IsEnabled { get => _isEnabled; set => SetProperty(ref _isEnabled, value); }

    public AgentsPageViewModel(AiEcosystemService service)
    {
        _service = service;
        SaveCommand = new Command(async () => await SaveAsync());
        DeleteCommand = new Command(async () => await DeleteAsync());
        NewCommand = new Command(ClearForm);
    }

    public async Task LoadAsync() => await ExecuteAsync(async () =>
    {
        Models.Clear();
        foreach (var model in await _service.GetEnabledModelsAsync()) Models.Add(model);
        Agents.Clear();
        foreach (var agent in await _service.GetAgentsAsync()) Agents.Add(agent);
    });

    private async Task SaveAsync() => await ExecuteAsync(async () =>
    {
        if (SelectedModel is null) throw new InvalidOperationException("Selecciona un modelo habilitado.");
        var saved = await _service.SaveAgentAsync(new AiAgentInput(
            SelectedAgent?.Id, Name, Description, Role, SelectedModel.Id, SystemPrompt, IsEnabled));
        StatusMessage = $"Agente guardado: {saved.Name}";
        await RefreshAsync(saved.Id);
    });

    private async Task DeleteAsync() => await ExecuteAsync(async () =>
    {
        if (SelectedAgent is null) throw new InvalidOperationException("Selecciona un agente.");
        await _service.DeleteAgentAsync(SelectedAgent.Id);
        ClearForm();
        await RefreshAsync();
        StatusMessage = "Agente eliminado.";
    });

    private async Task RefreshAsync(string? selectedId = null)
    {
        Agents.Clear();
        foreach (var agent in await _service.GetAgentsAsync()) Agents.Add(agent);
        SelectedAgent = selectedId is null ? null : Agents.FirstOrDefault(agent => agent.Id == selectedId);
    }

    private void ClearForm()
    {
        SelectedAgent = null;
        Name = string.Empty;
        Description = string.Empty;
        Role = string.Empty;
        SystemPrompt = string.Empty;
        SelectedModel = null;
        IsEnabled = true;
    }
}

public sealed class AgentMembershipOption : ViewModelBase
{
    private bool _isSelected;

    public string Id { get; }
    public string Name { get; }
    public string Role { get; }
    public bool IsSelected { get => _isSelected; set => SetProperty(ref _isSelected, value); }

    public AgentMembershipOption(string id, string name, string role, bool isSelected = false)
    {
        Id = id;
        Name = name;
        Role = role;
        _isSelected = isSelected;
    }
}

public sealed class TeamsPageViewModel : AiEcosystemViewModelBase
{
    private readonly AiEcosystemService _service;
    private AgentTeamDto? _selectedTeam;
    private string _name = string.Empty;
    private string _description = string.Empty;

    public ObservableCollection<AgentTeamDto> Teams { get; } = new();
    public ObservableCollection<AgentMembershipOption> AvailableAgents { get; } = new();
    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand NewCommand { get; }

    public AgentTeamDto? SelectedTeam
    {
        get => _selectedTeam;
        set
        {
            if (!SetProperty(ref _selectedTeam, value) || value is null) return;
            Name = value.Name;
            Description = value.Description;
            var selectedIds = value.AgentIds.ToHashSet(StringComparer.Ordinal);
            foreach (var agent in AvailableAgents) agent.IsSelected = selectedIds.Contains(agent.Id);
        }
    }

    public string Name { get => _name; set => SetProperty(ref _name, value); }
    public string Description { get => _description; set => SetProperty(ref _description, value); }

    public TeamsPageViewModel(AiEcosystemService service)
    {
        _service = service;
        SaveCommand = new Command(async () => await SaveAsync());
        DeleteCommand = new Command(async () => await DeleteAsync());
        NewCommand = new Command(ClearForm);
    }

    public async Task LoadAsync() => await ExecuteAsync(async () =>
    {
        var agents = await _service.GetAgentsAsync();
        AvailableAgents.Clear();
        foreach (var agent in agents)
            AvailableAgents.Add(new AgentMembershipOption(agent.Id, agent.Name, agent.Role));
        await RefreshAsync();
    });

    private async Task SaveAsync() => await ExecuteAsync(async () =>
    {
        var memberIds = AvailableAgents.Where(agent => agent.IsSelected).Select(agent => agent.Id).ToArray();
        var saved = await _service.SaveTeamAsync(new AgentTeamInput(SelectedTeam?.Id, Name, Description, memberIds));
        StatusMessage = $"Equipo guardado: {saved.Name}";
        await RefreshAsync(saved.Id);
    });

    private async Task DeleteAsync() => await ExecuteAsync(async () =>
    {
        if (SelectedTeam is null) throw new InvalidOperationException("Selecciona un equipo.");
        await _service.DeleteTeamAsync(SelectedTeam.Id);
        ClearForm();
        await RefreshAsync();
        StatusMessage = "Equipo eliminado.";
    });

    private async Task RefreshAsync(string? selectedId = null)
    {
        Teams.Clear();
        foreach (var team in await _service.GetTeamsAsync()) Teams.Add(team);
        SelectedTeam = selectedId is null ? null : Teams.FirstOrDefault(team => team.Id == selectedId);
    }

    private void ClearForm()
    {
        SelectedTeam = null;
        Name = string.Empty;
        Description = string.Empty;
        foreach (var agent in AvailableAgents) agent.IsSelected = false;
    }
}
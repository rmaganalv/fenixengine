using System.Collections.ObjectModel;
using FenixEngine.AppCore.Controllers;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.ModelView;

public sealed class WorkStationPageViewModel : ViewModelBase
{
    private readonly ProjectPortfolioController _controller;
    private string _searchText = string.Empty;
    private ProjectSummary? _selectedProject;

    public ObservableCollection<ProjectSummary> Projects { get; } = new();
    public ObservableCollection<ProjectActivity> Activities { get; } = new();

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetProperty(ref _searchText, value))
            {
                RefreshActivities();
            }
        }
    }

    public ProjectSummary? SelectedProject
    {
        get => _selectedProject;
        set
        {
            if (SetProperty(ref _selectedProject, value))
            {
                RefreshActivities();
            }
        }
    }

    public WorkStationPageViewModel(ProjectPortfolioController controller)
    {
        _controller = controller;
        foreach (var project in _controller.GetProjectsForUser())
        {
            Projects.Add(project);
        }

        RefreshActivities();
    }

    private void RefreshActivities()
    {
        var projectName = SelectedProject?.Name ?? "Todos";
        var query = SearchText.Trim();
        var activities = _controller.GetActivitiesForProject(projectName)
            .Where(activity => string.IsNullOrWhiteSpace(query)
                || activity.Title.Contains(query, StringComparison.OrdinalIgnoreCase)
                || activity.ProjectName.Contains(query, StringComparison.OrdinalIgnoreCase));

        Activities.Clear();
        foreach (var activity in activities)
        {
            Activities.Add(activity);
        }
    }
}

using System.Collections.ObjectModel;
using FenixEngine.AppCore.Controllers;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.ModelView;

public sealed class MainPageViewModel : ViewModelBase
{
    private readonly ProjectPortfolioController _controller;

    public ObservableCollection<ProjectSummary> Projects { get; } = new();

    public MainPageViewModel(ProjectPortfolioController controller)
    {
        _controller = controller;
        Load();
    }

    private void Load()
    {
        Projects.Clear();
        foreach (var project in _controller.GetProjectsForUser())
        {
            Projects.Add(project);
        }
    }
}

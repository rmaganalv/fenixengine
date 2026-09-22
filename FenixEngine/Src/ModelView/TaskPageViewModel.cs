using System.Collections.ObjectModel;
using FenixEngine.AppCore.Controllers;

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 Ruben Magaña Alvarado

namespace FenixEngine.ModelView;

public sealed class TaskPageViewModel : ViewModelBase
{
    public ObservableCollection<ProjectTaskGroup> TaskGroups { get; } = new();

    public TaskPageViewModel(ProjectPortfolioController controller)
    {
        var groups = controller.GetProjectTasks()
            .GroupBy(task => task.ProjectName)
            .Select(group => new ProjectTaskGroup(group.Key, group));

        foreach (var group in groups)
        {
            TaskGroups.Add(group);
        }
    }
}

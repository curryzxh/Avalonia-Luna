using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Luna.Desktop.Sample.Models;
using Luna.Desktop.Sample.ViewModels.Samples;

namespace Luna.Desktop.Sample.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IReadOnlyList<SampleNavigationItem> allSamples;

    public MainWindowViewModel()
    {
        allSamples = SampleCatalog.CreateSamples();
        FilteredSampleGroups = [];
        RefreshSamples();
        SelectedSample = FilteredSampleGroups.FirstOrDefault()?.Items.FirstOrDefault();
    }

    public ObservableCollection<SampleGroup> FilteredSampleGroups { get; }

    public string ShellTitle => "Luna Desktop";

    public string ShellSubtitle => "TDesign React 风格的 Avalonia 桌面控件示例工作台";

    public int SampleCount => allSamples.Count;

    public string SearchStatus => string.IsNullOrWhiteSpace(SearchText)
        ? $"共 {SampleCount} 个桌面示例"
        : $"筛选出 {FilteredSampleGroups.Sum(group => group.Items.Count)} 个示例";

    public string SelectedTitle => SelectedSample?.Content.Title ?? "未选择示例";

    public string SelectedCategory => SelectedSample is null
        ? string.Empty
        : $"{SelectedSample.Category.Key} / {SelectedSample.Category.DisplayName}";

    public string SelectedDescription => SelectedSample?.Content.Description ?? "从左侧选择一个 Luna.Desktop 控件示例。";

    public SampleDetailViewModelBase? SelectedContent => SelectedSample?.Content;

    public string? SelectedDocumentationUrl => SelectedSample?.DocumentationUrl;

    [ObservableProperty]
    private SampleNavigationItem? selectedSample;

    [ObservableProperty]
    private string searchText = string.Empty;

    [ObservableProperty]
    private bool isDarkTheme;

    partial void OnSelectedSampleChanged(SampleNavigationItem? value)
    {
        OnPropertyChanged(nameof(SelectedTitle));
        OnPropertyChanged(nameof(SelectedCategory));
        OnPropertyChanged(nameof(SelectedDescription));
        OnPropertyChanged(nameof(SelectedContent));
        OnPropertyChanged(nameof(SelectedDocumentationUrl));
        OpenDocumentationCommand.NotifyCanExecuteChanged();
    }

    partial void OnSearchTextChanged(string value)
    {
        RefreshSamples();
    }

    partial void OnIsDarkThemeChanged(bool value)
    {
        if (Application.Current is not null)
        {
            Application.Current.RequestedThemeVariant = value ? ThemeVariant.Dark : ThemeVariant.Light;
        }
    }

    [RelayCommand]
    private void SelectSample(SampleNavigationItem sample)
    {
        SelectedSample = sample;
    }

    [RelayCommand(CanExecute = nameof(CanOpenDocumentation))]
    private void OpenDocumentation()
    {
        if (string.IsNullOrWhiteSpace(SelectedDocumentationUrl))
        {
            return;
        }

        try
        {
            Process.Start(new ProcessStartInfo(SelectedDocumentationUrl) { UseShellExecute = true });
        }
        catch
        {
            // Documentation links are optional runtime helpers for the sample shell.
        }
    }

    private void RefreshSamples()
    {
        var current = SelectedSample;
        FilteredSampleGroups.Clear();

        var matches = allSamples.Where(MatchesSearch).ToList();
        foreach (var group in matches.GroupBy(sample => sample.Category))
        {
            FilteredSampleGroups.Add(new SampleGroup(group.Key, group));
        }

        SelectedSample = current is not null && matches.Contains(current)
            ? current
            : FilteredSampleGroups.FirstOrDefault()?.Items.FirstOrDefault();

        OnPropertyChanged(nameof(SearchStatus));
    }

    private bool MatchesSearch(SampleNavigationItem sample)
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            return true;
        }

        var keyword = SearchText.Trim();
        return sample.Category.SearchText.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            || sample.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            || sample.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            || (sample.DocumentationUrl?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false);
    }

    private bool CanOpenDocumentation()
    {
        return !string.IsNullOrWhiteSpace(SelectedDocumentationUrl);
    }
}

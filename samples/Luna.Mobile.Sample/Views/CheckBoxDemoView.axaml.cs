using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class CheckBoxDemoView : UserControl
{
    private bool _updating;

        private CheckBoxDemoViewModel ViewModel { get; } = new();

    public event EventHandler? BackRequested
    {
        add => ViewModel.BackRequested += value;
        remove => ViewModel.BackRequested -= value;
    }

    public CheckBoxDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
        UpdateCheckAll();
    }

    private void OnCheckAllChanged(object? sender, RoutedEventArgs e)
    {
        if (_updating)
        {
            return;
        }

        _updating = true;
        var isChecked = CheckAll.IsChecked == true;
        Option1.IsChecked = isChecked;
        Option2.IsChecked = isChecked;
        Option3.IsChecked = isChecked;
        _updating = false;
    }

    private void OnOptionChanged(object? sender, RoutedEventArgs e)
    {
        if (_updating)
        {
            return;
        }

        UpdateCheckAll();
    }

    private void UpdateCheckAll()
    {
        _updating = true;

        var checkedCount = 0;
        if (Option1.IsChecked == true) checkedCount++;
        if (Option2.IsChecked == true) checkedCount++;
        if (Option3.IsChecked == true) checkedCount++;

        CheckAll.IsChecked = checkedCount switch
        {
            0 => false,
            3 => true,
            _ => null,
        };

        _updating = false;
    }
}

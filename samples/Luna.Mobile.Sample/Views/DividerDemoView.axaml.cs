using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class DividerDemoView : UserControl
{
        private DividerDemoViewModel ViewModel { get; } = new();

    public event EventHandler? BackRequested
    {
        add => ViewModel.BackRequested += value;
        remove => ViewModel.BackRequested -= value;
    }

    public DividerDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}


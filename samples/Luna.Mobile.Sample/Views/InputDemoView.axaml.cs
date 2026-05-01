using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class InputDemoView : UserControl
{
        private InputDemoViewModel ViewModel { get; } = new();

    public event EventHandler? BackRequested
    {
        add => ViewModel.BackRequested += value;
        remove => ViewModel.BackRequested -= value;
    }

    public InputDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}


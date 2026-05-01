using Avalonia.Controls;
using System;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class EmptyDemoView : UserControl
{
        private EmptyDemoViewModel ViewModel { get; } = new();

    public event EventHandler? BackRequested
    {
        add => ViewModel.BackRequested += value;
        remove => ViewModel.BackRequested -= value;
    }

    public EmptyDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}


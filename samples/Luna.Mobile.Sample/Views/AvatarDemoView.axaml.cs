using Avalonia.Controls;
using System;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class AvatarDemoView : UserControl
{
        private AvatarDemoViewModel ViewModel { get; } = new();

    public event EventHandler? BackRequested
    {
        add => ViewModel.BackRequested += value;
        remove => ViewModel.BackRequested -= value;
    }

    public AvatarDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}

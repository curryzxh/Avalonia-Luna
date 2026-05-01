using Avalonia.Controls;
using System;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class TagDemoView : UserControl
{
        private TagDemoViewModel ViewModel { get; } = new();

    public event EventHandler? BackRequested
    {
        add => ViewModel.BackRequested += value;
        remove => ViewModel.BackRequested -= value;
    }

    public TagDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}

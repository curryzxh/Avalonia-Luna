using Avalonia.Controls;
using System;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class SearchDemoView : UserControl
{
        private SearchDemoViewModel ViewModel { get; } = new();

    public event EventHandler? BackRequested
    {
        add => ViewModel.BackRequested += value;
        remove => ViewModel.BackRequested -= value;
    }

    public SearchDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}

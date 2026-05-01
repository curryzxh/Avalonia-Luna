using Avalonia.Controls;
using System;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class IndexesDemoView : UserControl
{
        private IndexesDemoViewModel ViewModel { get; } = new();

    public IndexesDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}

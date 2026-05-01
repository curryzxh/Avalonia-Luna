using Avalonia.Controls;
using System;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class CellDemoView : UserControl
{
        private CellDemoViewModel ViewModel { get; } = new();

    public CellDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}

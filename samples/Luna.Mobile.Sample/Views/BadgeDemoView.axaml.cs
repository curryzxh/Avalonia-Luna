using Avalonia.Controls;
using System;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class BadgeDemoView : UserControl
{
        private BadgeDemoViewModel ViewModel { get; } = new();

    public BadgeDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}

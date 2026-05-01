using Avalonia.Controls;
using System;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class EmptyDemoView : UserControl
{
        private EmptyDemoViewModel ViewModel { get; } = new();

    public EmptyDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}


using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class InputDemoView : UserControl
{
        private InputDemoViewModel ViewModel { get; } = new();

    public InputDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}


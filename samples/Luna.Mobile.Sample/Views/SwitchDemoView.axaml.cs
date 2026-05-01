using Avalonia.Controls;
using System;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class SwitchDemoView : UserControl
{
        private SwitchDemoViewModel ViewModel { get; } = new();

    public SwitchDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}

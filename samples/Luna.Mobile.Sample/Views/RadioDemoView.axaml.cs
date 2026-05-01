using Avalonia.Controls;
using System;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class RadioDemoView : UserControl
{
        private RadioDemoViewModel ViewModel { get; } = new();

    public RadioDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}

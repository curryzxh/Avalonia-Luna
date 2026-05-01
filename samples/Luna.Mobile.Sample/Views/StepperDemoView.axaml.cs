using Avalonia.Controls;
using System;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class StepperDemoView : UserControl
{
        private StepperDemoViewModel ViewModel { get; } = new();

    public StepperDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}

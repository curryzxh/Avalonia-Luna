using Avalonia.Controls;
using Luna.Mobile.Sample.ViewModels;
using System;

namespace Luna.Mobile.Sample.Views;

public partial class RateDemoView : UserControl
{
    private RateDemoViewModel ViewModel { get; } = new();

    public RateDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}

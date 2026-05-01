using Avalonia.Controls;
using Luna.Mobile.Sample.ViewModels;
using System;

namespace Luna.Mobile.Sample.Views;

public partial class ToastDemoView : UserControl
{
    private ToastDemoViewModel ViewModel { get; } = new();

    public ToastDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}

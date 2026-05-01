using Avalonia.Controls;
using System;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class AvatarDemoView : UserControl
{
        private AvatarDemoViewModel ViewModel { get; } = new();

    public AvatarDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}

using Avalonia.Controls;
using System;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class TagDemoView : UserControl
{
        private TagDemoViewModel ViewModel { get; } = new();

    public TagDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}

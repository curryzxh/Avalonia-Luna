using Avalonia.Controls;
using Luna.Mobile.Sample.ViewModels;
using System;

namespace Luna.Mobile.Sample.Views;

public partial class MessageDemoView : UserControl
{
    private MessageDemoViewModel ViewModel { get; } = new();

    public MessageDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}

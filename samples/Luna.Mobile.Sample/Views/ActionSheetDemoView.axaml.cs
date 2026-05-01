using Avalonia.Controls;
using Luna.Mobile.Controls;
using System;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class ActionSheetDemoView : UserControl
{
    private ActionSheetDemoViewModel ViewModel { get; } = new();

    public ActionSheetDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
        ViewModel.ActionSheetRequested += OnActionSheetRequested;
    }

    private void OnActionSheetRequested(ActionSheetRequest request)
    {
        if (request.UseStaticApi)
        {
            ActionSheet.Show(request.Options);
            return;
        }

        ActionSheetHost.Show(request.Options);
    }
}

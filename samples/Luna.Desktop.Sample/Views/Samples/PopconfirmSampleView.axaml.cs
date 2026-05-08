using Avalonia.Controls;

namespace Luna.Desktop.Sample.Views.Samples;

public partial class PopconfirmSampleView : UserControl
{
    public PopconfirmSampleView()
    {
        InitializeComponent();
    }

    private void OnConfirmed(object? sender, System.EventArgs e)
    {
        if (DataContext is ViewModels.Samples.PopconfirmSampleViewModel vm)
            vm.OnConfirmedCommand.Execute(null);
    }

    private void OnCanceled(object? sender, System.EventArgs e)
    {
        if (DataContext is ViewModels.Samples.PopconfirmSampleViewModel vm)
            vm.OnCanceledCommand.Execute(null);
    }
}

using CommunityToolkit.Mvvm.Input;
using System;

namespace Luna.Mobile.Sample.ViewModels;

public abstract partial class DemoViewModelBase : ViewModelBase
{
    public event EventHandler? BackRequested;

    [RelayCommand]
    private void Back()
    {
        BackRequested?.Invoke(this, EventArgs.Empty);
    }
}

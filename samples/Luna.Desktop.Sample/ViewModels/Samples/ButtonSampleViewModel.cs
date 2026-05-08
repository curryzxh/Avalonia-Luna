using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class ButtonSampleViewModel()
    : SampleDetailViewModelBase("基础", "Button", "复刻 TDesign Button 的 theme、variant、size、shape、block、loading 和 disabled 示例。")
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ResetCommand))]
    private int clickCount;

    [ObservableProperty]
    private bool isLoading;

    public string StatusText => ClickCount == 0 ? "等待操作" : $"已执行 {ClickCount} 次";

    [RelayCommand]
    private void RunPrimary()
    {
        ClickCount++;
        OnPropertyChanged(nameof(StatusText));
    }

    [RelayCommand]
    private void ToggleLoading()
    {
        IsLoading = !IsLoading;
    }

    [RelayCommand(CanExecute = nameof(CanReset))]
    private void Reset()
    {
        ClickCount = 0;
        IsLoading = false;
        OnPropertyChanged(nameof(StatusText));
    }

    private bool CanReset()
    {
        return ClickCount > 0;
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class InputSampleViewModel()
    : SampleDetailViewModelBase("输入", "TextBox / Input", "复刻 TDesign Input 的默认、密码、禁用、错误状态和字数提示。")
{
    [ObservableProperty]
    private string account = "luna.desktop";

    [ObservableProperty]
    private string notes = "TDesign Input 强调清晰边框、焦点反馈、状态提示和可读的占位文案。";

    [ObservableProperty]
    private string clearableText = "点击右侧按钮清除";

    [ObservableProperty]
    private string prefixText = string.Empty;

    [ObservableProperty]
    private string suffixText = string.Empty;

    [ObservableProperty]
    private string errorInput = "error content";

    [ObservableProperty]
    private string warningInput = string.Empty;

    [ObservableProperty]
    private string successInput = "ok";

    [ObservableProperty]
    private string maxLengthText = "有限长度文本";

    [ObservableProperty]
    private int maxLength = 20;

    public int NotesLength => Notes.Length;

    public int MaxLengthTextLength => MaxLengthText.Length;

    partial void OnNotesChanged(string value)
    {
        OnPropertyChanged(nameof(NotesLength));
    }

    partial void OnMaxLengthTextChanged(string value)
    {
        OnPropertyChanged(nameof(MaxLengthTextLength));
    }

    [RelayCommand]
    private void ClearInput()
    {
        ClearableText = string.Empty;
    }
}

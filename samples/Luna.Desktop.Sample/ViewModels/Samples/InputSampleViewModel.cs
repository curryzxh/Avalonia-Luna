using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class InputSampleViewModel()
    : SampleDetailViewModelBase("输入", "TextBox / Input", "复刻 TDesign Input 的默认、密码、禁用、错误状态和字数提示。")
{
    [ObservableProperty]
    private string account = "luna.desktop";

    [ObservableProperty]
    private string notes = "TDesign Input 强调清晰边框、焦点反馈、状态提示和可读的占位文案。";

    public int NotesLength => Notes.Length;

    partial void OnNotesChanged(string value)
    {
        OnPropertyChanged(nameof(NotesLength));
    }
}

using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class TextareaSampleViewModel()
    : SampleDetailViewModelBase("输入", "Textarea", "多行文本输入，支持自适应高度、字数限制和多种状态。")
{
    [ObservableProperty]
    private string content = "TDesign Textarea 强调可调整大小的多行输入、清晰的焦点状态和字数统计。";

    [ObservableProperty]
    private string errorText = "输入内容不能为空";

    [ObservableProperty]
    private int maxLength = 200;

    public int ContentLength => Content.Length;

    partial void OnContentChanged(string value)
    {
        OnPropertyChanged(nameof(ContentLength));
    }
}

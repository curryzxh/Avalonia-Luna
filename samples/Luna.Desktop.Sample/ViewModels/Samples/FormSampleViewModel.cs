using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public sealed partial class FormSampleViewModel()
    : SampleDetailViewModelBase("输入", "Form", "表单布局控件，使用 FormItem 管理标签、必填标记和错误信息。")
{
    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _phone = string.Empty;

    [ObservableProperty]
    private string _remark = string.Empty;

    [ObservableProperty]
    private string _nameError = string.Empty;

    [ObservableProperty]
    private string _emailError = string.Empty;

    [ObservableProperty]
    private string _phoneError = string.Empty;

    [ObservableProperty]
    private bool _verticalLayout;

    [RelayCommand]
    private void Submit()
    {
        var hasError = false;
        if (string.IsNullOrWhiteSpace(Name))
        {
            NameError = "请输入姓名";
            hasError = true;
        }
        else
        {
            NameError = string.Empty;
        }

        if (string.IsNullOrWhiteSpace(Email))
        {
            EmailError = "请输入邮箱";
            hasError = true;
        }
        else if (!Email.Contains('@'))
        {
            EmailError = "邮箱格式不正确";
            hasError = true;
        }
        else
        {
            EmailError = string.Empty;
        }

        if (string.IsNullOrWhiteSpace(Phone))
        {
            PhoneError = "请输入手机号";
            hasError = true;
        }
        else
        {
            PhoneError = string.Empty;
        }

        if (!hasError)
        {
            NameError = string.Empty;
            EmailError = string.Empty;
            PhoneError = string.Empty;
        }
    }

    [RelayCommand]
    private void Reset()
    {
        Name = string.Empty;
        Email = string.Empty;
        Phone = string.Empty;
        Remark = string.Empty;
        NameError = string.Empty;
        EmailError = string.Empty;
        PhoneError = string.Empty;
    }
}

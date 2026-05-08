using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class UploadSampleViewModel : SampleDetailViewModelBase
{
    public UploadSampleViewModel() : base("输入", "Upload", "文件上传组件，支持拖拽上传、列表管理和进度显示。")
    {
    }

    [ObservableProperty]
    private ObservableCollection<UploadFileInfo> _files = [];

    [ObservableProperty]
    private bool _isDragOver;

    [RelayCommand]
    private void AddFile()
    {
        Files.Add(new UploadFileInfo($"文件_{Files.Count + 1}.pdf", 1024 * Random.Shared.Next(100, 5000), 100));
    }

    [RelayCommand]
    private void RemoveFile(UploadFileInfo file)
    {
        Files.Remove(file);
    }
}

public class UploadFileInfo(string name, long size, double progress)
{
    public string Name { get; } = name;
    public long Size { get; } = size;
    public string SizeDisplay => Size < 1024 * 1024 ? $"{Size / 1024.0:F1} KB" : $"{Size / 1024.0 / 1024.0:F1} MB";
    public double Progress { get; } = progress;
}

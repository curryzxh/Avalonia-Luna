namespace Luna.Desktop.Sample.ViewModels.Samples;

public abstract class SampleDetailViewModelBase(string category, string title, string description) : ViewModelBase
{
    public string Category { get; } = category;

    public string Title { get; } = title;

    public string Description { get; } = description;
}

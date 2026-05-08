using Luna.Desktop.Sample.ViewModels.Samples;

namespace Luna.Desktop.Sample.Models;

public sealed record SampleNavigationItem(
    ControlCategory Category,
    SampleDetailViewModelBase Content,
    string? DocumentationUrl)
{
    public string Title => Content.Title;

    public string Description => Content.Description;
}

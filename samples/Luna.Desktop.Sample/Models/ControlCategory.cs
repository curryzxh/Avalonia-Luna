namespace Luna.Desktop.Sample.Models;

public sealed record ControlCategory(string Key, string DisplayName, string Description)
{
    public string SearchText => $"{Key} {DisplayName} {Description}";
}

using System.Collections.Generic;
using System.Linq;

namespace Luna.Desktop.Sample.Models;

public sealed class SampleGroup
{
    public SampleGroup(ControlCategory category, IEnumerable<SampleNavigationItem> items)
    {
        Category = category;
        Items = items.ToList();
    }

    public ControlCategory Category { get; }

    public IReadOnlyList<SampleNavigationItem> Items { get; }

    public string Key => Category.Key;

    public string Title => Category.DisplayName;

    public string Description => Category.Description;
}

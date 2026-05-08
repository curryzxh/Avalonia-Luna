using System.Collections.Generic;

namespace Luna.Desktop.Sample.Models;

public static class SampleCatalog
{
    public static IReadOnlyList<SampleNavigationItem> CreateSamples()
    {
        return ControlSampleCatalog.CreateSamples();
    }
}

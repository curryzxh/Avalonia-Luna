using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Luna.Mobile.Sample.Views;

public partial class CollapseDemoView : UserControl
{
    public CollapseDemoView()
    {
        InitializeComponent();
    }

    private void AccordionExpander_OnExpanded(object? sender, RoutedEventArgs e)
    {
        if (sender is not Expander expanded)
        {
            return;
        }

        foreach (var expander in new[] { AccordionExpander1, AccordionExpander2, AccordionExpander3 })
        {
            if (!ReferenceEquals(expander, expanded))
            {
                expander.IsExpanded = false;
            }
        }
    }
}

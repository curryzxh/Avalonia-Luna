using System.Collections;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Media;
using Avalonia.VisualTree;

namespace Luna.Desktop.Controls;

public class Anchor : ItemsControl
{
    public static readonly StyledProperty<ScrollViewer?> TargetContainerProperty =
        AvaloniaProperty.Register<Anchor, ScrollViewer?>(nameof(TargetContainer));

    public static readonly AttachedProperty<string?> AnchorIdProperty =
        AvaloniaProperty.RegisterAttached<Anchor, Visual, string?>("AnchorId");

    public static readonly StyledProperty<double> TopOffsetProperty =
        AvaloniaProperty.Register<Anchor, double>(nameof(TopOffset), 0);

    private List<(string Id, double Position)> _positions = [];
    private AnchorItem? _selectedContainer;
    private bool _scrollingFromSelection;

    public ScrollViewer? TargetContainer
    {
        get => GetValue(TargetContainerProperty);
        set => SetValue(TargetContainerProperty, value);
    }

    public double TopOffset
    {
        get => GetValue(TopOffsetProperty);
        set => SetValue(TopOffsetProperty, value);
    }

    public static void SetAnchorId(Visual obj, string? value)
    {
        obj.SetValue(AnchorIdProperty, value);
    }

    public static string? GetAnchorId(Visual obj)
    {
        return obj.GetValue(AnchorIdProperty);
    }

    static Anchor()
    {
        ItemsPanelProperty.OverrideDefaultValue<Anchor>(
            new FuncTemplate<Panel?>(() => new StackPanel { Spacing = 4 }));
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<AnchorItem>(item, out recycleKey);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new AnchorItem();
    }

    protected override void PrepareContainerForItemOverride(Control container, object? item, int index)
    {
        base.PrepareContainerForItemOverride(container, item, index);
        if (container is AnchorItem anchorItem)
        {
            if (item is IDictionary dict)
            {
                anchorItem.Text = dict["Title"]?.ToString();
                anchorItem.AnchorId = dict["AnchorId"]?.ToString();
            }
        }
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        if (TargetContainer != null)
        {
            TargetContainer.AddHandler(ScrollViewer.ScrollChangedEvent, OnScrollChanged);
        }
        InvalidatePositions();
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        base.OnUnloaded(e);
        if (TargetContainer != null)
        {
            TargetContainer.RemoveHandler(ScrollViewer.ScrollChangedEvent, OnScrollChanged);
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        var source = e.Source as Visual;
        while (source != null)
        {
            if (source is AnchorItem item)
            {
                MarkSelectedContainer(item);
                if (TargetContainer != null && !string.IsNullOrEmpty(item.AnchorId))
                {
                    var target = TargetContainer.GetVisualDescendants()
                        .FirstOrDefault(a => GetAnchorId(a) == item.AnchorId);
                    if (target != null)
                    {
                        ScrollToAnchor(target);
                    }
                }
                break;
            }
            source = source.GetVisualParent();
        }
    }

    public void InvalidatePositions()
    {
        if (TargetContainer == null) return;

        var items = TargetContainer.GetVisualDescendants()
            .Where(a => GetAnchorId(a) != null);

        var positions = new List<(string, double)>();
        foreach (var visual in items)
        {
            var anchorId = GetAnchorId(visual);
            if (anchorId == null) continue;
            var transform = visual.TransformToVisual(TargetContainer);
            if (transform.HasValue)
            {
                var pos = transform.Value.Transform(new Point(0, 0));
                positions.Add((anchorId, pos.Y + TargetContainer.Offset.Y));
            }
        }

        positions.Sort((a, b) => a.Item2.CompareTo(b.Item2));
        _positions = positions;
        MarkSelectedContainerByPosition();
    }

    private void OnScrollChanged(object? sender, ScrollChangedEventArgs e)
    {
        if (_scrollingFromSelection) return;
        InvalidatePositions();
        MarkSelectedContainerByPosition();
    }

    private void ScrollToAnchor(Visual target)
    {
        if (TargetContainer == null) return;

        var targetPosition = target.TransformToVisual(TargetContainer);
        if (!targetPosition.HasValue) return;

        var pos = targetPosition.Value.Transform(new Point(0, 0));
        var to = TargetContainer.Offset.Y + pos.Y - TopOffset;

        if (to < 0) to = 0;
        if (to > TargetContainer.Extent.Height - TargetContainer.Viewport.Height)
            to = TargetContainer.Extent.Height - TargetContainer.Viewport.Height;

        _scrollingFromSelection = true;
        TargetContainer.Offset = new Vector(0, to);

        Task.Run(async () =>
        {
            await Task.Delay(300);
            _scrollingFromSelection = false;
        });
    }

    internal void MarkSelectedContainer(AnchorItem? item)
    {
        if (_selectedContainer == item) return;
        _selectedContainer?.SetValue(AnchorItem.IsSelectedProperty, false);
        _selectedContainer = item;
        _selectedContainer?.SetValue(AnchorItem.IsSelectedProperty, true);
    }

    private void MarkSelectedContainerByPosition()
    {
        if (TargetContainer == null || _positions.Count == 0) return;

        var top = TargetContainer.Offset.Y + TopOffset;
        var match = _positions.LastOrDefault(p => p.Position <= top);
        if (match.Id == null) return;

        var items = this.GetVisualDescendants().OfType<AnchorItem>();
        var found = items.FirstOrDefault(a => a.AnchorId == match.Id);
        if (found != null)
        {
            MarkSelectedContainer(found);
        }
    }
}

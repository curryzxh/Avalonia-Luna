using System.Linq;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Styling;
using Avalonia.VisualTree;

namespace Luna.Desktop.Controls;

public class Anchor : ItemsControl
{
    public static readonly StyledProperty<ScrollViewer?> TargetContainerProperty =
        AvaloniaProperty.Register<Anchor, ScrollViewer?>(nameof(TargetContainer));

    public static readonly AttachedProperty<string?> AnchorIdProperty =
        AvaloniaProperty.RegisterAttached<Anchor, Visual, string?>("AnchorId");

    public static readonly StyledProperty<double> TopOffsetProperty =
        AvaloniaProperty.Register<Anchor, double>(nameof(TopOffset));

    private CancellationTokenSource _cts = new();
    private List<(string Id, double Position)> _positions = [];
    private bool _scrollingFromSelection;
    private AnchorItem? _selectedContainer;

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

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<AnchorItem>(item, out recycleKey);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new AnchorItem();
    }

    public void InvalidatePositions()
    {
        InvalidateAnchorPositions();
        MarkSelectedContainerByPosition();
    }

    internal void InvalidateAnchorPositions()
    {
        if (TargetContainer is null) return;
        var items = TargetContainer.GetVisualDescendants()
            .Where(a => GetAnchorId(a) is not null);
        var positions = new List<(string Id, double Position)>();
        foreach (var item in items)
        {
            var anchorId = GetAnchorId(item);
            if (anchorId is null) continue;
            var transform = item.TransformToVisual(TargetContainer);
            if (transform.HasValue)
            {
                var position = transform.Value.M32 + TargetContainer.Offset.Y;
                positions.Add((anchorId, position));
            }
        }

        positions.Sort((a, b) => a.Position.CompareTo(b.Position));
        _positions = positions;
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        if (TargetContainer is null) return;
        TargetContainer.AddHandler(ScrollViewer.ScrollChangedEvent, OnScrollChanged);
        TargetContainer.AddHandler(LoadedEvent, OnTargetContainerLoaded);
        if (TargetContainer.IsLoaded)
            InvalidateAnchorPositions();
        MarkSelectedContainerByPosition();
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        base.OnUnloaded(e);
        if (TargetContainer is null) return;
        TargetContainer.RemoveHandler(LoadedEvent, OnTargetContainerLoaded);
        TargetContainer.RemoveHandler(ScrollViewer.ScrollChangedEvent, OnScrollChanged);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        var source = e.Source as Visual;
        var container = FindContainerFromSource(source);
        if (container is null) return;
        MarkSelectedContainer(container);
        var target = TargetContainer?.GetVisualDescendants()
            .FirstOrDefault(a => GetAnchorId(a) == container.AnchorId);
        if (target is null) return;
        ScrollToAnchor(target);
    }

    internal Control CreateContainerForItemOverrideInternal(object? item, int index, object? recycleKey)
    {
        return CreateContainerForItemOverride(item, index, recycleKey);
    }

    internal bool NeedsContainerOverrideInternal(object? item, int index, out object? recycleKey)
    {
        return NeedsContainerOverride(item, index, out recycleKey);
    }

    internal void PrepareContainerForItemOverrideInternal(Control container, object? item, int index)
    {
        PrepareContainerForItemOverride(container, item, index);
    }

    internal void ContainerForItemPreparedOverrideInternal(Control container, object? item, int index)
    {
        ContainerForItemPreparedOverride(container, item, index);
    }

    internal void MarkSelectedContainer(AnchorItem? item)
    {
        if (_selectedContainer == item) return;
        _selectedContainer?.SetValue(AnchorItem.IsSelectedProperty, false);
        _selectedContainer = item;
        _selectedContainer?.SetValue(AnchorItem.IsSelectedProperty, true);
    }

    private void ScrollToAnchor(Visual target)
    {
        if (TargetContainer is null) return;
        var targetPosition = target.TranslatePoint(new Point(0, 0), TargetContainer);
        if (!targetPosition.HasValue) return;

        var from = TargetContainer.Offset.Y;
        var to = from + targetPosition.Value.Y - TopOffset;
        var maxOffset = TargetContainer.Extent.Height - TargetContainer.Bounds.Height;
        if (to > maxOffset) to = maxOffset;
        if (to < 0) to = 0;
        if (Math.Abs(from - to) < 0.5) return;

        var animation = new Animation
        {
            Duration = TimeSpan.FromSeconds(0.3),
            Easing = new QuadraticEaseOut(),
            Children =
            {
                new KeyFrame
                {
                    Setters = { new Setter(ScrollViewer.OffsetProperty, new Vector(0, from)) },
                    Cue = new Cue(0.0)
                },
                new KeyFrame
                {
                    Setters = { new Setter(ScrollViewer.OffsetProperty, new Vector(0, to)) },
                    Cue = new Cue(1.0)
                }
            }
        };
        _cts.Cancel();
        _cts.Dispose();
        _cts = new CancellationTokenSource();
        var token = _cts.Token;
        _scrollingFromSelection = true;
        animation.RunAsync(TargetContainer, token).ContinueWith(_ => _scrollingFromSelection = false, token);
    }

    private void MarkSelectedContainerByPosition()
    {
        if (TargetContainer is null) return;
        var top = TargetContainer.Offset.Y + TopOffset;
        var match = _positions.LastOrDefault(p => p.Position <= top);
        if (match.Id is null) return;
        var item = this.GetVisualDescendants().OfType<AnchorItem>()
            .FirstOrDefault(a => a.AnchorId == match.Id);
        if (item is null) return;
        MarkSelectedContainer(item);
    }

    private void OnScrollChanged(object? sender, ScrollChangedEventArgs e)
    {
        if (_scrollingFromSelection) return;
        MarkSelectedContainerByPosition();
    }

    private void OnTargetContainerLoaded(object? sender, RoutedEventArgs e)
    {
        InvalidateAnchorPositions();
    }

    private AnchorItem? FindContainerFromSource(Visual? source)
    {
        if (source is null) return null;
        var current = source;
        while (current is not null)
        {
            if (current is AnchorItem item)
                return item;
            current = current.GetVisualParent();
        }
        return null;
    }
}

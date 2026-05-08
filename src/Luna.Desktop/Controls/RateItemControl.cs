using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Input;
using Avalonia.VisualTree;

namespace Luna.Desktop.Controls;

[PseudoClasses(":filled")]
public class RateItemControl : ContentControl
{
    private Rate? _owner;
    private RateItem? _item;

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _owner = this.FindAncestorOfType<Rate>();
        SubscribeItem();
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        UnsubscribeItem();
        _owner = null;
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        UnsubscribeItem();
        SubscribeItem();
    }

    private void SubscribeItem()
    {
        UnsubscribeItem();
        if (DataContext is RateItem item)
        {
            _item = item;
            PseudoClasses.Set(":filled", item.IsFilled);
            item.PropertyChanged += OnItemPropertyChanged;
        }
    }

    private void UnsubscribeItem()
    {
        if (_item != null)
        {
            _item.PropertyChanged -= OnItemPropertyChanged;
            _item = null;
        }
    }

    private void OnItemPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == RateItem.IsFilledProperty && _item != null)
        {
            PseudoClasses.Set(":filled", _item.IsFilled);
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (_item != null)
        {
            _owner?.OnItemPointerPressed(_item.Index);
        }
    }

    protected override void OnPointerEntered(PointerEventArgs e)
    {
        base.OnPointerEntered(e);
        if (_item != null)
        {
            _owner?.OnItemPointerEntered(_item.Index);
        }
    }

    protected override void OnPointerExited(PointerEventArgs e)
    {
        base.OnPointerExited(e);
        _owner?.OnItemPointerExited();
    }
}

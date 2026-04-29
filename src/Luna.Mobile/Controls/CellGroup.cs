using Avalonia;
using Avalonia.Controls;

namespace Luna.Mobile.Controls;

/// <summary>
/// 单元格分组容器，可选标题与概要说明。
/// </summary>
public sealed class CellGroup : ContentControl
{
    private bool _showTitle;
    private bool _showSummary;

    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<CellGroup, string?>(nameof(Title));

    public static readonly StyledProperty<string?> SummaryProperty =
        AvaloniaProperty.Register<CellGroup, string?>(nameof(Summary));

    public static readonly DirectProperty<CellGroup, bool> ShowTitleProperty =
        AvaloniaProperty.RegisterDirect<CellGroup, bool>(
            nameof(ShowTitle),
            o => o.ShowTitle);

    public static readonly DirectProperty<CellGroup, bool> ShowSummaryProperty =
        AvaloniaProperty.RegisterDirect<CellGroup, bool>(
            nameof(ShowSummary),
            o => o.ShowSummary);

    static CellGroup()
    {
        TitleProperty.Changed.AddClassHandler<CellGroup>((control, _) => control.UpdateFlags());
        SummaryProperty.Changed.AddClassHandler<CellGroup>((control, _) => control.UpdateFlags());
    }

    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string? Summary
    {
        get => GetValue(SummaryProperty);
        set => SetValue(SummaryProperty, value);
    }

    public bool ShowTitle
    {
        get => _showTitle;
        private set => SetAndRaise(ShowTitleProperty, ref _showTitle, value);
    }

    public bool ShowSummary
    {
        get => _showSummary;
        private set => SetAndRaise(ShowSummaryProperty, ref _showSummary, value);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        UpdateFlags();
    }

    private void UpdateFlags()
    {
        ShowTitle = !string.IsNullOrWhiteSpace(Title);
        ShowSummary = !string.IsNullOrWhiteSpace(Summary);

        PseudoClasses.Set(":has-title", ShowTitle);
        PseudoClasses.Set(":has-summary", ShowSummary);
    }
}

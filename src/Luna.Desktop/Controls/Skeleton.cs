using Avalonia;

namespace Luna.Desktop.Controls;

public enum SkeletonTheme
{
    Text,
    Avatar,
    Paragraph,
}

public class Skeleton : Avalonia.Controls.ContentControl
{
    public static readonly StyledProperty<bool> LoadingProperty =
        AvaloniaProperty.Register<Skeleton, bool>(nameof(Loading), true);

    public static readonly StyledProperty<SkeletonTheme> ThemeProperty =
        AvaloniaProperty.Register<Skeleton, SkeletonTheme>(nameof(Theme), SkeletonTheme.Text);

    public static readonly StyledProperty<double> RowSpacingProperty =
        AvaloniaProperty.Register<Skeleton, double>(nameof(RowSpacing), 16d);

    public bool Loading
    {
        get => GetValue(LoadingProperty);
        set => SetValue(LoadingProperty, value);
    }

    public SkeletonTheme Theme
    {
        get => GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    public double RowSpacing
    {
        get => GetValue(RowSpacingProperty);
        set => SetValue(RowSpacingProperty, value);
    }

    static Skeleton()
    {
        LoadingProperty.Changed.AddClassHandler<Skeleton>((c, _) => c.UpdateState());
        ThemeProperty.Changed.AddClassHandler<Skeleton>((c, _) => c.UpdateState());
    }

    public Skeleton()
    {
        UpdateState();
    }

    private void UpdateState()
    {
        PseudoClasses.Set(":loading", Loading);
        PseudoClasses.Set(":text", Theme == SkeletonTheme.Text);
        PseudoClasses.Set(":avatar", Theme == SkeletonTheme.Avatar);
        PseudoClasses.Set(":paragraph", Theme == SkeletonTheme.Paragraph);
    }
}

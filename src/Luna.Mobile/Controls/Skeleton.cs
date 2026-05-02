using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using System;
using System.Collections.Generic;

namespace Luna.Mobile.Controls;

/// <summary>
/// 骨架屏预设类型。
/// </summary>
public enum SkeletonTheme
{
    Text,
    Avatar,
    Image,
    Paragraph,
    Custom,
}

/// <summary>
/// 骨架屏动效类型。
/// </summary>
public enum SkeletonAnimation
{
    Gradient,
    Flashed,
    None,
}

/// <summary>
/// 骨架块形状。
/// </summary>
public enum SkeletonShape
{
    Rect,
    Round,
    Circle,
}

/// <summary>
/// 单个骨架块定义。
/// </summary>
public sealed class SkeletonBlockDefinition
{
    /// <summary>
    /// 获取或设置骨架块宽度；未设置时由布局自动分配。
    /// </summary>
    public double Width { get; init; } = double.NaN;

    /// <summary>
    /// 获取或设置骨架块宽度占可用空间的比例，取值范围为 0 到 1。
    /// </summary>
    public double WidthFactor { get; init; }

    /// <summary>
    /// 获取或设置骨架块高度。
    /// </summary>
    public double Height { get; init; } = 16d;

    /// <summary>
    /// 获取或设置骨架块形状。
    /// </summary>
    public SkeletonShape Shape { get; init; } = SkeletonShape.Round;

    /// <summary>
    /// 获取或设置骨架块圆角。
    /// </summary>
    public CornerRadius CornerRadius { get; init; }
}

/// <summary>
/// 单行骨架布局定义。
/// </summary>
public sealed class SkeletonRowDefinition
{
    /// <summary>
    /// 获取或设置当前行的左侧缩进。
    /// </summary>
    public double Indent { get; init; }

    /// <summary>
    /// 获取或设置当前行内骨架块之间的水平间距。
    /// </summary>
    public double Spacing { get; init; } = 10d;

    /// <summary>
    /// 获取或设置当前行包含的骨架块集合。
    /// </summary>
    public IReadOnlyList<SkeletonBlockDefinition> Blocks { get; init; } = Array.Empty<SkeletonBlockDefinition>();
}

/// <summary>
/// 模板层使用的骨架块布局数据。
/// </summary>
public sealed class SkeletonBlockLayout
{
    public SkeletonBlockLayout(double width, double height, CornerRadius cornerRadius, Thickness margin)
    {
        Width = width;
        Height = height;
        CornerRadius = cornerRadius;
        Margin = margin;
    }

    public double Width { get; }

    public double Height { get; }

    public CornerRadius CornerRadius { get; }

    public Thickness Margin { get; }
}

/// <summary>
/// 模板层使用的骨架行布局数据。
/// </summary>
public sealed class SkeletonRowLayout
{
    public SkeletonRowLayout(Thickness margin, Thickness padding, IReadOnlyList<SkeletonBlockLayout> blocks)
    {
        Margin = margin;
        Padding = padding;
        Blocks = blocks;
    }

    public Thickness Margin { get; }

    public Thickness Padding { get; }

    public IReadOnlyList<SkeletonBlockLayout> Blocks { get; }
}

/// <summary>
/// 骨架屏控件，用于在内容加载前展示占位结构。
/// </summary>
public sealed class Skeleton : ContentControl
{
    private const double DefaultFallbackWidth = 240d;
    private const double DefaultAnimationDurationMilliseconds = 1200d;

    private bool _showSkeleton = true;
    private bool _showContent;
    private bool _showGradientAnimation = true;
    private bool _showFlashedAnimation;
    private IReadOnlyList<SkeletonRowLayout> _resolvedRows = Array.Empty<SkeletonRowLayout>();
    private double _gradientLeadOffset = -0.4d;
    private double _gradientCenterOffset = -0.2d;
    private double _gradientTrailOffset = 0d;
    private double _flashedOpacity = 1d;
    private DispatcherTimer? _animationTimer;
    private DateTimeOffset _animationStartedAt;
    private double _lastResolvedWidth = double.NaN;

    public static readonly StyledProperty<bool> LoadingProperty =
        AvaloniaProperty.Register<Skeleton, bool>(nameof(Loading), true);

    public static readonly new StyledProperty<SkeletonTheme> ThemeProperty =
        AvaloniaProperty.Register<Skeleton, SkeletonTheme>(nameof(Theme), SkeletonTheme.Text);

    public static readonly StyledProperty<SkeletonAnimation> AnimationProperty =
        AvaloniaProperty.Register<Skeleton, SkeletonAnimation>(nameof(Animation), SkeletonAnimation.Gradient);

    public static readonly StyledProperty<IReadOnlyList<SkeletonRowDefinition>?> RowsProperty =
        AvaloniaProperty.Register<Skeleton, IReadOnlyList<SkeletonRowDefinition>?>(nameof(Rows));

    public static readonly StyledProperty<double> RowSpacingProperty =
        AvaloniaProperty.Register<Skeleton, double>(nameof(RowSpacing), 12d);

    public static readonly DirectProperty<Skeleton, bool> ShowSkeletonProperty =
        AvaloniaProperty.RegisterDirect<Skeleton, bool>(
            nameof(ShowSkeleton),
            o => o.ShowSkeleton);

    public static readonly DirectProperty<Skeleton, bool> ShowContentProperty =
        AvaloniaProperty.RegisterDirect<Skeleton, bool>(
            nameof(ShowContent),
            o => o.ShowContent);

    public static readonly DirectProperty<Skeleton, bool> ShowGradientAnimationProperty =
        AvaloniaProperty.RegisterDirect<Skeleton, bool>(
            nameof(ShowGradientAnimation),
            o => o.ShowGradientAnimation);

    public static readonly DirectProperty<Skeleton, bool> ShowFlashedAnimationProperty =
        AvaloniaProperty.RegisterDirect<Skeleton, bool>(
            nameof(ShowFlashedAnimation),
            o => o.ShowFlashedAnimation);

    public static readonly DirectProperty<Skeleton, IReadOnlyList<SkeletonRowLayout>> ResolvedRowsProperty =
        AvaloniaProperty.RegisterDirect<Skeleton, IReadOnlyList<SkeletonRowLayout>>(
            nameof(ResolvedRows),
            o => o.ResolvedRows);

    public static readonly DirectProperty<Skeleton, double> GradientLeadOffsetProperty =
        AvaloniaProperty.RegisterDirect<Skeleton, double>(
            nameof(GradientLeadOffset),
            o => o.GradientLeadOffset);

    public static readonly DirectProperty<Skeleton, double> GradientCenterOffsetProperty =
        AvaloniaProperty.RegisterDirect<Skeleton, double>(
            nameof(GradientCenterOffset),
            o => o.GradientCenterOffset);

    public static readonly DirectProperty<Skeleton, double> GradientTrailOffsetProperty =
        AvaloniaProperty.RegisterDirect<Skeleton, double>(
            nameof(GradientTrailOffset),
            o => o.GradientTrailOffset);

    public static readonly DirectProperty<Skeleton, double> FlashedOpacityProperty =
        AvaloniaProperty.RegisterDirect<Skeleton, double>(
            nameof(FlashedOpacity),
            o => o.FlashedOpacity);

    static Skeleton()
    {
        LoadingProperty.Changed.AddClassHandler<Skeleton>((control, _) => control.UpdateState());
        ThemeProperty.Changed.AddClassHandler<Skeleton>((control, _) => control.UpdateState());
        AnimationProperty.Changed.AddClassHandler<Skeleton>((control, _) => control.UpdateState());
        RowsProperty.Changed.AddClassHandler<Skeleton>((control, _) => control.UpdateResolvedRows(force: true));
        RowSpacingProperty.Changed.AddClassHandler<Skeleton>((control, _) => control.UpdateResolvedRows(force: true));
        ContentProperty.Changed.AddClassHandler<Skeleton>((control, _) => control.UpdateState());
        BoundsProperty.Changed.AddClassHandler<Skeleton>((control, _) => control.UpdateResolvedRows());
    }

    public Skeleton()
    {
        UpdateState();
    }

    /// <summary>
    /// 获取或设置是否显示骨架占位。
    /// </summary>
    public bool Loading
    {
        get => GetValue(LoadingProperty);
        set => SetValue(LoadingProperty, value);
    }

    /// <summary>
    /// 获取或设置骨架预设主题。
    /// </summary>
    public new SkeletonTheme Theme
    {
        get => GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    /// <summary>
    /// 获取或设置骨架动效类型。
    /// </summary>
    public SkeletonAnimation Animation
    {
        get => GetValue(AnimationProperty);
        set => SetValue(AnimationProperty, value);
    }

    /// <summary>
    /// 获取或设置自定义骨架行定义；设置后优先于 <see cref="Theme"/> 生效。
    /// </summary>
    public IReadOnlyList<SkeletonRowDefinition>? Rows
    {
        get => GetValue(RowsProperty);
        set => SetValue(RowsProperty, value);
    }

    /// <summary>
    /// 获取或设置骨架行之间的垂直间距。
    /// </summary>
    public double RowSpacing
    {
        get => GetValue(RowSpacingProperty);
        set => SetValue(RowSpacingProperty, value);
    }

    /// <summary>
    /// 获取当前是否展示骨架占位。
    /// </summary>
    public bool ShowSkeleton
    {
        get => _showSkeleton;
        private set => SetAndRaise(ShowSkeletonProperty, ref _showSkeleton, value);
    }

    /// <summary>
    /// 获取当前是否展示真实内容。
    /// </summary>
    public bool ShowContent
    {
        get => _showContent;
        private set => SetAndRaise(ShowContentProperty, ref _showContent, value);
    }

    /// <summary>
    /// 获取当前是否启用渐变动效。
    /// </summary>
    public bool ShowGradientAnimation
    {
        get => _showGradientAnimation;
        private set => SetAndRaise(ShowGradientAnimationProperty, ref _showGradientAnimation, value);
    }

    /// <summary>
    /// 获取当前是否启用闪烁动效。
    /// </summary>
    public bool ShowFlashedAnimation
    {
        get => _showFlashedAnimation;
        private set => SetAndRaise(ShowFlashedAnimationProperty, ref _showFlashedAnimation, value);
    }

    /// <summary>
    /// 获取模板使用的解析后行布局。
    /// </summary>
    public IReadOnlyList<SkeletonRowLayout> ResolvedRows
    {
        get => _resolvedRows;
        private set => SetAndRaise(ResolvedRowsProperty, ref _resolvedRows, value);
    }

    /// <summary>
    /// 获取渐变动效前沿位置。
    /// </summary>
    public double GradientLeadOffset
    {
        get => _gradientLeadOffset;
        private set => SetAndRaise(GradientLeadOffsetProperty, ref _gradientLeadOffset, value);
    }

    /// <summary>
    /// 获取渐变动效中心位置。
    /// </summary>
    public double GradientCenterOffset
    {
        get => _gradientCenterOffset;
        private set => SetAndRaise(GradientCenterOffsetProperty, ref _gradientCenterOffset, value);
    }

    /// <summary>
    /// 获取渐变动效尾部位置。
    /// </summary>
    public double GradientTrailOffset
    {
        get => _gradientTrailOffset;
        private set => SetAndRaise(GradientTrailOffsetProperty, ref _gradientTrailOffset, value);
    }

    /// <summary>
    /// 获取闪烁动效透明度。
    /// </summary>
    public double FlashedOpacity
    {
        get => _flashedOpacity;
        private set => SetAndRaise(FlashedOpacityProperty, ref _flashedOpacity, value);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var arranged = base.ArrangeOverride(finalSize);
        UpdateResolvedRows(finalSize.Width);
        return arranged;
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        UpdateAnimationState();
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        StopAnimationTimer();
        base.OnDetachedFromVisualTree(e);
    }

    private void UpdateState()
    {
        ShowSkeleton = Loading;
        ShowContent = !Loading && Content is not null;
        PseudoClasses.Set(":loading", ShowSkeleton);
        PseudoClasses.Set(":avatar", Theme == SkeletonTheme.Avatar);
        PseudoClasses.Set(":image", Theme == SkeletonTheme.Image);
        PseudoClasses.Set(":text", Theme == SkeletonTheme.Text);
        PseudoClasses.Set(":paragraph", Theme == SkeletonTheme.Paragraph);
        PseudoClasses.Set(":custom", HasCustomRows());

        UpdateAnimationState();
        UpdateResolvedRows(force: true);
    }

    private void UpdateAnimationState()
    {
        ShowGradientAnimation = ShowSkeleton && Animation == SkeletonAnimation.Gradient;
        ShowFlashedAnimation = ShowSkeleton && Animation == SkeletonAnimation.Flashed;

        PseudoClasses.Set(":gradient", ShowGradientAnimation);
        PseudoClasses.Set(":flashed", ShowFlashedAnimation);

        if (ShowGradientAnimation || ShowFlashedAnimation)
        {
            StartAnimationTimer();
            return;
        }

        StopAnimationTimer();
        GradientLeadOffset = -0.4d;
        GradientCenterOffset = -0.2d;
        GradientTrailOffset = 0d;
        FlashedOpacity = 1d;
    }

    private void StartAnimationTimer()
    {
        if (VisualRoot is null)
        {
            return;
        }

        _animationStartedAt = DateTimeOffset.UtcNow;

        _animationTimer ??= new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(33),
        };

        _animationTimer.Tick -= OnAnimationTick;
        _animationTimer.Tick += OnAnimationTick;

        if (!_animationTimer.IsEnabled)
        {
            _animationTimer.Start();
        }
    }

    private void StopAnimationTimer()
    {
        if (_animationTimer is null)
        {
            return;
        }

        _animationTimer.Tick -= OnAnimationTick;
        _animationTimer.Stop();
    }

    private void OnAnimationTick(object? sender, EventArgs e)
    {
        var elapsed = (DateTimeOffset.UtcNow - _animationStartedAt).TotalMilliseconds;
        var progress = (elapsed % DefaultAnimationDurationMilliseconds) / DefaultAnimationDurationMilliseconds;

        if (ShowGradientAnimation)
        {
            GradientLeadOffset = progress - 0.35d;
            GradientCenterOffset = progress;
            GradientTrailOffset = progress + 0.35d;
            FlashedOpacity = 1d;
            return;
        }

        if (ShowFlashedAnimation)
        {
            FlashedOpacity = 0.6d + (((Math.Sin(progress * Math.PI * 2d) + 1d) / 2d) * 0.35d);
        }
    }

    private void UpdateResolvedRows(double? availableWidth = null, bool force = false)
    {
        var width = availableWidth ?? Bounds.Width;
        if (!force && AreClose(width, _lastResolvedWidth))
        {
            return;
        }

        _lastResolvedWidth = width;
        ResolvedRows = BuildResolvedRows(width);
    }

    private IReadOnlyList<SkeletonRowLayout> BuildResolvedRows(double availableWidth)
    {
        var rows = GetEffectiveRows();
        if (rows.Count == 0)
        {
            return Array.Empty<SkeletonRowLayout>();
        }

        var resolvedWidth = double.IsFinite(availableWidth) && availableWidth > 0
            ? availableWidth
            : DefaultFallbackWidth;

        var result = new List<SkeletonRowLayout>(rows.Count);
        for (var rowIndex = 0; rowIndex < rows.Count; rowIndex++)
        {
            var row = rows[rowIndex];
            var rowBlocks = ResolveRowBlocks(row, resolvedWidth);
            var margin = rowIndex < rows.Count - 1 ? new Thickness(0, 0, 0, RowSpacing) : default;
            var padding = row.Indent > 0 ? new Thickness(row.Indent, 0, 0, 0) : default;
            result.Add(new SkeletonRowLayout(margin, padding, rowBlocks));
        }

        return result;
    }

    private IReadOnlyList<SkeletonBlockLayout> ResolveRowBlocks(SkeletonRowDefinition row, double availableWidth)
    {
        var blocks = row.Blocks;
        if (blocks.Count == 0)
        {
            return Array.Empty<SkeletonBlockLayout>();
        }

        var contentWidth = Math.Max(availableWidth - row.Indent, 0d);
        var spacingTotal = row.Spacing * Math.Max(blocks.Count - 1, 0);
        var fixedWidth = 0d;
        var flexibleCount = 0;

        foreach (var block in blocks)
        {
            if (block.Shape == SkeletonShape.Circle)
            {
                fixedWidth += block.Height;
                continue;
            }

            if (double.IsFinite(block.Width))
            {
                fixedWidth += Math.Max(block.Width, 0d);
                continue;
            }

            if (block.WidthFactor > 0d)
            {
                fixedWidth += contentWidth * block.WidthFactor;
                continue;
            }

            flexibleCount++;
        }

        var remainingWidth = Math.Max(contentWidth - spacingTotal - fixedWidth, 0d);
        var autoWidth = flexibleCount > 0 ? remainingWidth / flexibleCount : 0d;
        var result = new List<SkeletonBlockLayout>(blocks.Count);

        for (var index = 0; index < blocks.Count; index++)
        {
            var block = blocks[index];
            var width = ResolveBlockWidth(block, contentWidth, autoWidth);
            var height = Math.Max(block.Height, 0d);
            var cornerRadius = ResolveCornerRadius(block, width, height);
            var margin = index > 0 ? new Thickness(row.Spacing, 0, 0, 0) : default;
            result.Add(new SkeletonBlockLayout(width, height, cornerRadius, margin));
        }

        return result;
    }

    private IReadOnlyList<SkeletonRowDefinition> GetEffectiveRows()
    {
        if (HasCustomRows())
        {
            return Rows!;
        }

        return Theme switch
        {
            SkeletonTheme.Avatar => CreateAvatarRows(),
            SkeletonTheme.Image => CreateImageRows(),
            SkeletonTheme.Paragraph => CreateParagraphRows(),
            SkeletonTheme.Custom => Array.Empty<SkeletonRowDefinition>(),
            _ => CreateTextRows(),
        };
    }

    private bool HasCustomRows()
    {
        return Rows is { Count: > 0 };
    }

    private static double ResolveBlockWidth(SkeletonBlockDefinition block, double contentWidth, double autoWidth)
    {
        if (block.Shape == SkeletonShape.Circle)
        {
            return Math.Max(block.Height, 0d);
        }

        if (double.IsFinite(block.Width))
        {
            return Math.Max(block.Width, 0d);
        }

        if (block.WidthFactor > 0d)
        {
            return Math.Max(contentWidth * block.WidthFactor, 0d);
        }

        return Math.Max(autoWidth, 0d);
    }

    private static CornerRadius ResolveCornerRadius(SkeletonBlockDefinition block, double width, double height)
    {
        if (block.Shape == SkeletonShape.Circle)
        {
            return new CornerRadius(Math.Max(width, height));
        }

        if (block.Shape == SkeletonShape.Round)
        {
            return new CornerRadius(height / 2d);
        }

        if (!block.CornerRadius.Equals(default(CornerRadius)))
        {
            return block.CornerRadius;
        }

        return new CornerRadius(6d);
    }

    private static IReadOnlyList<SkeletonRowDefinition> CreateTextRows()
    {
        return
        [
            new SkeletonRowDefinition
            {
                Blocks =
                [
                    new SkeletonBlockDefinition
                    {
                        WidthFactor = 0.4d,
                        Height = 16d,
                    },
                ],
            },
        ];
    }

    private static IReadOnlyList<SkeletonRowDefinition> CreateAvatarRows()
    {
        return
        [
            new SkeletonRowDefinition
            {
                Blocks =
                [
                    new SkeletonBlockDefinition
                    {
                        Height = 48d,
                        Shape = SkeletonShape.Circle,
                    },
                ],
            },
        ];
    }

    private static IReadOnlyList<SkeletonRowDefinition> CreateImageRows()
    {
        return
        [
            new SkeletonRowDefinition
            {
                Blocks =
                [
                    new SkeletonBlockDefinition
                    {
                        Height = 112d,
                        WidthFactor = 1d,
                        Shape = SkeletonShape.Rect,
                        CornerRadius = new CornerRadius(12d),
                    },
                ],
            },
        ];
    }

    private static IReadOnlyList<SkeletonRowDefinition> CreateParagraphRows()
    {
        return
        [
            new SkeletonRowDefinition
            {
                Blocks =
                [
                    new SkeletonBlockDefinition
                    {
                        WidthFactor = 1d,
                        Height = 16d,
                    },
                ],
            },
            new SkeletonRowDefinition
            {
                Blocks =
                [
                    new SkeletonBlockDefinition
                    {
                        WidthFactor = 1d,
                        Height = 16d,
                    },
                ],
            },
            new SkeletonRowDefinition
            {
                Blocks =
                [
                    new SkeletonBlockDefinition
                    {
                        WidthFactor = 0.61d,
                        Height = 16d,
                    },
                ],
            },
        ];
    }

    private static bool AreClose(double left, double right)
    {
        if (!double.IsFinite(left) && !double.IsFinite(right))
        {
            return true;
        }

        if (!double.IsFinite(left) || !double.IsFinite(right))
        {
            return false;
        }

        return Math.Abs(left - right) < 0.5d;
    }
}

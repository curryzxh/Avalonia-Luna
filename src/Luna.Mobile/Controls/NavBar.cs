using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using System;

namespace Luna.Mobile.Controls;

/// <summary>
/// 移动端导航栏控件。
/// </summary>
/// <remarks>
/// 模板契约：
/// <list type="bullet">
/// <item><description>PART_BackButton：<see cref="Button"/></description></item>
/// </list>
/// </remarks>
[TemplatePart(BackButtonPartName, typeof(Button))]
public sealed class NavBar : TemplatedControl
{
    private const string BackButtonPartName = "PART_BackButton";

    private Button? _backButton;

    /// <inheritdoc cref="Title" />
    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<NavBar, string?>(nameof(Title));

    /// <inheritdoc cref="ShowBackButton" />
    public static readonly StyledProperty<bool> ShowBackButtonProperty =
        AvaloniaProperty.Register<NavBar, bool>(nameof(ShowBackButton));

    /// <inheritdoc cref="LeftContent" />
    public static readonly StyledProperty<object?> LeftContentProperty =
        AvaloniaProperty.Register<NavBar, object?>(nameof(LeftContent));

    /// <inheritdoc cref="RightContent" />
    public static readonly StyledProperty<object?> RightContentProperty =
        AvaloniaProperty.Register<NavBar, object?>(nameof(RightContent));

    /// <inheritdoc cref="ShowDivider" />
    public static readonly StyledProperty<bool> ShowDividerProperty =
        AvaloniaProperty.Register<NavBar, bool>(nameof(ShowDivider), true);

    /// <inheritdoc cref="BarHeight" />
    public static readonly StyledProperty<double> BarHeightProperty =
        AvaloniaProperty.Register<NavBar, double>(nameof(BarHeight), 56d);

    /// <summary>
    /// 定义返回按钮触发事件。
    /// </summary>
    public static readonly RoutedEvent<RoutedEventArgs> BackRequestedEvent =
        RoutedEvent.Register<NavBar, RoutedEventArgs>(nameof(BackRequested), RoutingStrategies.Bubble);

    /// <summary>
    /// 当用户点击返回按钮时触发。
    /// </summary>
    public event EventHandler<RoutedEventArgs>? BackRequested
    {
        add => AddHandler(BackRequestedEvent, value);
        remove => RemoveHandler(BackRequestedEvent, value);
    }

    /// <summary>
    /// 获取或设置导航栏标题。
    /// </summary>
    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>
    /// 获取或设置是否显示返回按钮。
    /// </summary>
    public bool ShowBackButton
    {
        get => GetValue(ShowBackButtonProperty);
        set => SetValue(ShowBackButtonProperty, value);
    }

    /// <summary>
    /// 获取或设置左侧扩展内容。
    /// </summary>
    public object? LeftContent
    {
        get => GetValue(LeftContentProperty);
        set => SetValue(LeftContentProperty, value);
    }

    /// <summary>
    /// 获取或设置右侧扩展内容。
    /// </summary>
    public object? RightContent
    {
        get => GetValue(RightContentProperty);
        set => SetValue(RightContentProperty, value);
    }

    /// <summary>
    /// 获取或设置是否显示底部分割线。
    /// </summary>
    public bool ShowDivider
    {
        get => GetValue(ShowDividerProperty);
        set => SetValue(ShowDividerProperty, value);
    }

    /// <summary>
    /// 获取或设置导航栏高度。
    /// </summary>
    public double BarHeight
    {
        get => GetValue(BarHeightProperty);
        set => SetValue(BarHeightProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        if (_backButton is not null)
        {
            _backButton.Click -= BackButton_OnClick;
        }

        _backButton = e.NameScope.Find<Button>(BackButtonPartName);

        if (_backButton is not null)
        {
            _backButton.Click += BackButton_OnClick;
        }
    }

    private void BackButton_OnClick(object? sender, RoutedEventArgs e)
    {
        RaiseEvent(new RoutedEventArgs(BackRequestedEvent));
    }
}

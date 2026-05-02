using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using System;
using System.Collections.Specialized;

namespace Luna.Mobile.Controls;

/// <summary>
/// 引导展示模式。
/// </summary>
public enum GuideMode
{
    /// <summary>
    /// 气泡提示。
    /// </summary>
    Popover,

    /// <summary>
    /// 对话框提示。
    /// </summary>
    Dialog,
}

/// <summary>
/// 引导气泡相对目标的位置。
/// </summary>
public enum GuidePlacement
{
    Top,
    Bottom,
    Left,
    Right,
    Center,
    BottomRight,
    BottomLeft,
    TopRight,
    TopLeft,
}

/// <summary>
/// 单个引导步骤。
/// </summary>
public sealed class GuideStep : AvaloniaObject
{
    /// <summary>
    /// 获取或设置高亮目标控件。
    /// </summary>
    public Control? Target { get; set; }

    /// <summary>
    /// 获取或设置标题。
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// 获取或设置说明文本。
    /// </summary>
    public string? Body { get; set; }

    /// <summary>
    /// 获取或设置自定义内容。
    /// </summary>
    public object? Content { get; set; }

    /// <summary>
    /// 获取或设置该步骤的模式覆盖。
    /// </summary>
    public GuideMode? Mode { get; set; }

    /// <summary>
    /// 获取或设置该步骤的气泡位置。
    /// </summary>
    public GuidePlacement Placement { get; set; } = GuidePlacement.Top;

    /// <summary>
    /// 获取或设置该步骤的高亮边距。
    /// </summary>
    public double? HighlightPadding { get; set; }

    /// <summary>
    /// 获取或设置该步骤是否显示遮罩。
    /// </summary>
    public bool? ShowOverlay { get; set; }
}

/// <summary>
/// 当前引导变化事件参数。
/// </summary>
public sealed class GuideCurrentChangedEventArgs(int current, int total) : EventArgs
{
    /// <summary>
    /// 获取当前步骤索引。
    /// </summary>
    public int Current { get; } = current;

    /// <summary>
    /// 获取总步骤数。
    /// </summary>
    public int Total { get; } = total;
}

/// <summary>
/// 引导宿主控件。
/// </summary>
[TemplatePart(PopoverPanelPartName, typeof(Border))]
[TemplatePart(DialogPanelPartName, typeof(Border))]
[TemplatePart(BackButtonPopoverPartName, typeof(Button))]
[TemplatePart(SkipButtonPopoverPartName, typeof(Button))]
[TemplatePart(NextButtonPopoverPartName, typeof(Button))]
[TemplatePart(FinishButtonPopoverPartName, typeof(Button))]
[TemplatePart(BackButtonDialogPartName, typeof(Button))]
[TemplatePart(SkipButtonDialogPartName, typeof(Button))]
[TemplatePart(NextButtonDialogPartName, typeof(Button))]
[TemplatePart(FinishButtonDialogPartName, typeof(Button))]
public sealed class GuideHost : ContentControl
{
    private const string PopoverPanelPartName = "PART_PopoverPanel";
    private const string DialogPanelPartName = "PART_DialogPanel";
    private const string BackButtonPopoverPartName = "PART_BackButton_Popover";
    private const string SkipButtonPopoverPartName = "PART_SkipButton_Popover";
    private const string NextButtonPopoverPartName = "PART_NextButton_Popover";
    private const string FinishButtonPopoverPartName = "PART_FinishButton_Popover";
    private const string BackButtonDialogPartName = "PART_BackButton_Dialog";
    private const string SkipButtonDialogPartName = "PART_SkipButton_Dialog";
    private const string NextButtonDialogPartName = "PART_NextButton_Dialog";
    private const string FinishButtonDialogPartName = "PART_FinishButton_Dialog";

    private Border? _popoverPanel;
    private Border? _dialogPanel;
    private Button? _backButtonPopover;
    private Button? _skipButtonPopover;
    private Button? _nextButtonPopover;
    private Button? _finishButtonPopover;
    private Button? _backButtonDialog;
    private Button? _skipButtonDialog;
    private Button? _nextButtonDialog;
    private Button? _finishButtonDialog;
    private int _totalSteps;
    private bool _hasCurrentStep;
    private GuideMode _resolvedMode;
    private bool _resolvedShowOverlay = true;
    private string? _resolvedTitle;
    private string? _resolvedBody;
    private object? _resolvedContent;
    private bool _hasResolvedTitle;
    private bool _hasResolvedBody;
    private bool _hasResolvedContent;
    private bool _showDefaultContent = true;
    private bool _isPopoverMode;
    private bool _isDialogMode;
    private bool _isLastStep;
    private bool _canGoBack;
    private bool _showSkipButton = true;
    private bool _showCounter = true;
    private bool _showNextButton = true;
    private string _counterText = "1/1";
    private double _highlightLeft;
    private double _highlightTop;
    private double _highlightWidth;
    private double _highlightHeight;
    private double _popoverLeft;
    private double _popoverTop;
    private double _popoverWidth = 260;

    /// <inheritdoc cref="Current" />
    public static readonly StyledProperty<int> CurrentProperty =
        AvaloniaProperty.Register<GuideHost, int>(
            nameof(Current),
            -1,
            defaultBindingMode: BindingMode.TwoWay);

    /// <inheritdoc cref="Mode" />
    public static readonly StyledProperty<GuideMode> ModeProperty =
        AvaloniaProperty.Register<GuideHost, GuideMode>(nameof(Mode), GuideMode.Popover);

    /// <inheritdoc cref="ShowOverlay" />
    public static readonly StyledProperty<bool> ShowOverlayProperty =
        AvaloniaProperty.Register<GuideHost, bool>(nameof(ShowOverlay), true);

    /// <inheritdoc cref="HideSkip" />
    public static readonly StyledProperty<bool> HideSkipProperty =
        AvaloniaProperty.Register<GuideHost, bool>(nameof(HideSkip));

    /// <inheritdoc cref="HideCounter" />
    public static readonly StyledProperty<bool> HideCounterProperty =
        AvaloniaProperty.Register<GuideHost, bool>(nameof(HideCounter));

    /// <inheritdoc cref="HighlightPadding" />
    public static readonly StyledProperty<double> HighlightPaddingProperty =
        AvaloniaProperty.Register<GuideHost, double>(nameof(HighlightPadding), 8d);

    /// <inheritdoc cref="BackText" />
    public static readonly StyledProperty<string> BackTextProperty =
        AvaloniaProperty.Register<GuideHost, string>(nameof(BackText), "返回");

    /// <inheritdoc cref="SkipText" />
    public static readonly StyledProperty<string> SkipTextProperty =
        AvaloniaProperty.Register<GuideHost, string>(nameof(SkipText), "跳过");

    /// <inheritdoc cref="NextText" />
    public static readonly StyledProperty<string> NextTextProperty =
        AvaloniaProperty.Register<GuideHost, string>(nameof(NextText), "下一步");

    /// <inheritdoc cref="FinishText" />
    public static readonly StyledProperty<string> FinishTextProperty =
        AvaloniaProperty.Register<GuideHost, string>(nameof(FinishText), "完成");

    /// <inheritdoc cref="HasCurrentStep" />
    public static readonly DirectProperty<GuideHost, bool> HasCurrentStepProperty =
        AvaloniaProperty.RegisterDirect<GuideHost, bool>(
            nameof(HasCurrentStep),
            o => o.HasCurrentStep);

    /// <inheritdoc cref="ResolvedMode" />
    public static readonly DirectProperty<GuideHost, GuideMode> ResolvedModeProperty =
        AvaloniaProperty.RegisterDirect<GuideHost, GuideMode>(
            nameof(ResolvedMode),
            o => o.ResolvedMode);

    /// <inheritdoc cref="ResolvedShowOverlay" />
    public static readonly DirectProperty<GuideHost, bool> ResolvedShowOverlayProperty =
        AvaloniaProperty.RegisterDirect<GuideHost, bool>(
            nameof(ResolvedShowOverlay),
            o => o.ResolvedShowOverlay);

    /// <inheritdoc cref="ResolvedTitle" />
    public static readonly DirectProperty<GuideHost, string?> ResolvedTitleProperty =
        AvaloniaProperty.RegisterDirect<GuideHost, string?>(
            nameof(ResolvedTitle),
            o => o.ResolvedTitle);

    /// <inheritdoc cref="ResolvedBody" />
    public static readonly DirectProperty<GuideHost, string?> ResolvedBodyProperty =
        AvaloniaProperty.RegisterDirect<GuideHost, string?>(
            nameof(ResolvedBody),
            o => o.ResolvedBody);

    /// <inheritdoc cref="ResolvedContent" />
    public static readonly DirectProperty<GuideHost, object?> ResolvedContentProperty =
        AvaloniaProperty.RegisterDirect<GuideHost, object?>(
            nameof(ResolvedContent),
            o => o.ResolvedContent);

    /// <inheritdoc cref="HasResolvedTitle" />
    public static readonly DirectProperty<GuideHost, bool> HasResolvedTitleProperty =
        AvaloniaProperty.RegisterDirect<GuideHost, bool>(
            nameof(HasResolvedTitle),
            o => o.HasResolvedTitle);

    /// <inheritdoc cref="HasResolvedBody" />
    public static readonly DirectProperty<GuideHost, bool> HasResolvedBodyProperty =
        AvaloniaProperty.RegisterDirect<GuideHost, bool>(
            nameof(HasResolvedBody),
            o => o.HasResolvedBody);

    /// <inheritdoc cref="HasResolvedContent" />
    public static readonly DirectProperty<GuideHost, bool> HasResolvedContentProperty =
        AvaloniaProperty.RegisterDirect<GuideHost, bool>(
            nameof(HasResolvedContent),
            o => o.HasResolvedContent);

    /// <inheritdoc cref="ShowDefaultContent" />
    public static readonly DirectProperty<GuideHost, bool> ShowDefaultContentProperty =
        AvaloniaProperty.RegisterDirect<GuideHost, bool>(
            nameof(ShowDefaultContent),
            o => o.ShowDefaultContent);

    /// <inheritdoc cref="IsPopoverMode" />
    public static readonly DirectProperty<GuideHost, bool> IsPopoverModeProperty =
        AvaloniaProperty.RegisterDirect<GuideHost, bool>(
            nameof(IsPopoverMode),
            o => o.IsPopoverMode);

    /// <inheritdoc cref="IsDialogMode" />
    public static readonly DirectProperty<GuideHost, bool> IsDialogModeProperty =
        AvaloniaProperty.RegisterDirect<GuideHost, bool>(
            nameof(IsDialogMode),
            o => o.IsDialogMode);

    /// <inheritdoc cref="IsLastStep" />
    public static readonly DirectProperty<GuideHost, bool> IsLastStepProperty =
        AvaloniaProperty.RegisterDirect<GuideHost, bool>(
            nameof(IsLastStep),
            o => o.IsLastStep);

    /// <inheritdoc cref="CanGoBack" />
    public static readonly DirectProperty<GuideHost, bool> CanGoBackProperty =
        AvaloniaProperty.RegisterDirect<GuideHost, bool>(
            nameof(CanGoBack),
            o => o.CanGoBack);

    /// <inheritdoc cref="ShowSkipButton" />
    public static readonly DirectProperty<GuideHost, bool> ShowSkipButtonProperty =
        AvaloniaProperty.RegisterDirect<GuideHost, bool>(
            nameof(ShowSkipButton),
            o => o.ShowSkipButton);

    /// <inheritdoc cref="ShowCounter" />
    public static readonly DirectProperty<GuideHost, bool> ShowCounterProperty =
        AvaloniaProperty.RegisterDirect<GuideHost, bool>(
            nameof(ShowCounter),
            o => o.ShowCounter);

    /// <inheritdoc cref="ShowNextButton" />
    public static readonly DirectProperty<GuideHost, bool> ShowNextButtonProperty =
        AvaloniaProperty.RegisterDirect<GuideHost, bool>(
            nameof(ShowNextButton),
            o => o.ShowNextButton);

    /// <inheritdoc cref="CounterText" />
    public static readonly DirectProperty<GuideHost, string> CounterTextProperty =
        AvaloniaProperty.RegisterDirect<GuideHost, string>(
            nameof(CounterText),
            o => o.CounterText);

    /// <inheritdoc cref="HighlightLeft" />
    public static readonly DirectProperty<GuideHost, double> HighlightLeftProperty =
        AvaloniaProperty.RegisterDirect<GuideHost, double>(
            nameof(HighlightLeft),
            o => o.HighlightLeft);

    /// <inheritdoc cref="HighlightTop" />
    public static readonly DirectProperty<GuideHost, double> HighlightTopProperty =
        AvaloniaProperty.RegisterDirect<GuideHost, double>(
            nameof(HighlightTop),
            o => o.HighlightTop);

    /// <inheritdoc cref="HighlightWidth" />
    public static readonly DirectProperty<GuideHost, double> HighlightWidthProperty =
        AvaloniaProperty.RegisterDirect<GuideHost, double>(
            nameof(HighlightWidth),
            o => o.HighlightWidth);

    /// <inheritdoc cref="HighlightHeight" />
    public static readonly DirectProperty<GuideHost, double> HighlightHeightProperty =
        AvaloniaProperty.RegisterDirect<GuideHost, double>(
            nameof(HighlightHeight),
            o => o.HighlightHeight);

    /// <inheritdoc cref="PopoverLeft" />
    public static readonly DirectProperty<GuideHost, double> PopoverLeftProperty =
        AvaloniaProperty.RegisterDirect<GuideHost, double>(
            nameof(PopoverLeft),
            o => o.PopoverLeft);

    /// <inheritdoc cref="PopoverTop" />
    public static readonly DirectProperty<GuideHost, double> PopoverTopProperty =
        AvaloniaProperty.RegisterDirect<GuideHost, double>(
            nameof(PopoverTop),
            o => o.PopoverTop);

    /// <inheritdoc cref="PopoverWidth" />
    public static readonly DirectProperty<GuideHost, double> PopoverWidthProperty =
        AvaloniaProperty.RegisterDirect<GuideHost, double>(
            nameof(PopoverWidth),
            o => o.PopoverWidth);

    static GuideHost()
    {
        CurrentProperty.Changed.AddClassHandler<GuideHost>((control, _) => control.UpdateResolvedState(true));
        ModeProperty.Changed.AddClassHandler<GuideHost>((control, _) => control.UpdateResolvedState(false));
        ShowOverlayProperty.Changed.AddClassHandler<GuideHost>((control, _) => control.UpdateResolvedState(false));
        HideSkipProperty.Changed.AddClassHandler<GuideHost>((control, _) => control.UpdateResolvedState(false));
        HideCounterProperty.Changed.AddClassHandler<GuideHost>((control, _) => control.UpdateResolvedState(false));
        HighlightPaddingProperty.Changed.AddClassHandler<GuideHost>((control, _) => control.UpdateResolvedState(false));
        BoundsProperty.Changed.AddClassHandler<GuideHost>((control, _) => control.UpdateGuideLayout());
    }

    /// <summary>
    /// 当前步骤变化时触发。
    /// </summary>
    public event EventHandler<GuideCurrentChangedEventArgs>? CurrentChanged;

    /// <summary>
    /// 点击跳过时触发。
    /// </summary>
    public event EventHandler<GuideCurrentChangedEventArgs>? SkipRequested;

    /// <summary>
    /// 引导结束时触发。
    /// </summary>
    public event EventHandler<GuideCurrentChangedEventArgs>? Finished;

    /// <summary>
    /// 初始化引导宿主。
    /// </summary>
    public GuideHost()
    {
        Steps.CollectionChanged += OnStepsCollectionChanged;
        LayoutUpdated += OnLayoutUpdated;
    }

    /// <summary>
    /// 获取步骤集合。
    /// </summary>
    public AvaloniaList<GuideStep> Steps { get; } = [];

    /// <summary>
    /// 获取或设置当前步骤索引，设置为 -1 时隐藏引导。
    /// </summary>
    public int Current
    {
        get => GetValue(CurrentProperty);
        set => SetValue(CurrentProperty, value);
    }

    /// <summary>
    /// 获取或设置默认展示模式。
    /// </summary>
    public GuideMode Mode
    {
        get => GetValue(ModeProperty);
        set => SetValue(ModeProperty, value);
    }

    /// <summary>
    /// 获取或设置是否显示遮罩。
    /// </summary>
    public bool ShowOverlay
    {
        get => GetValue(ShowOverlayProperty);
        set => SetValue(ShowOverlayProperty, value);
    }

    /// <summary>
    /// 获取或设置是否隐藏跳过按钮。
    /// </summary>
    public bool HideSkip
    {
        get => GetValue(HideSkipProperty);
        set => SetValue(HideSkipProperty, value);
    }

    /// <summary>
    /// 获取或设置是否隐藏计数。
    /// </summary>
    public bool HideCounter
    {
        get => GetValue(HideCounterProperty);
        set => SetValue(HideCounterProperty, value);
    }

    /// <summary>
    /// 获取或设置高亮框默认内边距。
    /// </summary>
    public double HighlightPadding
    {
        get => GetValue(HighlightPaddingProperty);
        set => SetValue(HighlightPaddingProperty, value);
    }

    /// <summary>
    /// 获取或设置返回按钮文本。
    /// </summary>
    public string BackText
    {
        get => GetValue(BackTextProperty);
        set => SetValue(BackTextProperty, value);
    }

    /// <summary>
    /// 获取或设置跳过按钮文本。
    /// </summary>
    public string SkipText
    {
        get => GetValue(SkipTextProperty);
        set => SetValue(SkipTextProperty, value);
    }

    /// <summary>
    /// 获取或设置下一步按钮文本。
    /// </summary>
    public string NextText
    {
        get => GetValue(NextTextProperty);
        set => SetValue(NextTextProperty, value);
    }

    /// <summary>
    /// 获取或设置完成按钮文本。
    /// </summary>
    public string FinishText
    {
        get => GetValue(FinishTextProperty);
        set => SetValue(FinishTextProperty, value);
    }

    /// <summary>
    /// 获取当前是否正在显示引导。
    /// </summary>
    public bool HasCurrentStep
    {
        get => _hasCurrentStep;
        private set => SetAndRaise(HasCurrentStepProperty, ref _hasCurrentStep, value);
    }

    /// <summary>
    /// 获取当前解析出的展示模式。
    /// </summary>
    public GuideMode ResolvedMode
    {
        get => _resolvedMode;
        private set => SetAndRaise(ResolvedModeProperty, ref _resolvedMode, value);
    }

    /// <summary>
    /// 获取当前解析出的遮罩显示状态。
    /// </summary>
    public bool ResolvedShowOverlay
    {
        get => _resolvedShowOverlay;
        private set => SetAndRaise(ResolvedShowOverlayProperty, ref _resolvedShowOverlay, value);
    }

    /// <summary>
    /// 获取当前解析出的标题。
    /// </summary>
    public string? ResolvedTitle
    {
        get => _resolvedTitle;
        private set => SetAndRaise(ResolvedTitleProperty, ref _resolvedTitle, value);
    }

    /// <summary>
    /// 获取当前解析出的说明文本。
    /// </summary>
    public string? ResolvedBody
    {
        get => _resolvedBody;
        private set => SetAndRaise(ResolvedBodyProperty, ref _resolvedBody, value);
    }

    /// <summary>
    /// 获取当前解析出的自定义内容。
    /// </summary>
    public object? ResolvedContent
    {
        get => _resolvedContent;
        private set => SetAndRaise(ResolvedContentProperty, ref _resolvedContent, value);
    }

    /// <summary>
    /// 获取当前步骤是否有标题。
    /// </summary>
    public bool HasResolvedTitle
    {
        get => _hasResolvedTitle;
        private set => SetAndRaise(HasResolvedTitleProperty, ref _hasResolvedTitle, value);
    }

    /// <summary>
    /// 获取当前步骤是否有说明文本。
    /// </summary>
    public bool HasResolvedBody
    {
        get => _hasResolvedBody;
        private set => SetAndRaise(HasResolvedBodyProperty, ref _hasResolvedBody, value);
    }

    /// <summary>
    /// 获取当前步骤是否有自定义内容。
    /// </summary>
    public bool HasResolvedContent
    {
        get => _hasResolvedContent;
        private set => SetAndRaise(HasResolvedContentProperty, ref _hasResolvedContent, value);
    }

    /// <summary>
    /// 获取当前是否显示默认标题和说明区域。
    /// </summary>
    public bool ShowDefaultContent
    {
        get => _showDefaultContent;
        private set => SetAndRaise(ShowDefaultContentProperty, ref _showDefaultContent, value);
    }

    /// <summary>
    /// 获取当前是否为气泡模式。
    /// </summary>
    public bool IsPopoverMode
    {
        get => _isPopoverMode;
        private set => SetAndRaise(IsPopoverModeProperty, ref _isPopoverMode, value);
    }

    /// <summary>
    /// 获取当前是否为对话框模式。
    /// </summary>
    public bool IsDialogMode
    {
        get => _isDialogMode;
        private set => SetAndRaise(IsDialogModeProperty, ref _isDialogMode, value);
    }

    /// <summary>
    /// 获取当前是否为最后一步。
    /// </summary>
    public bool IsLastStep
    {
        get => _isLastStep;
        private set => SetAndRaise(IsLastStepProperty, ref _isLastStep, value);
    }

    /// <summary>
    /// 获取当前是否可以返回上一步。
    /// </summary>
    public bool CanGoBack
    {
        get => _canGoBack;
        private set => SetAndRaise(CanGoBackProperty, ref _canGoBack, value);
    }

    /// <summary>
    /// 获取当前是否显示跳过按钮。
    /// </summary>
    public bool ShowSkipButton
    {
        get => _showSkipButton;
        private set => SetAndRaise(ShowSkipButtonProperty, ref _showSkipButton, value);
    }

    /// <summary>
    /// 获取当前是否显示计数。
    /// </summary>
    public bool ShowCounter
    {
        get => _showCounter;
        private set => SetAndRaise(ShowCounterProperty, ref _showCounter, value);
    }

    /// <summary>
    /// 获取当前是否显示下一步按钮。
    /// </summary>
    public bool ShowNextButton
    {
        get => _showNextButton;
        private set => SetAndRaise(ShowNextButtonProperty, ref _showNextButton, value);
    }

    /// <summary>
    /// 获取当前计数字符串。
    /// </summary>
    public string CounterText
    {
        get => _counterText;
        private set => SetAndRaise(CounterTextProperty, ref _counterText, value);
    }

    /// <summary>
    /// 获取高亮框左侧位置。
    /// </summary>
    public double HighlightLeft
    {
        get => _highlightLeft;
        private set => SetAndRaise(HighlightLeftProperty, ref _highlightLeft, value);
    }

    /// <summary>
    /// 获取高亮框顶部位置。
    /// </summary>
    public double HighlightTop
    {
        get => _highlightTop;
        private set => SetAndRaise(HighlightTopProperty, ref _highlightTop, value);
    }

    /// <summary>
    /// 获取高亮框宽度。
    /// </summary>
    public double HighlightWidth
    {
        get => _highlightWidth;
        private set => SetAndRaise(HighlightWidthProperty, ref _highlightWidth, value);
    }

    /// <summary>
    /// 获取高亮框高度。
    /// </summary>
    public double HighlightHeight
    {
        get => _highlightHeight;
        private set => SetAndRaise(HighlightHeightProperty, ref _highlightHeight, value);
    }

    /// <summary>
    /// 获取气泡左侧位置。
    /// </summary>
    public double PopoverLeft
    {
        get => _popoverLeft;
        private set => SetAndRaise(PopoverLeftProperty, ref _popoverLeft, value);
    }

    /// <summary>
    /// 获取气泡顶部位置。
    /// </summary>
    public double PopoverTop
    {
        get => _popoverTop;
        private set => SetAndRaise(PopoverTopProperty, ref _popoverTop, value);
    }

    /// <summary>
    /// 获取气泡宽度。
    /// </summary>
    public double PopoverWidth
    {
        get => _popoverWidth;
        private set => SetAndRaise(PopoverWidthProperty, ref _popoverWidth, value);
    }

    /// <inheritdoc />
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        DetachButtonHandlers();

        _popoverPanel = e.NameScope.Find<Border>(PopoverPanelPartName);
        _dialogPanel = e.NameScope.Find<Border>(DialogPanelPartName);
        _backButtonPopover = e.NameScope.Find<Button>(BackButtonPopoverPartName);
        _skipButtonPopover = e.NameScope.Find<Button>(SkipButtonPopoverPartName);
        _nextButtonPopover = e.NameScope.Find<Button>(NextButtonPopoverPartName);
        _finishButtonPopover = e.NameScope.Find<Button>(FinishButtonPopoverPartName);
        _backButtonDialog = e.NameScope.Find<Button>(BackButtonDialogPartName);
        _skipButtonDialog = e.NameScope.Find<Button>(SkipButtonDialogPartName);
        _nextButtonDialog = e.NameScope.Find<Button>(NextButtonDialogPartName);
        _finishButtonDialog = e.NameScope.Find<Button>(FinishButtonDialogPartName);

        AttachButtonHandlers();
        UpdateResolvedState(false);
    }

    /// <summary>
    /// 开始引导。
    /// </summary>
    public void Start()
    {
        if (Steps.Count == 0)
        {
            return;
        }

        SetCurrentValue(CurrentProperty, 0);
    }

    /// <summary>
    /// 关闭引导。
    /// </summary>
    public void Close()
    {
        SetCurrentValue(CurrentProperty, -1);
    }

    /// <summary>
    /// 切换到上一步。
    /// </summary>
    public void Back()
    {
        if (Current <= 0)
        {
            return;
        }

        SetCurrentValue(CurrentProperty, Current - 1);
    }

    /// <summary>
    /// 切换到下一步。
    /// </summary>
    public void Next()
    {
        if (Current < 0)
        {
            return;
        }

        if (Current >= Steps.Count - 1)
        {
            Finish();
            return;
        }

        SetCurrentValue(CurrentProperty, Current + 1);
    }

    /// <summary>
    /// 跳过引导。
    /// </summary>
    public void Skip()
    {
        SkipRequested?.Invoke(this, new GuideCurrentChangedEventArgs(Current, Steps.Count));
        Close();
    }

    /// <summary>
    /// 完成引导。
    /// </summary>
    public void Finish()
    {
        Finished?.Invoke(this, new GuideCurrentChangedEventArgs(Current, Steps.Count));
        Close();
    }

    private void OnStepsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateResolvedState(false);
    }

    private void OnLayoutUpdated(object? sender, EventArgs e)
    {
        if (HasCurrentStep)
        {
            UpdateGuideLayout();
        }
    }

    private void AttachButtonHandlers()
    {
        if (_backButtonPopover is not null)
        {
            _backButtonPopover.Click += OnBackButtonClick;
        }

        if (_skipButtonPopover is not null)
        {
            _skipButtonPopover.Click += OnSkipButtonClick;
        }

        if (_nextButtonPopover is not null)
        {
            _nextButtonPopover.Click += OnNextButtonClick;
        }

        if (_finishButtonPopover is not null)
        {
            _finishButtonPopover.Click += OnFinishButtonClick;
        }

        if (_backButtonDialog is not null)
        {
            _backButtonDialog.Click += OnBackButtonClick;
        }

        if (_skipButtonDialog is not null)
        {
            _skipButtonDialog.Click += OnSkipButtonClick;
        }

        if (_nextButtonDialog is not null)
        {
            _nextButtonDialog.Click += OnNextButtonClick;
        }

        if (_finishButtonDialog is not null)
        {
            _finishButtonDialog.Click += OnFinishButtonClick;
        }
    }

    private void DetachButtonHandlers()
    {
        if (_backButtonPopover is not null)
        {
            _backButtonPopover.Click -= OnBackButtonClick;
        }

        if (_skipButtonPopover is not null)
        {
            _skipButtonPopover.Click -= OnSkipButtonClick;
        }

        if (_nextButtonPopover is not null)
        {
            _nextButtonPopover.Click -= OnNextButtonClick;
        }

        if (_finishButtonPopover is not null)
        {
            _finishButtonPopover.Click -= OnFinishButtonClick;
        }

        if (_backButtonDialog is not null)
        {
            _backButtonDialog.Click -= OnBackButtonClick;
        }

        if (_skipButtonDialog is not null)
        {
            _skipButtonDialog.Click -= OnSkipButtonClick;
        }

        if (_nextButtonDialog is not null)
        {
            _nextButtonDialog.Click -= OnNextButtonClick;
        }

        if (_finishButtonDialog is not null)
        {
            _finishButtonDialog.Click -= OnFinishButtonClick;
        }
    }

    private void OnBackButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Back();
    }

    private void OnSkipButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Skip();
    }

    private void OnNextButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Next();
    }

    private void OnFinishButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Finish();
    }

    private void UpdateResolvedState(bool raiseCurrentChanged)
    {
        _totalSteps = Steps.Count;

        if (Current < 0 || Current >= Steps.Count)
        {
            ResetResolvedState();

            if (raiseCurrentChanged)
            {
                CurrentChanged?.Invoke(this, new GuideCurrentChangedEventArgs(Current, Steps.Count));
            }

            return;
        }

        var step = Steps[Current];
        HasCurrentStep = true;
        ResolvedMode = step.Mode ?? Mode;
        ResolvedShowOverlay = step.ShowOverlay ?? ShowOverlay;
        ResolvedTitle = step.Title;
        ResolvedBody = step.Body;
        ResolvedContent = step.Content;
        HasResolvedTitle = !string.IsNullOrWhiteSpace(ResolvedTitle);
        HasResolvedBody = !string.IsNullOrWhiteSpace(ResolvedBody);
        HasResolvedContent = ResolvedContent is not null;
        ShowDefaultContent = !HasResolvedContent;
        IsPopoverMode = ResolvedMode == GuideMode.Popover;
        IsDialogMode = ResolvedMode == GuideMode.Dialog;
        IsLastStep = Current == Steps.Count - 1;
        CanGoBack = Current > 0;
        ShowSkipButton = !HideSkip && !IsLastStep;
        ShowCounter = !HideCounter;
        ShowNextButton = !IsLastStep;
        CounterText = $"{Current + 1}/{Math.Max(Steps.Count, 1)}";

        PseudoClasses.Set(":open", HasCurrentStep);
        PseudoClasses.Set(":dialog", ResolvedMode == GuideMode.Dialog);
        PseudoClasses.Set(":popover", ResolvedMode == GuideMode.Popover);
        PseudoClasses.Set(":overlay", ResolvedShowOverlay);

        UpdateGuideLayout();

        if (raiseCurrentChanged)
        {
            CurrentChanged?.Invoke(this, new GuideCurrentChangedEventArgs(Current, Steps.Count));
        }
    }

    private void ResetResolvedState()
    {
        HasCurrentStep = false;
        ResolvedMode = Mode;
        ResolvedShowOverlay = ShowOverlay;
        ResolvedTitle = null;
        ResolvedBody = null;
        ResolvedContent = null;
        HasResolvedTitle = false;
        HasResolvedBody = false;
        HasResolvedContent = false;
        ShowDefaultContent = true;
        IsPopoverMode = false;
        IsDialogMode = false;
        IsLastStep = false;
        CanGoBack = false;
        ShowSkipButton = false;
        ShowCounter = false;
        ShowNextButton = false;
        CounterText = $"0/{Math.Max(_totalSteps, 1)}";
        HighlightLeft = 0;
        HighlightTop = 0;
        HighlightWidth = 0;
        HighlightHeight = 0;
        PopoverLeft = 0;
        PopoverTop = 0;

        PseudoClasses.Set(":open", false);
        PseudoClasses.Set(":dialog", false);
        PseudoClasses.Set(":popover", false);
        PseudoClasses.Set(":overlay", false);
    }

    private void UpdateGuideLayout()
    {
        if (!HasCurrentStep || Current < 0 || Current >= Steps.Count || Bounds.Width <= 0 || Bounds.Height <= 0)
        {
            return;
        }

        var step = Steps[Current];
        if (step.Target is null)
        {
            HighlightLeft = 0;
            HighlightTop = 0;
            HighlightWidth = 0;
            HighlightHeight = 0;
            PopoverLeft = 16;
            PopoverTop = 16;
            return;
        }

        var targetPoint = step.Target.TranslatePoint(new Point(0, 0), this);
        if (targetPoint is null)
        {
            return;
        }

        var padding = step.HighlightPadding ?? HighlightPadding;
        var rect = new Rect(targetPoint.Value, step.Target.Bounds.Size).Inflate(padding);
        var clampedRect = new Rect(
            Math.Max(0, rect.X),
            Math.Max(0, rect.Y),
            Math.Min(Bounds.Width - Math.Max(0, rect.X), rect.Width),
            Math.Min(Bounds.Height - Math.Max(0, rect.Y), rect.Height));

        HighlightLeft = clampedRect.X;
        HighlightTop = clampedRect.Y;
        HighlightWidth = Math.Max(0, clampedRect.Width);
        HighlightHeight = Math.Max(0, clampedRect.Height);

        if (ResolvedMode == GuideMode.Dialog)
        {
            return;
        }

        var preferredWidth = Math.Min(Math.Max(Bounds.Width - 32, 220), 280);
        PopoverWidth = preferredWidth;

        var panelWidth = _popoverPanel?.Bounds.Width > 0 ? _popoverPanel.Bounds.Width : preferredWidth;
        var panelHeight = _popoverPanel?.Bounds.Height > 0 ? _popoverPanel.Bounds.Height : 168;
        var spacing = 12d;

        var left = clampedRect.X + (clampedRect.Width - panelWidth) / 2;
        var top = clampedRect.Y - panelHeight - spacing;

        switch (step.Placement)
        {
            case GuidePlacement.Center:
                left = clampedRect.X + (clampedRect.Width - panelWidth) / 2;
                top = clampedRect.Y + (clampedRect.Height - panelHeight) / 2;
                break;
            case GuidePlacement.Bottom:
                left = clampedRect.X + (clampedRect.Width - panelWidth) / 2;
                top = clampedRect.Bottom + spacing;
                break;
            case GuidePlacement.BottomRight:
                left = clampedRect.Right - panelWidth;
                top = clampedRect.Bottom + spacing;
                break;
            case GuidePlacement.BottomLeft:
                left = clampedRect.X;
                top = clampedRect.Bottom + spacing;
                break;
            case GuidePlacement.Left:
                left = clampedRect.X - panelWidth - spacing;
                top = clampedRect.Y + (clampedRect.Height - panelHeight) / 2;
                break;
            case GuidePlacement.Right:
                left = clampedRect.Right + spacing;
                top = clampedRect.Y + (clampedRect.Height - panelHeight) / 2;
                break;
            case GuidePlacement.TopRight:
                left = clampedRect.Right - panelWidth;
                top = clampedRect.Y - panelHeight - spacing;
                break;
            case GuidePlacement.TopLeft:
                left = clampedRect.X;
                top = clampedRect.Y - panelHeight - spacing;
                break;
            case GuidePlacement.Top:
            default:
                left = clampedRect.X + (clampedRect.Width - panelWidth) / 2;
                top = clampedRect.Y - panelHeight - spacing;
                break;
        }

        PopoverLeft = Math.Clamp(left, 16, Math.Max(16, Bounds.Width - panelWidth - 16));
        PopoverTop = Math.Clamp(top, 16, Math.Max(16, Bounds.Height - panelHeight - 16));
    }
}

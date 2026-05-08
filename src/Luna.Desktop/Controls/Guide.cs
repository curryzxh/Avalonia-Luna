using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

namespace Luna.Desktop.Controls;

[PseudoClasses(":active")]
public class Guide : ContentControl
{
    public static readonly StyledProperty<ObservableCollection<GuideStep>> StepsProperty =
        AvaloniaProperty.Register<Guide, ObservableCollection<GuideStep>>(nameof(Steps));

    public static readonly StyledProperty<int> CurrentStepIndexProperty =
        AvaloniaProperty.Register<Guide, int>(nameof(CurrentStepIndex), -1);

    public static readonly StyledProperty<bool> IsActiveProperty =
        AvaloniaProperty.Register<Guide, bool>(nameof(IsActive), false);

    public static readonly StyledProperty<string?> CurrentTitleProperty =
        AvaloniaProperty.Register<Guide, string?>(nameof(CurrentTitle));

    public static readonly StyledProperty<string?> CurrentDescriptionProperty =
        AvaloniaProperty.Register<Guide, string?>(nameof(CurrentDescription));

    private Border? _overlay;
    private Border? _highlightBorder;
    private Border? _infoPanel;
    private Control? _currentTarget;

    public ObservableCollection<GuideStep> Steps
    {
        get => GetValue(StepsProperty);
        set => SetValue(StepsProperty, value);
    }

    public int CurrentStepIndex
    {
        get => GetValue(CurrentStepIndexProperty);
        set => SetValue(CurrentStepIndexProperty, value);
    }

    public bool IsActive
    {
        get => GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    public string? CurrentTitle
    {
        get => GetValue(CurrentTitleProperty);
        set => SetValue(CurrentTitleProperty, value);
    }

    public string? CurrentDescription
    {
        get => GetValue(CurrentDescriptionProperty);
        set => SetValue(CurrentDescriptionProperty, value);
    }

    static Guide()
    {
        IsActiveProperty.Changed.AddClassHandler<Guide>(OnIsActiveChanged);
        CurrentStepIndexProperty.Changed.AddClassHandler<Guide>(OnCurrentStepIndexChanged);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _overlay = this.FindDescendantOfType<Border>();
    }

    public void Start()
    {
        if (Steps == null || Steps.Count == 0) return;
        IsActive = true;
        CurrentStepIndex = 0;
        PseudoClasses.Set(":active", true);
        ShowCurrentStep();
    }

    public void Next()
    {
        if (Steps == null || CurrentStepIndex < 0) return;
        var next = CurrentStepIndex + 1;
        if (next < Steps.Count)
        {
            CurrentStepIndex = next;
            ShowCurrentStep();
        }
        else
        {
            Finish();
        }
    }

    public void Prev()
    {
        if (Steps == null || CurrentStepIndex <= 0) return;
        CurrentStepIndex = CurrentStepIndex - 1;
        ShowCurrentStep();
    }

    public void Skip()
    {
        Finish();
    }

    public void Finish()
    {
        IsActive = false;
        CurrentStepIndex = -1;
        PseudoClasses.Set(":active", false);
        CurrentTitle = null;
        CurrentDescription = null;
    }

    private static void OnIsActiveChanged(Guide sender, AvaloniaPropertyChangedEventArgs e)
    {
        sender.PseudoClasses.Set(":active", sender.IsActive);
    }

    private static void OnCurrentStepIndexChanged(Guide sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (sender.IsActive)
        {
            sender.ShowCurrentStep();
        }
    }

    private void ShowCurrentStep()
    {
        if (Steps == null || CurrentStepIndex < 0 || CurrentStepIndex >= Steps.Count) return;

        var step = Steps[CurrentStepIndex];
        CurrentTitle = step.Title;
        CurrentDescription = step.Description;

        if (!string.IsNullOrEmpty(step.TargetControlId))
        {
            var topLevel = TopLevel.GetTopLevel(this);
            if (topLevel != null)
            {
                _currentTarget = topLevel.GetVisualDescendants()
                    .FirstOrDefault(d => d.Name == step.TargetControlId) as Control;
            }
        }
    }
}

public class GuideStep
{
    public string? TargetControlId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
}

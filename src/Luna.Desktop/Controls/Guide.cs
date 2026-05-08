using Avalonia;
using Avalonia.Controls;

namespace Luna.Desktop.Controls;

public class Guide : ContentControl
{
    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<Guide, string?>(nameof(Title));

    public static readonly StyledProperty<string?> DescriptionProperty =
        AvaloniaProperty.Register<Guide, string?>(nameof(Description));

    public static readonly StyledProperty<bool> IsVisibleProperty =
        AvaloniaProperty.Register<Guide, bool>(nameof(IsVisible), false);

    public static readonly StyledProperty<int> CurrentStepProperty =
        AvaloniaProperty.Register<Guide, int>(nameof(CurrentStep), 0);

    public static readonly StyledProperty<int> TotalStepsProperty =
        AvaloniaProperty.Register<Guide, int>(nameof(TotalSteps), 1);

    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string? Description
    {
        get => GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public bool IsVisible
    {
        get => GetValue(IsVisibleProperty);
        set => SetValue(IsVisibleProperty, value);
    }

    public int CurrentStep
    {
        get => GetValue(CurrentStepProperty);
        set => SetValue(CurrentStepProperty, value);
    }

    public int TotalSteps
    {
        get => GetValue(TotalStepsProperty);
        set => SetValue(TotalStepsProperty, value);
    }
}

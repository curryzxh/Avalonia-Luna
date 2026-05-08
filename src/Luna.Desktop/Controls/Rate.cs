using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;

namespace Luna.Desktop.Controls;

public class Rate : TemplatedControl
{
    public static readonly StyledProperty<int> ValueProperty =
        AvaloniaProperty.Register<Rate, int>(nameof(Value), defaultBindingMode: BindingMode.TwoWay);

    public int Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly StyledProperty<int> CountProperty =
        AvaloniaProperty.Register<Rate, int>(nameof(Count), 5);

    public int Count
    {
        get => GetValue(CountProperty);
        set => SetValue(CountProperty, value);
    }

    public static readonly StyledProperty<bool> AllowClearProperty =
        AvaloniaProperty.Register<Rate, bool>(nameof(AllowClear), true);

    public bool AllowClear
    {
        get => GetValue(AllowClearProperty);
        set => SetValue(AllowClearProperty, value);
    }

    public static readonly StyledProperty<string> CharacterProperty =
        AvaloniaProperty.Register<Rate, string>(nameof(Character), "★");

    public string Character
    {
        get => GetValue(CharacterProperty);
        set => SetValue(CharacterProperty, value);
    }

    public static readonly StyledProperty<double> IconSizeProperty =
        AvaloniaProperty.Register<Rate, double>(nameof(IconSize), 24);

    public double IconSize
    {
        get => GetValue(IconSizeProperty);
        set => SetValue(IconSizeProperty, value);
    }

    private readonly StackPanel _panel;

    static Rate()
    {
        ValueProperty.Changed.AddClassHandler<Rate>((x, _) => x.Rebuild());
        CountProperty.Changed.AddClassHandler<Rate>((x, _) => x.Rebuild());
        CharacterProperty.Changed.AddClassHandler<Rate>((x, _) => x.Rebuild());
        IconSizeProperty.Changed.AddClassHandler<Rate>((x, _) => x.Rebuild());
    }

    public Rate()
    {
        _panel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 4,
        };
        VisualChildren.Add(_panel);
        LogicalChildren.Add(_panel);
    }

    protected override void OnLoaded(global::Avalonia.Interactivity.RoutedEventArgs e)
    {
        base.OnLoaded(e);
        Rebuild();
    }

    private void Rebuild()
    {
        _panel.Children.Clear();
        for (int i = 1; i <= Count; i++)
        {
            var idx = i;
            var tb = new TextBlock
            {
                Text = Character,
                FontSize = IconSize,
                Width = IconSize,
                TextAlignment = TextAlignment.Center,
                Foreground = i <= Value
                    ? (IBrush?)this.FindResource("Luna.Brush.Warning") ?? Brushes.Orange
                    : (IBrush?)this.FindResource("Luna.Brush.Border.Default") ?? Brushes.LightGray,
            };
            var btn = new Button
            {
                Padding = new Avalonia.Thickness(2),
                Background = Brushes.Transparent,
                BorderThickness = new Avalonia.Thickness(0),
                Cursor = new Cursor(StandardCursorType.Hand),
                Content = tb,
            };
            btn.Click += (_, _) =>
            {
                if (AllowClear && Value == idx)
                    SetCurrentValue(ValueProperty, 0);
                else
                    SetCurrentValue(ValueProperty, idx);
            };
            _panel.Children.Add(btn);
        }
        UpdateColors();
    }

    private void UpdateColors()
    {
        var filledBrush = (IBrush?)this.FindResource("Luna.Brush.Warning") ?? Brushes.Orange;
        var emptyBrush = (IBrush?)this.FindResource("Luna.Brush.Border.Default") ?? Brushes.LightGray;

        for (int i = 0; i < _panel.Children.Count; i++)
        {
            if (_panel.Children[i] is Button btn && btn.Content is TextBlock tb)
            {
                tb.Foreground = (i + 1) <= Value ? filledBrush : emptyBrush;
            }
        }
    }
}

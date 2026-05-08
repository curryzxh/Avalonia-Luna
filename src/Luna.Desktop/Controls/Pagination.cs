using System;
using System.Collections.Generic;
using System.Windows.Input;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Luna.Desktop.Controls;

public class Pagination : TemplatedControl
{
    public const string PART_PreviousButton = "PART_PreviousButton";
    public const string PART_NextButton = "PART_NextButton";
    public const string PART_ButtonPanel = "PART_ButtonPanel";
    public const string PART_PageSizeSelector = "PART_PageSizeSelector";
    public const string PART_JumperInput = "PART_JumperInput";

    public static readonly StyledProperty<int> CurrentPageProperty =
        AvaloniaProperty.Register<Pagination, int>(nameof(CurrentPage), 1, defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<int> TotalCountProperty =
        AvaloniaProperty.Register<Pagination, int>(nameof(TotalCount));

    public static readonly StyledProperty<int> PageSizeProperty =
        AvaloniaProperty.Register<Pagination, int>(nameof(PageSize), 10);

    public static readonly DirectProperty<Pagination, int> PageCountProperty =
        AvaloniaProperty.RegisterDirect<Pagination, int>(nameof(PageCount), o => o.PageCount);

    public static readonly StyledProperty<ICommand?> CommandProperty =
        AvaloniaProperty.Register<Pagination, ICommand?>(nameof(Command));

    public static readonly StyledProperty<object?> CommandParameterProperty =
        AvaloniaProperty.Register<Pagination, object?>(nameof(CommandParameter));

    public static readonly StyledProperty<bool> ShowTotalProperty =
        AvaloniaProperty.Register<Pagination, bool>(nameof(ShowTotal));

    public static readonly StyledProperty<bool> ShowPageSizeSelectorProperty =
        AvaloniaProperty.Register<Pagination, bool>(nameof(ShowPageSizeSelector));

    public static readonly StyledProperty<IList<int>> PageSizeOptionsProperty =
        AvaloniaProperty.Register<Pagination, IList<int>>(nameof(PageSizeOptions),
            new AvaloniaList<int> { 10, 20, 50, 100 });

    public static readonly StyledProperty<bool> ShowJumperProperty =
        AvaloniaProperty.Register<Pagination, bool>(nameof(ShowJumper));

    public static readonly RoutedEvent<RoutedEventArgs> CurrentPageChangedEvent =
        RoutedEvent.Register<Pagination, RoutedEventArgs>(nameof(CurrentPageChanged), RoutingStrategies.Bubble);

    private Button? _nextButton;
    private int _pageCount = 1;
    private Button? _previousButton;
    private StackPanel? _buttonPanel;
    private ComboBox? _pageSizeSelector;
    private TextBox? _jumperInput;

    static Pagination()
    {
        CurrentPageProperty.Changed.AddClassHandler<Pagination>((x, _) => x.UpdateButtons());
        TotalCountProperty.Changed.AddClassHandler<Pagination>((x, _) => x.UpdateButtons());
        PageSizeProperty.Changed.AddClassHandler<Pagination>((x, _) => x.UpdateButtons());
    }

    public int CurrentPage
    {
        get => GetValue(CurrentPageProperty);
        set => SetValue(CurrentPageProperty, value);
    }

    public int TotalCount
    {
        get => GetValue(TotalCountProperty);
        set => SetValue(TotalCountProperty, value);
    }

    public int PageSize
    {
        get => GetValue(PageSizeProperty);
        set => SetValue(PageSizeProperty, value);
    }

    public int PageCount
    {
        get => _pageCount;
        private set => SetAndRaise(PageCountProperty, ref _pageCount, value);
    }

    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public bool ShowTotal
    {
        get => GetValue(ShowTotalProperty);
        set => SetValue(ShowTotalProperty, value);
    }

    public bool ShowPageSizeSelector
    {
        get => GetValue(ShowPageSizeSelectorProperty);
        set => SetValue(ShowPageSizeSelectorProperty, value);
    }

    public IList<int> PageSizeOptions
    {
        get => GetValue(PageSizeOptionsProperty);
        set => SetValue(PageSizeOptionsProperty, value);
    }

    public bool ShowJumper
    {
        get => GetValue(ShowJumperProperty);
        set => SetValue(ShowJumperProperty, value);
    }

    public event EventHandler<RoutedEventArgs>? CurrentPageChanged
    {
        add => AddHandler(CurrentPageChangedEvent, value);
        remove => RemoveHandler(CurrentPageChangedEvent, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        if (_previousButton is not null)
            _previousButton.Click -= OnNavigateButtonClick;
        if (_nextButton is not null)
            _nextButton.Click -= OnNavigateButtonClick;

        _previousButton = e.NameScope.Find<Button>(PART_PreviousButton);
        _nextButton = e.NameScope.Find<Button>(PART_NextButton);
        _buttonPanel = e.NameScope.Find<StackPanel>(PART_ButtonPanel);
        _pageSizeSelector = e.NameScope.Find<ComboBox>(PART_PageSizeSelector);
        _jumperInput = e.NameScope.Find<TextBox>(PART_JumperInput);

        if (_previousButton is not null)
            _previousButton.Click += OnNavigateButtonClick;
        if (_nextButton is not null)
            _nextButton.Click += OnNavigateButtonClick;
        if (_pageSizeSelector is not null)
        {
            _pageSizeSelector.ItemsSource = PageSizeOptions;
            _pageSizeSelector.SelectedItem = PageSize;
            _pageSizeSelector.SelectionChanged += OnPageSizeChanged;
        }
        if (_jumperInput is not null)
        {
            _jumperInput.KeyDown += OnJumperKeyDown;
        }

        UpdateButtons();
    }

    private void OnPageSizeChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_pageSizeSelector?.SelectedItem is int newSize && newSize != PageSize)
        {
            SetCurrentValue(PageSizeProperty, newSize);
            SetCurrentValue(CurrentPageProperty, 1);
            RaiseEvent(new RoutedEventArgs(CurrentPageChangedEvent));
            InvokeCommand();
        }
    }

    private void OnJumperKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && _jumperInput is not null)
        {
            if (int.TryParse(_jumperInput.Text, out var page))
            {
                page = Math.Clamp(page, 1, PageCount);
                if (page != CurrentPage)
                {
                    SetCurrentValue(CurrentPageProperty, page);
                    RaiseEvent(new RoutedEventArgs(CurrentPageChangedEvent));
                    InvokeCommand();
                }
                _jumperInput.Text = string.Empty;
            }
            e.Handled = true;
        }
    }

    private void OnNavigateButtonClick(object? sender, RoutedEventArgs e)
    {
        var delta = Equals(sender, _previousButton) ? -1 : 1;
        var next = Math.Clamp(CurrentPage + delta, 1, PageCount);
        if (next != CurrentPage)
        {
            SetCurrentValue(CurrentPageProperty, next);
            RaiseEvent(new RoutedEventArgs(CurrentPageChangedEvent));
            InvokeCommand();
        }
    }

    private void UpdateButtons()
    {
        var pageSize = Math.Max(PageSize, 1);
        var total = Math.Max(TotalCount, 0);
        var pageCount = total == 0 ? 1 : (total + pageSize - 1) / pageSize;
        PageCount = pageCount;

        var current = Math.Clamp(CurrentPage, 1, pageCount);
        if (current != CurrentPage)
        {
            SetCurrentValue(CurrentPageProperty, current);
        }

        if (_previousButton is not null)
        {
            _previousButton.IsEnabled = CurrentPage > 1;
        }

        if (_nextButton is not null)
        {
            _nextButton.IsEnabled = CurrentPage < PageCount;
        }

        RenderPageButtons();
    }

    private void RenderPageButtons()
    {
        if (_buttonPanel is null) return;

        _buttonPanel.Children.Clear();

        if (PageCount <= 7)
        {
            for (var i = 1; i <= PageCount; i++)
            {
                _buttonPanel.Children.Add(CreatePageButton(i, i == CurrentPage));
            }
        }
        else
        {
            _buttonPanel.Children.Add(CreatePageButton(1, CurrentPage == 1));

            if (CurrentPage > 4)
            {
                _buttonPanel.Children.Add(CreateEllipsisButton(-5));
            }

            var start = Math.Max(2, CurrentPage - 1);
            var end = Math.Min(PageCount - 1, CurrentPage + 1);

            for (var i = start; i <= end; i++)
            {
                _buttonPanel.Children.Add(CreatePageButton(i, i == CurrentPage));
            }

            if (CurrentPage < PageCount - 3)
            {
                _buttonPanel.Children.Add(CreateEllipsisButton(5));
            }

            _buttonPanel.Children.Add(CreatePageButton(PageCount, CurrentPage == PageCount));
        }
    }

    private Button CreatePageButton(int page, bool isSelected)
    {
        var button = new Button
        {
            Content = page.ToString(),
            MinWidth = 32,
            Height = 32,
            Padding = new Avalonia.Thickness(4, 0),
            HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            VerticalContentAlignment = Avalonia.Layout.VerticalAlignment.Center,
            CornerRadius = (CornerRadius)Application.Current!.FindResource("Luna.Radius.Default")!,
            FontSize = (double)Application.Current.FindResource("Luna.FontSize.Body.Medium")!,
        };

        if (isSelected)
        {
            button.Classes.Add("active");
        }

        button.Click += (_, _) =>
        {
            if (page != CurrentPage)
            {
                SetCurrentValue(CurrentPageProperty, page);
                RaiseEvent(new RoutedEventArgs(CurrentPageChangedEvent));
                InvokeCommand();
            }
        };

        return button;
    }

    private Button CreateEllipsisButton(int delta)
    {
        var button = new Button
        {
            Content = "...",
            MinWidth = 32,
            Height = 32,
            Padding = new Avalonia.Thickness(4, 0),
            HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            VerticalContentAlignment = Avalonia.Layout.VerticalAlignment.Center,
            CornerRadius = (CornerRadius)Application.Current!.FindResource("Luna.Radius.Default")!,
            FontSize = (double)Application.Current.FindResource("Luna.FontSize.Body.Medium")!,
        };

        button.Click += (_, _) =>
        {
            var next = Math.Clamp(CurrentPage + delta, 1, PageCount);
            if (next != CurrentPage)
            {
                SetCurrentValue(CurrentPageProperty, next);
                RaiseEvent(new RoutedEventArgs(CurrentPageChangedEvent));
                InvokeCommand();
            }
        };

        return button;
    }

    private void InvokeCommand()
    {
        if (Command is { } cmd && cmd.CanExecute(CommandParameter))
        {
            cmd.Execute(CommandParameter);
        }
    }
}

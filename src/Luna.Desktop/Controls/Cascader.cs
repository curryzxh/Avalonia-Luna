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

[PseudoClasses(":dropdown-open")]
public class Cascader : TemplatedControl
{
    public static readonly StyledProperty<string?> PlaceholderProperty =
        AvaloniaProperty.Register<Cascader, string?>(nameof(Placeholder), "请选择");

    public static readonly StyledProperty<string?> DisplayTextProperty =
        AvaloniaProperty.Register<Cascader, string?>(nameof(DisplayText));

    public static readonly StyledProperty<ObservableCollection<CascaderOption>> OptionsProperty =
        AvaloniaProperty.Register<Cascader, ObservableCollection<CascaderOption>>(nameof(Options));

    public static readonly StyledProperty<bool> IsDropDownOpenProperty =
        AvaloniaProperty.Register<Cascader, bool>(nameof(IsDropDownOpen), false);

    public static readonly StyledProperty<bool> IsClearableProperty =
        AvaloniaProperty.Register<Cascader, bool>(nameof(IsClearable), true);

    public static readonly StyledProperty<string?> SelectedValueProperty =
        AvaloniaProperty.Register<Cascader, string?>(nameof(SelectedValue));

    private Popup? _popup;
    private Panel? _columnsPanel;
    private readonly List<List<CascaderOption>> _columnData = [];
    private readonly List<CascaderOption> _selectedPath = [];

    public string? Placeholder
    {
        get => GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public string? DisplayText
    {
        get => GetValue(DisplayTextProperty);
        set => SetValue(DisplayTextProperty, value);
    }

    public ObservableCollection<CascaderOption> Options
    {
        get => GetValue(OptionsProperty);
        set => SetValue(OptionsProperty, value);
    }

    public bool IsDropDownOpen
    {
        get => GetValue(IsDropDownOpenProperty);
        set => SetValue(IsDropDownOpenProperty, value);
    }

    public bool IsClearable
    {
        get => GetValue(IsClearableProperty);
        set => SetValue(IsClearableProperty, value);
    }

    public string? SelectedValue
    {
        get => GetValue(SelectedValueProperty);
        set => SetValue(SelectedValueProperty, value);
    }

    static Cascader()
    {
        OptionsProperty.Changed.AddClassHandler<Cascader>(OnOptionsChanged);
        IsDropDownOpenProperty.Changed.AddClassHandler<Cascader>(OnIsDropDownOpenChanged);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _popup = this.FindDescendantOfType<Popup>();
        _columnsPanel = this.FindDescendantOfType<Panel>();

        if (_popup != null)
        {
            _popup.Opened += OnPopupOpened;
            _popup.Closed += OnPopupClosed;
        }

        RebuildColumns();
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        IsDropDownOpen = !IsDropDownOpen;
    }

    private static void OnOptionsChanged(Cascader sender, AvaloniaPropertyChangedEventArgs e)
    {
        sender.RebuildColumns();
    }

    private static void OnIsDropDownOpenChanged(Cascader sender, AvaloniaPropertyChangedEventArgs e)
    {
        sender.PseudoClasses.Set(":dropdown-open", sender.IsDropDownOpen);
    }

    private void OnPopupOpened(object? sender, EventArgs e)
    {
        IsDropDownOpen = true;
    }

    private void OnPopupClosed(object? sender, EventArgs e)
    {
        IsDropDownOpen = false;
    }

    private void RebuildColumns()
    {
        if (_columnsPanel == null) return;
        _columnsPanel.Children.Clear();
        _columnData.Clear();
        _selectedPath.Clear();

        if (Options == null || Options.Count == 0) return;

        _columnData.Add(Options.ToList());
        RenderColumns();
    }

    private void RenderColumns()
    {
        if (_columnsPanel == null) return;
        _columnsPanel.Children.Clear();

        for (var i = 0; i < _columnData.Count; i++)
        {
            var columnIndex = i;
            var listBox = new ListBox
            {
                Width = 180,
                MaxHeight = 300,
                ItemsSource = _columnData[columnIndex],
                DisplayMemberBinding = new Avalonia.Data.Binding("Label"),
            };

            if (_selectedPath.Count > columnIndex)
            {
                listBox.SelectedItem = _selectedPath[columnIndex];
            }

            var col = columnIndex;
            listBox.SelectionChanged += (s, e) =>
            {
                OnColumnItemSelected(col, listBox);
            };

            _columnsPanel.Children.Add(listBox);
        }
    }

    private void OnColumnItemSelected(int columnIndex, ListBox listBox)
    {
        if (listBox.SelectedItem is not CascaderOption option) return;

        while (_selectedPath.Count > columnIndex)
        {
            _selectedPath.RemoveAt(_selectedPath.Count - 1);
        }
        _selectedPath.Add(option);

        while (_columnData.Count > columnIndex + 1)
        {
            _columnData.RemoveAt(_columnData.Count - 1);
        }

        if (option.Children != null && option.Children.Count > 0)
        {
            _columnData.Add(option.Children.ToList());
            RenderColumns();
        }
        else
        {
            var pathText = string.Join(" / ", _selectedPath.Select(p => p.Label));
            DisplayText = pathText;
            SelectedValue = option.Value;
            IsDropDownOpen = false;
            RenderColumns();
        }
    }

    public void Clear()
    {
        _selectedPath.Clear();
        DisplayText = null;
        SelectedValue = null;
        RebuildColumns();
    }
}

public class CascaderOption
{
    public string Label { get; set; } = string.Empty;
    public string? Value { get; set; }
    public ObservableCollection<CascaderOption>? Children { get; set; }
}

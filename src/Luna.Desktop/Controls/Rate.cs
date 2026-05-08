using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;

namespace Luna.Desktop.Controls;

[PseudoClasses(":disabled")]
public class Rate : TemplatedControl
{
    public static readonly StyledProperty<int> ValueProperty =
        AvaloniaProperty.Register<Rate, int>(nameof(Value), 0, defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<int> CountProperty =
        AvaloniaProperty.Register<Rate, int>(nameof(Count), 5);

    public static readonly StyledProperty<string> CharacterProperty =
        AvaloniaProperty.Register<Rate, string>(nameof(Character), "★");

    public static readonly StyledProperty<double> IconSizeProperty =
        AvaloniaProperty.Register<Rate, double>(nameof(IconSize), 24);

    public static readonly StyledProperty<bool> AllowClearProperty =
        AvaloniaProperty.Register<Rate, bool>(nameof(AllowClear), true);

    public static readonly StyledProperty<bool> IsReadOnlyProperty =
        AvaloniaProperty.Register<Rate, bool>(nameof(IsReadOnly), false);

    public static readonly StyledProperty<int> HoverIndexProperty =
        AvaloniaProperty.Register<Rate, int>(nameof(HoverIndex), -1);

    public static readonly StyledProperty<ObservableCollection<RateItem>> ItemsProperty =
        AvaloniaProperty.Register<Rate, ObservableCollection<RateItem>>(nameof(Items));

    private bool _updating;

    public int Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public int Count
    {
        get => GetValue(CountProperty);
        set => SetValue(CountProperty, value);
    }

    public string Character
    {
        get => GetValue(CharacterProperty);
        set => SetValue(CharacterProperty, value);
    }

    public double IconSize
    {
        get => GetValue(IconSizeProperty);
        set => SetValue(IconSizeProperty, value);
    }

    public bool AllowClear
    {
        get => GetValue(AllowClearProperty);
        set => SetValue(AllowClearProperty, value);
    }

    public bool IsReadOnly
    {
        get => GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    public int HoverIndex
    {
        get => GetValue(HoverIndexProperty);
        set => SetValue(HoverIndexProperty, value);
    }

    public ObservableCollection<RateItem> Items
    {
        get => GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    static Rate()
    {
        ValueProperty.Changed.AddClassHandler<Rate>(OnValueChanged);
        CountProperty.Changed.AddClassHandler<Rate>(OnCountChanged);
        CharacterProperty.Changed.AddClassHandler<Rate>(OnCharacterChanged);
        IsReadOnlyProperty.Changed.AddClassHandler<Rate>(OnReadOnlyChanged);
    }

    public Rate()
    {
        Items = new ObservableCollection<RateItem>();
        RebuildItems();
    }

    private static void OnValueChanged(Rate sender, AvaloniaPropertyChangedEventArgs e)
    {
        sender.UpdateItemStates();
    }

    private static void OnCountChanged(Rate sender, AvaloniaPropertyChangedEventArgs e)
    {
        sender.RebuildItems();
    }

    private static void OnCharacterChanged(Rate sender, AvaloniaPropertyChangedEventArgs e)
    {
        sender.UpdateItemCharacters();
    }

    private static void OnReadOnlyChanged(Rate sender, AvaloniaPropertyChangedEventArgs e)
    {
        sender.PseudoClasses.Set(":disabled", sender.IsReadOnly);
    }

    private void RebuildItems()
    {
        _updating = true;
        Items.Clear();
        for (var i = 0; i < Count; i++)
        {
            Items.Add(new RateItem(i, Character, i < Value));
        }
        _updating = false;
    }

    private void UpdateItemStates()
    {
        if (_updating) return;
        _updating = true;
        for (var i = 0; i < Items.Count; i++)
        {
            Items[i].IsFilled = i < Value;
        }
        _updating = false;
    }

    private void UpdateItemCharacters()
    {
        foreach (var item in Items)
        {
            item.Character = Character;
        }
    }

    internal void OnItemPointerPressed(int index)
    {
        if (IsReadOnly) return;

        if (AllowClear && Value == index + 1)
        {
            Value = 0;
        }
        else
        {
            Value = index + 1;
        }
    }

    internal void OnItemPointerEntered(int index)
    {
        if (IsReadOnly) return;
        HoverIndex = index;
    }

    internal void OnItemPointerExited()
    {
        if (IsReadOnly) return;
        HoverIndex = -1;
    }
}

public class RateItem : AvaloniaObject
{
    public static readonly StyledProperty<int> IndexProperty =
        AvaloniaProperty.Register<RateItem, int>(nameof(Index));

    public static readonly StyledProperty<string> CharacterProperty =
        AvaloniaProperty.Register<RateItem, string>(nameof(Character), "★");

    public static readonly StyledProperty<bool> IsFilledProperty =
        AvaloniaProperty.Register<RateItem, bool>(nameof(IsFilled));

    public RateItem(int index, string character, bool isFilled)
    {
        Index = index;
        Character = character;
        IsFilled = isFilled;
    }

    public int Index
    {
        get => GetValue(IndexProperty);
        set => SetValue(IndexProperty, value);
    }

    public string Character
    {
        get => GetValue(CharacterProperty);
        set => SetValue(CharacterProperty, value);
    }

    public bool IsFilled
    {
        get => GetValue(IsFilledProperty);
        set => SetValue(IsFilledProperty, value);
    }
}

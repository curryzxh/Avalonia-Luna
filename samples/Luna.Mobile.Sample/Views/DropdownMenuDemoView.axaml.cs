using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Luna.Mobile.Controls;
using Luna.Mobile.Sample.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Luna.Mobile.Sample.Views;

public partial class DropdownMenuDemoView : UserControl
{
    private static readonly string[] ChineseNumbers = ["一", "二", "三", "四", "五", "六", "七", "八", "九", "十"];

    private DropdownMenuDemoViewModel ViewModel { get; } = new();

    public DropdownMenuDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;

        SingleDemoHost.Content = BuildSingleDemo();
        MultipleDemoHost.Content = BuildMultipleDemo();
        DirectionDemoHost.Content = BuildDirectionDemo();
        DisabledDemoHost.Content = BuildDisabledDemo();
        CustomizedDemoHost.Content = BuildCustomizedDemo();
    }

    private static DropdownMenu BuildSingleDemo()
    {
        var productItem = new DropdownMenuItem
        {
            Value = "all",
        };
        productItem.Options.AddRange([
            new DropdownMenuOption { Value = "all", Label = "全部产品" },
            new DropdownMenuOption { Value = "new", Label = "最新产品" },
            new DropdownMenuOption { Value = "hot", Label = "最火产品" },
            new DropdownMenuOption { Value = "disabled", Label = "禁用选项", IsDisabled = true },
        ]);

        var sorterItem = new DropdownMenuItem
        {
            Value = "default",
        };
        sorterItem.Options.AddRange([
            new DropdownMenuOption { Value = "default", Label = "默认排序" },
            new DropdownMenuOption { Value = "price", Label = "价格从高到低" },
        ]);

        return new DropdownMenu
        {
            Items =
            {
                productItem,
                sorterItem,
            },
        };
    }

    private static DropdownMenu BuildMultipleDemo()
    {
        var menu = new DropdownMenu();

        menu.Items.Add(CreateMultipleItem("单列多选", 1, 8, ["option_1"]));
        menu.Items.Add(CreateMultipleItem("双列多选", 2, 9, ["option_1", "option_2"]));
        menu.Items.Add(CreateMultipleItem("三列多选", 3, 10, ["option_1", "option_2", "option_3"]));

        return menu;
    }

    private static DropdownMenu BuildDirectionDemo()
    {
        var menu = BuildSingleDemo();
        menu.Direction = DropdownMenuDirection.Up;
        return menu;
    }

    private static DropdownMenu BuildDisabledDemo()
    {
        return new DropdownMenu
        {
            Items =
            {
                new DropdownMenuItem { Label = "禁用菜单", IsDisabled = true },
                new DropdownMenuItem { Label = "禁用菜单", IsDisabled = true },
            },
        };
    }

    private static DropdownMenu BuildCustomizedDemo()
    {
        var menu = new DropdownMenu();
        var item = new DropdownMenuItem
        {
            Label = "三列多选",
            Multiple = true,
            OptionsColumns = 1,
        };

        var group1 = CreateTagGroup("类型", "type", ["type_1", "type_2"]);
        var group2 = CreateTagGroup("角色", "role", ["role_2"]);

        var content = new StackPanel();
        content.Children.Add(group1.host);
        content.Children.Add(group2.host);
        item.Content = content;

        var resetButton = new Button
        {
            Content = "重置",
            Classes = { "light" },
            CornerRadius = new CornerRadius(0),
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };
        resetButton.Click += (_, _) =>
        {
            foreach (var checkBox in group1.checkBoxes.Concat(group2.checkBoxes))
            {
                checkBox.IsChecked = false;
            }
        };

        var confirmButton = new Button
        {
            Content = "确定",
            Classes = { "primary" },
            CornerRadius = new CornerRadius(0),
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };
        confirmButton.Click += (_, _) => menu.CollapseMenu();

        item.Footer = new Grid
        {
            ColumnDefinitions = new ColumnDefinitions("*,*"),
            Children =
            {
                resetButton,
                confirmButton,
            },
        };
        Grid.SetColumn(confirmButton, 1);

        menu.Items.Add(item);
        return menu;
    }

    private static DropdownMenuItem CreateMultipleItem(string label, int columns, int optionCount, IReadOnlyList<string> selectedValues)
    {
        var item = new DropdownMenuItem
        {
            Label = label,
            Multiple = true,
            OptionsColumns = columns,
            Value = selectedValues.ToArray(),
        };

        for (var i = 0; i < optionCount; i++)
        {
            item.Options.Add(new DropdownMenuOption
            {
                Label = $"选项{ChineseNumbers[i]}",
                Value = $"option_{i + 1}",
            });
        }

        item.Options.Add(new DropdownMenuOption
        {
            Label = "禁用选项",
            Value = "disabled",
            IsDisabled = true,
        });

        return item;
    }

    private static (StackPanel host, List<CheckBox> checkBoxes) CreateTagGroup(string title, string prefix, IReadOnlyList<string> selected)
    {
        var host = new StackPanel();
        host.Children.Add(new TextBlock
        {
            Text = title,
            Margin = new Thickness(16, 16, 16, 8),
            FontSize = 14,
            Foreground = new SolidColorBrush(Color.Parse("#1F1F1F")),
        });

        var grid = new UniformGrid
        {
            Columns = 3,
            Margin = new Thickness(16, 0, 16, 12),
        };

        var checkBoxes = new List<CheckBox>();
        for (var i = 0; i < 8; i++)
        {
            var value = $"{prefix}_{i + 1}";
            var checkBox = new CheckBox
            {
                Content = $"{title}{ChineseNumbers[i]}",
                IsChecked = selected.Contains(value),
                Margin = new Thickness(0, 0, 8, 8),
            };
            checkBoxes.Add(checkBox);
            grid.Children.Add(checkBox);
        }

        var disabledBox = new CheckBox
        {
            Content = "禁用选项",
            IsEnabled = false,
            Margin = new Thickness(0, 0, 8, 8),
        };
        checkBoxes.Add(disabledBox);
        grid.Children.Add(disabledBox);

        host.Children.Add(grid);
        return (host, checkBoxes);
    }
}

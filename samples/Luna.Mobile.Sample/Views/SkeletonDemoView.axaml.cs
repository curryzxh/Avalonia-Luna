using Avalonia.Controls;
using Luna.Mobile.Controls;
using System;

namespace Luna.Mobile.Sample.Views;

public partial class SkeletonDemoView : UserControl
{
    public SkeletonDemoView()
    {
        InitializeComponent();
        ConfigureSamples();
    }

    private void ConfigureSamples()
    {
        CellAvatarSkeleton.Rows = CreateCellGroupRows(circleAvatar: true);
        CellImageSkeleton.Rows = CreateCellGroupRows(circleAvatar: false);
        GridSkeleton.Rows = CreateGridRows();
        ImageGroupLeftSkeleton.Rows = CreateImageGroupRows();
        ImageGroupRightSkeleton.Rows = CreateImageGroupRows();
    }

    private static SkeletonRowDefinition[] CreateCellGroupRows(bool circleAvatar)
    {
        return
        [
            new SkeletonRowDefinition
            {
                Spacing = 16,
                Blocks =
                [
                    new SkeletonBlockDefinition
                    {
                        Height = 48,
                        Width = 48,
                        Shape = circleAvatar ? SkeletonShape.Circle : SkeletonShape.Rect,
                        CornerRadius = circleAvatar ? default : new Avalonia.CornerRadius(6),
                    },
                    new SkeletonBlockDefinition
                    {
                        WidthFactor = 0.5,
                        Height = 16,
                    },
                ],
            },
            new SkeletonRowDefinition
            {
                Indent = 64,
                Blocks =
                [
                    new SkeletonBlockDefinition
                    {
                        WidthFactor = 1,
                        Height = 16,
                    },
                ],
            },
        ];
    }

    private static SkeletonRowDefinition[] CreateGridRows()
    {
        var iconRow = CreateGridBlockRow(48, 48, 6);
        var textRow = CreateGridBlockRow(48, 16, 3);

        return
        [
            iconRow,
            textRow,
        ];
    }

    private static SkeletonRowDefinition CreateGridBlockRow(double width, double height, double radius)
    {
        return new SkeletonRowDefinition
        {
            Spacing = 10,
            Blocks =
            [
                CreateRectBlock(width, height, radius),
                CreateRectBlock(width, height, radius),
                CreateRectBlock(width, height, radius),
                CreateRectBlock(width, height, radius),
                CreateRectBlock(width, height, radius),
            ],
        };
    }

    private static SkeletonRowDefinition[] CreateImageGroupRows()
    {
        return
        [
            new SkeletonRowDefinition
            {
                Blocks =
                [
                    CreateRectBlock(double.NaN, 163.5, 12, widthFactor: 1),
                ],
            },
            new SkeletonRowDefinition
            {
                Blocks =
                [
                    new SkeletonBlockDefinition
                    {
                        WidthFactor = 1,
                        Height = 16,
                    },
                ],
            },
            new SkeletonRowDefinition
            {
                Blocks =
                [
                    new SkeletonBlockDefinition
                    {
                        WidthFactor = 0.61,
                        Height = 16,
                    },
                ],
            },
        ];
    }

    private static SkeletonBlockDefinition CreateRectBlock(double width, double height, double radius, double widthFactor = 0)
    {
        return new SkeletonBlockDefinition
        {
            Width = width,
            WidthFactor = widthFactor,
            Height = height,
            Shape = SkeletonShape.Rect,
            CornerRadius = new Avalonia.CornerRadius(radius),
        };
    }
}

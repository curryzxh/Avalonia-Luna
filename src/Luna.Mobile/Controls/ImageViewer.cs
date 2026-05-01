using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;

namespace Luna.Mobile.Controls;

/// <summary>
/// 图片预览控件，支持弹出查看、索引切换与删除请求。
/// </summary>
public sealed class ImageViewer : TemplatedControl
{
    private Popup? _popup;
    private Border? _overlay;
    private Button? _closeButton;
    private Button? _prevButton;
    private Button? _nextButton;
    private Button? _deleteButton;
    private ContentControl? _imageContainer;
    private TextBlock? _indexText;

    /// <inheritdoc cref="Images" />
    public static readonly StyledProperty<IList<object>> ImagesProperty =
        AvaloniaProperty.Register<ImageViewer, IList<object>>(nameof(Images));

    /// <inheritdoc cref="CurrentIndex" />
    public static readonly StyledProperty<int> CurrentIndexProperty =
        AvaloniaProperty.Register<ImageViewer, int>(nameof(CurrentIndex));

    /// <inheritdoc cref="IsOpen" />
    public static readonly StyledProperty<bool> IsOpenProperty =
        AvaloniaProperty.Register<ImageViewer, bool>(nameof(IsOpen));

    /// <inheritdoc cref="ShowIndex" />
    public static readonly StyledProperty<bool> ShowIndexProperty =
        AvaloniaProperty.Register<ImageViewer, bool>(nameof(ShowIndex));

    /// <inheritdoc cref="ShowDelete" />
    public static readonly StyledProperty<bool> ShowDeleteProperty =
        AvaloniaProperty.Register<ImageViewer, bool>(nameof(ShowDelete));

    /// <inheritdoc cref="Closed" />
    public static readonly RoutedEvent<RoutedEventArgs> ClosedEvent =
        RoutedEvent.Register<ImageViewer, RoutedEventArgs>(nameof(Closed), RoutingStrategies.Bubble);

    /// <inheritdoc cref="DeleteRequested" />
    public static readonly RoutedEvent<RoutedEventArgs> DeleteRequestedEvent =
        RoutedEvent.Register<ImageViewer, RoutedEventArgs>(nameof(DeleteRequested), RoutingStrategies.Bubble);

    static ImageViewer()
    {
        IsOpenProperty.Changed.AddClassHandler<ImageViewer>((control, args) =>
        {
            control.PseudoClasses.Set(":open", args.GetNewValue<bool>());
            control.SyncPopupState();
        });
        CurrentIndexProperty.Changed.AddClassHandler<ImageViewer>((control, _) => control.UpdateDisplay());
        ImagesProperty.Changed.AddClassHandler<ImageViewer>((control, _) => control.UpdateDisplay());
        ShowIndexProperty.Changed.AddClassHandler<ImageViewer>((control, _) =>
            control.PseudoClasses.Set(":index", control.ShowIndex));
        ShowDeleteProperty.Changed.AddClassHandler<ImageViewer>((control, _) =>
            control.PseudoClasses.Set(":delete", control.ShowDelete));
    }

    /// <summary>
    /// 初始化 <see cref="ImageViewer" /> 的新实例。
    /// </summary>
    public ImageViewer()
    {
        Images = new AvaloniaList<object>();
        PseudoClasses.Set(":index", ShowIndex);
        PseudoClasses.Set(":delete", ShowDelete);
    }

    /// <summary>
    /// 获取或设置可预览的图片项集合。
    /// </summary>
    public IList<object> Images
    {
        get => GetValue(ImagesProperty);
        set => SetValue(ImagesProperty, value);
    }

    /// <summary>
    /// 获取或设置当前显示的图片索引。
    /// </summary>
    public int CurrentIndex
    {
        get => GetValue(CurrentIndexProperty);
        set => SetValue(CurrentIndexProperty, value);
    }

    /// <summary>
    /// 获取或设置预览弹层是否打开。
    /// </summary>
    public bool IsOpen
    {
        get => GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    /// <summary>
    /// 获取或设置是否显示索引指示。
    /// </summary>
    public bool ShowIndex
    {
        get => GetValue(ShowIndexProperty);
        set => SetValue(ShowIndexProperty, value);
    }

    /// <summary>
    /// 获取或设置是否显示删除按钮。
    /// </summary>
    public bool ShowDelete
    {
        get => GetValue(ShowDeleteProperty);
        set => SetValue(ShowDeleteProperty, value);
    }

    /// <summary>
    /// 预览关闭时触发。
    /// </summary>
    public event EventHandler<RoutedEventArgs> Closed
    {
        add => AddHandler(ClosedEvent, value);
        remove => RemoveHandler(ClosedEvent, value);
    }

    /// <summary>
    /// 用户请求删除当前图片时触发。
    /// </summary>
    public event EventHandler<RoutedEventArgs> DeleteRequested
    {
        add => AddHandler(DeleteRequestedEvent, value);
        remove => RemoveHandler(DeleteRequestedEvent, value);
    }

    /// <inheritdoc />
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        if (_overlay is not null)
            _overlay.PointerPressed -= OnOverlayPressed;
        if (_closeButton is not null)
            _closeButton.Click -= OnCloseClick;
        if (_prevButton is not null)
            _prevButton.Click -= OnPrevClick;
        if (_nextButton is not null)
            _nextButton.Click -= OnNextClick;
        if (_deleteButton is not null)
            _deleteButton.Click -= OnDeleteClick;

        _popup = e.NameScope.Find<Popup>("PART_Popup");
        _overlay = e.NameScope.Find<Border>("PART_Overlay");
        _closeButton = e.NameScope.Find<Button>("PART_CloseButton");
        _prevButton = e.NameScope.Find<Button>("PART_PrevButton");
        _nextButton = e.NameScope.Find<Button>("PART_NextButton");
        _deleteButton = e.NameScope.Find<Button>("PART_DeleteButton");
        _imageContainer = e.NameScope.Find<ContentControl>("PART_ImageContainer");
        _indexText = e.NameScope.Find<TextBlock>("PART_IndexText");

        if (_overlay is not null)
            _overlay.PointerPressed += OnOverlayPressed;
        if (_closeButton is not null)
            _closeButton.Click += OnCloseClick;
        if (_prevButton is not null)
            _prevButton.Click += OnPrevClick;
        if (_nextButton is not null)
            _nextButton.Click += OnNextClick;
        if (_deleteButton is not null)
            _deleteButton.Click += OnDeleteClick;

        UpdateDisplay();
    }

    private void OnOverlayPressed(object? sender, PointerPressedEventArgs e)
    {
        Close();
    }

    private void OnCloseClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private void OnPrevClick(object? sender, RoutedEventArgs e)
    {
        if (CurrentIndex > 0)
            CurrentIndex--;
    }

    private void OnNextClick(object? sender, RoutedEventArgs e)
    {
        if (Images is not null && CurrentIndex < Images.Count - 1)
            CurrentIndex++;
    }

    private void OnDeleteClick(object? sender, RoutedEventArgs e)
    {
        RaiseEvent(new RoutedEventArgs(DeleteRequestedEvent));
    }

    private void Close()
    {
        IsOpen = false;
        RaiseEvent(new RoutedEventArgs(ClosedEvent));
    }

    private void SyncPopupState()
    {
        if (_popup is null)
            return;
        _popup.IsOpen = IsOpen;
    }

    private void UpdateDisplay()
    {
        if (_imageContainer is null)
            return;

        var images = Images;
        if (images is null || images.Count == 0)
        {
            _imageContainer.Content = null;
            return;
        }

        var idx = CurrentIndex;
        if (idx < 0 || idx >= images.Count)
        {
            _imageContainer.Content = null;
            return;
        }

        var item = images[idx];
        if (item is Avalonia.Media.IImage img)
        {
            _imageContainer.Content = new Image { Source = img, Stretch = Avalonia.Media.Stretch.Uniform };
        }
        else if (item is Uri uri)
        {
            try
            {
                var bitmap = new Avalonia.Media.Imaging.Bitmap(uri.AbsoluteUri);
                _imageContainer.Content = new Image { Source = bitmap, Stretch = Avalonia.Media.Stretch.Uniform };
            }
            catch
            {
                _imageContainer.Content = CreatePlaceholder(idx);
            }
        }
        else if (item is string s)
        {
            try
            {
                var bitmap = new Avalonia.Media.Imaging.Bitmap(s);
                _imageContainer.Content = new Image { Source = bitmap, Stretch = Avalonia.Media.Stretch.Uniform };
            }
            catch
            {
                var textBlock = new TextBlock
                {
                    Text = s,
                    Foreground = Avalonia.Media.Brushes.White,
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                };
                _imageContainer.Content = textBlock;
            }
        }
        else if (item is Avalonia.Controls.Control control)
        {
            _imageContainer.Content = control;
        }
        else
        {
            _imageContainer.Content = CreatePlaceholder(idx);
        }

        if (_indexText is not null && ShowIndex)
        {
            _indexText.Text = $"{idx + 1} / {images.Count}";
        }

        if (_prevButton is not null)
            _prevButton.IsVisible = CurrentIndex > 0;
        if (_nextButton is not null)
            _nextButton.IsVisible = images is not null && CurrentIndex < images.Count - 1;
    }

    private static Border CreatePlaceholder(int index)
    {
        var colors = new[]
        {
            Avalonia.Media.Color.FromRgb(100, 149, 237),
            Avalonia.Media.Color.FromRgb(255, 127, 80),
            Avalonia.Media.Color.FromRgb(60, 179, 113),
        };
        var color = colors[index % colors.Length];
        return new Border
        {
            Background = new Avalonia.Media.SolidColorBrush(color),
            Width = 300,
            Height = 400,
            CornerRadius = new CornerRadius(8),
            Child = new TextBlock
            {
                Text = $"图片 {index + 1}",
                Foreground = Avalonia.Media.Brushes.White,
                FontSize = 24,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            },
        };
    }

    /// <summary>
    /// 打开图片预览。
    /// </summary>
    public void Show()
    {
        IsOpen = true;
    }

    /// <summary>
    /// 打开图片预览并切换到指定索引。
    /// </summary>
    /// <param name="index">要显示的图片索引。</param>
    public void Show(int index)
    {
        CurrentIndex = index;
        IsOpen = true;
    }
}

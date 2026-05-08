using System.Collections;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace Luna.Desktop.Controls;

public class Comment : TemplatedControl
{
    public static readonly StyledProperty<string?> AuthorProperty =
        AvaloniaProperty.Register<Comment, string?>(nameof(Author));

    public static readonly StyledProperty<string?> AvatarTextProperty =
        AvaloniaProperty.Register<Comment, string?>(nameof(AvatarText));

    public static readonly StyledProperty<IImage?> AvatarSourceProperty =
        AvaloniaProperty.Register<Comment, IImage?>(nameof(AvatarSource));

    public static readonly StyledProperty<string?> ContentProperty =
        AvaloniaProperty.Register<Comment, string?>(nameof(Content));

    public static readonly StyledProperty<string?> TimestampProperty =
        AvaloniaProperty.Register<Comment, string?>(nameof(Timestamp));

    public static readonly StyledProperty<IEnumerable?> RepliesProperty =
        AvaloniaProperty.Register<Comment, IEnumerable?>(nameof(Replies));

    public static readonly StyledProperty<object?> ActionsProperty =
        AvaloniaProperty.Register<Comment, object?>(nameof(Actions));

    public string? Author
    {
        get => GetValue(AuthorProperty);
        set => SetValue(AuthorProperty, value);
    }

    public string? AvatarText
    {
        get => GetValue(AvatarTextProperty);
        set => SetValue(AvatarTextProperty, value);
    }

    public IImage? AvatarSource
    {
        get => GetValue(AvatarSourceProperty);
        set => SetValue(AvatarSourceProperty, value);
    }

    public string? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public string? Timestamp
    {
        get => GetValue(TimestampProperty);
        set => SetValue(TimestampProperty, value);
    }

    public IEnumerable? Replies
    {
        get => GetValue(RepliesProperty);
        set => SetValue(RepliesProperty, value);
    }

    public object? Actions
    {
        get => GetValue(ActionsProperty);
        set => SetValue(ActionsProperty, value);
    }
}

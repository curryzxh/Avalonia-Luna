using Avalonia;
using Avalonia.Controls.Primitives;

namespace Luna.Desktop.Controls;

public class Comment : TemplatedControl
{
    public static readonly StyledProperty<string?> AuthorProperty =
        AvaloniaProperty.Register<Comment, string?>(nameof(Author));

    public static readonly StyledProperty<string?> AvatarTextProperty =
        AvaloniaProperty.Register<Comment, string?>(nameof(AvatarText));

    public static readonly StyledProperty<string?> ContentProperty =
        AvaloniaProperty.Register<Comment, string?>(nameof(Content));

    public static readonly StyledProperty<string?> TimestampProperty =
        AvaloniaProperty.Register<Comment, string?>(nameof(Timestamp));

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
}

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Threading;
using Avalonia.VisualTree;
using System;
using System.Collections.Generic;

namespace Luna.Mobile.Controls;

public sealed class MessageHost : TemplatedControl
{
    private const string ItemsHostPartName = "PART_ItemsHost";

    private static MessageHost? _current;

    private Panel? _itemsHost;
    private readonly List<MessageEntry> _entries = [];

    public static MessageHost? Current => _current;

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _current = this;
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        if (ReferenceEquals(_current, this))
        {
            _current = null;
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _itemsHost = e.NameScope.Find<Panel>(ItemsHostPartName);
        RenderEntries();
    }

    public void Show(MessageOptions options)
    {
        var entry = new MessageEntry(Guid.NewGuid(), options);
        _entries.Add(entry);
        RenderEntries();

        if (options.Duration > TimeSpan.Zero)
        {
            entry.Timer = new DispatcherTimer
            {
                Interval = options.Duration,
            };
            entry.Timer.Tick += (_, _) =>
            {
                entry.Timer?.Stop();
                Close(entry.Id);
            };
            entry.Timer.Start();
        }
    }

    public void CloseAll()
    {
        foreach (var entry in _entries)
        {
            entry.Timer?.Stop();
        }

        _entries.Clear();
        RenderEntries();
    }

    private void Close(Guid id)
    {
        var index = _entries.FindIndex(x => x.Id == id);
        if (index < 0)
        {
            return;
        }

        _entries[index].Timer?.Stop();
        _entries.RemoveAt(index);
        RenderEntries();
    }

    private void RenderEntries()
    {
        if (_itemsHost is null)
        {
            return;
        }

        _itemsHost.Children.Clear();

        for (var i = 0; i < _entries.Count; i++)
        {
            var entry = _entries[i];
            _itemsHost.Children.Add(CreateMessageView(entry));
        }
    }

    private Control CreateMessageView(MessageEntry entry)
    {
        var options = entry.Options;
        var card = new MessageCard
        {
            Theme = options.Theme,
            Content = options.Content,
            ShowIcon = options.ShowIcon,
            CloseBtn = options.CloseBtn,
            Link = options.Link,
            Marquee = options.Marquee,
        };
        card.CloseRequested += (_, _) => Close(entry.Id);
        return card;
    }

    private sealed class MessageEntry
    {
        public MessageEntry(Guid id, MessageOptions options)
        {
            Id = id;
            Options = options;
        }

        public Guid Id { get; }
        public MessageOptions Options { get; }
        public DispatcherTimer? Timer { get; set; }
    }
}

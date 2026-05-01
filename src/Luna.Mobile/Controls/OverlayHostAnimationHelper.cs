using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Media;
using Avalonia.Styling;
using System;

namespace Luna.Mobile.Controls;

internal static class OverlayHostAnimationHelper
{
    internal static readonly TimeSpan DefaultDuration = TimeSpan.FromMilliseconds(200);

    internal static Animation CreateOpacityAnimation(bool appear, double visibleOpacity = 1d)
    {
        var animation = new Animation
        {
            Duration = DefaultDuration,
            FillMode = FillMode.Forward,
        };

        var from = new KeyFrame { Cue = new Cue(0d) };
        from.Setters.Add(new Setter(Visual.OpacityProperty, appear ? 0d : visibleOpacity));

        var to = new KeyFrame { Cue = new Cue(1d) };
        to.Setters.Add(new Setter(Visual.OpacityProperty, appear ? visibleOpacity : 0d));

        animation.Children.Add(from);
        animation.Children.Add(to);
        return animation;
    }

    internal static Animation CreateSlideAnimation(bool appear, AvaloniaProperty<double> property, double offset)
    {
        var animation = new Animation
        {
            Duration = DefaultDuration,
            FillMode = FillMode.Forward,
            Easing = appear ? new CubicEaseOut() : new CubicEaseIn(),
        };

        var from = new KeyFrame { Cue = new Cue(0d) };
        from.Setters.Add(new Setter(Visual.OpacityProperty, appear ? 0d : 1d));
        from.Setters.Add(new Setter(property, appear ? offset : 0d));

        var to = new KeyFrame { Cue = new Cue(1d) };
        to.Setters.Add(new Setter(Visual.OpacityProperty, appear ? 1d : 0d));
        to.Setters.Add(new Setter(property, appear ? 0d : offset));

        animation.Children.Add(from);
        animation.Children.Add(to);
        return animation;
    }

    internal static TranslateTransform EnsureTranslateTransform(Visual visual)
    {
        if (visual.RenderTransform is TranslateTransform transform)
        {
            return transform;
        }

        transform = new TranslateTransform();
        visual.RenderTransform = transform;
        return transform;
    }

    internal static double ResolveDistance(double value, double fallback)
    {
        if (double.IsNaN(value) || value <= 0)
        {
            return fallback;
        }

        return value;
    }
}

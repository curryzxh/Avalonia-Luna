using System.Collections.ObjectModel;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Luna.Mobile.Controls;

namespace Luna.Mobile.Sample.Views;

public partial class StepsDemoView : UserControl
{
    private const int SpecialStepsMaxCount = 6;
    private readonly ObservableCollection<StepItem> _specialInteractiveItems = [];
    private int _specialCurrentIndex = 3;
    private int _specialStepCount = 4;

    public StepsDemoView()
    {
        InitializeComponent();
        SpecialInteractiveSteps.ItemsSource = _specialInteractiveItems;
        RebuildSpecialInteractiveSteps();
    }

    private void OnSpecialNextStepClick(object? sender, RoutedEventArgs e)
    {
        if (_specialStepCount < SpecialStepsMaxCount)
        {
            _specialCurrentIndex = _specialStepCount;
            _specialStepCount++;
        }
        else if (_specialCurrentIndex < _specialStepCount - 1)
        {
            _specialCurrentIndex++;
        }

        RebuildSpecialInteractiveSteps();
    }

    private void RebuildSpecialInteractiveSteps()
    {
        _specialInteractiveItems.Clear();

        for (var index = 0; index < _specialStepCount; index++)
        {
            _specialInteractiveItems.Add(new StepItem
            {
                Title = $"步骤 {index + 1}",
                TitleRight = ">",
            });
        }

        SpecialInteractiveSteps.Current = _specialCurrentIndex.ToString(CultureInfo.InvariantCulture);
    }
}

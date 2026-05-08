using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class ColorPickerSampleViewModel : SampleDetailViewModelBase
{
    public ColorPickerSampleViewModel() : base("输入", "ColorPicker", "颜色选择器，支持预设色板和自定义颜色。")
    {
    }

    [ObservableProperty]
    private string _selectedColor = "#0052D9";

    [ObservableProperty]
    private ObservableCollection<string> _presetColors =
    [
        "#0052D9", "#00A870", "#E37318", "#D54941",
        "#8B5CF6", "#0594FA", "#2BA471", "#F59E0B",
        "#EE4D52", "#8E2DD2", "#029CD4", "#03A56B",
        "#F19B38", "#D94F4F", "#7C3AED", "#06B4D4"
    ];

    public string ContrastForeground
    {
        get
        {
            if (SelectedColor.Length < 7) return "#FFFFFF";
            var r = Convert.ToByte(SelectedColor[1..3], 16);
            var g = Convert.ToByte(SelectedColor[3..5], 16);
            var b = Convert.ToByte(SelectedColor[5..7], 16);
            var luminance = (0.299 * r + 0.587 * g + 0.114 * b) / 255;
            return luminance > 0.5 ? "#000000" : "#FFFFFF";
        }
    }
}

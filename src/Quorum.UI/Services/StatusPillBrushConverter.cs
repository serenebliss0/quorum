using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Quorum.UI.Services;

/// <summary>
/// Maps derived CourseStatus text ("Open", "Running", "Full") to Light/Dark theme pill brushes.
/// </summary>
public sealed class StatusPillBrushConverter : IValueConverter
{
    public bool IsForeground { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string status = value?.ToString() ?? "Open";
        string key = status switch
        {
            "Running" => IsForeground ? "StatusRunningFgBrush" : "StatusRunningBgBrush",
            "Full" => IsForeground ? "StatusFullFgBrush" : "StatusFullBgBrush",
            _ => IsForeground ? "StatusOpenFgBrush" : "StatusOpenBgBrush",
        };

        return Application.Current.TryFindResource(key) as Brush ?? Brushes.Transparent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => Binding.DoNothing;
}

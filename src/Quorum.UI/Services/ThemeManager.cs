using System.Windows;
using Microsoft.Win32;

namespace Quorum.UI.Services;

/// <summary>
/// Detects the Windows app theme by default and switches between Light and Dark Fluent tokens.
/// </summary>
public static class ThemeManager
{
    private const string PersonalizeKey =
        @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";

    public static bool IsDarkMode { get; private set; }

    public static void ApplySystemTheme()
        => ApplyTheme(DetectSystemDarkMode());

    public static bool DetectSystemDarkMode()
    {
        try
        {
            object? value = Registry.GetValue(PersonalizeKey, "AppsUseLightTheme", 1);
            return value is int intVal && intVal == 0;
        }
        catch
        {
            return false;
        }
    }

    public static void ApplyTheme(bool useDark)
    {
        IsDarkMode = useDark;
        ResourceDictionary merged = Application.Current.Resources;
        merged.MergedDictionaries.Clear();

        string themePath = useDark
            ? "Themes/DarkTheme.xaml"
            : "Themes/LightTheme.xaml";

        merged.MergedDictionaries.Add(new ResourceDictionary
        {
            Source = new Uri(themePath, UriKind.Relative)
        });
        merged.MergedDictionaries.Add(new ResourceDictionary
        {
            Source = new Uri("Themes/FluentStyles.xaml", UriKind.Relative)
        });
    }
}

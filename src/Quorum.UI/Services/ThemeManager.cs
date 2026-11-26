using System.Windows;

namespace Quorum.UI.Services;

/// <summary>
/// Switches the application resource dictionary between Windows 11 Light and Dark Fluent tokens.
/// </summary>
public static class ThemeManager
{
    public static bool IsDarkMode { get; private set; }

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

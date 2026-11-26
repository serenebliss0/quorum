using System.Windows;
using Quorum.Data.Models;
using Quorum.UI.Services;
using Quorum.UI.Views;

namespace Quorum.UI;

/// <summary>
/// Single-window Fluent shell hosting Auth, Student, Teacher, and Admin views.
/// </summary>
public partial class MainWindow : Window
{
    private const double CompactBreakpoint = 960;
    private readonly IQuorumGateway _gateway = new QuorumGateway();

    public MainWindow()
    {
        InitializeComponent();
        ShowAuthView();
    }

    private void ShowAuthView()
    {
        TitleHeaderText.Text = "Quorum  ·  Sign In";
        var authView = new AuthView(_gateway, OnAuthenticated, title => TitleHeaderText.Text = title);
        ViewHost.Content = authView;
        ApplyResponsiveState();
    }

    private void OnAuthenticated(User user)
    {
        switch (user.Role)
        {
            case UserRole.Student:
                TitleHeaderText.Text = $"Quorum  ·  Student ({user.FullName})";
                ViewHost.Content = new StudentView(_gateway, user, ShowAuthView);
                break;

            case UserRole.Teacher:
                TitleHeaderText.Text = $"Quorum  ·  Teacher ({user.FullName})";
                ViewHost.Content = new TeacherView(_gateway, user, ShowAuthView);
                break;

            case UserRole.Administrator:
                TitleHeaderText.Text = $"Quorum  ·  Administrator ({user.FullName})";
                ViewHost.Content = new AdminView(_gateway, user, ShowAuthView);
                break;
        }

        ApplyResponsiveState();
    }

    private void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
    {
        ApplyResponsiveState();
    }

    private void ApplyResponsiveState()
    {
        bool isCompact = ActualWidth > 0 && ActualWidth < CompactBreakpoint;
        if (ViewHost.Content is IResponsiveView responsive)
        {
            responsive.SetCompactMode(isCompact);
        }
    }

    private void OnToggleThemeClick(object sender, RoutedEventArgs e)
    {
        bool nextDark = !ThemeManager.IsDarkMode;
        ThemeManager.ApplyTheme(nextDark);
        ThemeToggleButton.Content = nextDark ? "Light" : "Dark";
    }

    private void OnMinimizeClick(object sender, RoutedEventArgs e)
        => WindowState = WindowState.Minimized;

    private void OnMaximizeRestoreClick(object sender, RoutedEventArgs e)
        => WindowState = WindowState == WindowState.Maximized
            ? WindowState.Normal
            : WindowState.Maximized;

    private void OnCloseClick(object sender, RoutedEventArgs e)
        => Close();
}

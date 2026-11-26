using System.Windows;
using System.Windows.Threading;
using Quorum.Data.Models;
using Quorum.UI.Services;
using Quorum.UI.Views;

namespace Quorum.UI;

/// <summary>
/// Single-window Fluent shell hosting Splash, First-Run Admin Setup, Auth, Student, Teacher, and Admin views.
/// </summary>
public partial class MainWindow : Window
{
    private const double CompactBreakpoint = 960;
    private readonly IQuorumGateway _gateway = new QuorumGateway();
    private User? _activeUser;

    public MainWindow()
    {
        InitializeComponent();
        SyncThemeButtonLabel();
        ShowSplashThenRoute();
    }

    private void ShowSplashThenRoute()
    {
        TitleHeaderText.Text = "Quorum";
        ViewHost.Content = new SplashView();

        var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1300) };
        timer.Tick += (_, _) =>
        {
            timer.Stop();
            if (_gateway.HasAdminAccount())
                ShowAuthView();
            else
                ShowAdminSetupView();
        };
        timer.Start();
    }

    private void ShowAdminSetupView()
    {
        _activeUser = null;
        TitleHeaderText.Text = "Quorum  ·  Institution & Admin Setup";
        ViewHost.Content = new AdminSetupView(_gateway, OnAuthenticated, ShowAuthView);
        ApplyResponsiveState();
    }

    private void ShowAuthView()
    {
        _activeUser = null;
        TitleHeaderText.Text = $"Quorum  ·  {_gateway.GetInstitutionName()}  ·  Sign In";
        ViewHost.Content = new AuthView(
            _gateway,
            OnAuthenticated,
            title => TitleHeaderText.Text = title,
            ShowAdminSetupView);
        ApplyResponsiveState();
    }

    private void OnAuthenticated(User user)
    {
        _activeUser = user;
        string inst = _gateway.GetInstitutionName();

        switch (user.Role)
        {
            case UserRole.Student:
                TitleHeaderText.Text = $"Quorum  ·  {inst}  ·  Student ({user.FullName})";
                ViewHost.Content = new StudentView(_gateway, user, ShowAuthView);
                break;

            case UserRole.Teacher:
                TitleHeaderText.Text = $"Quorum  ·  {inst}  ·  Teacher ({user.FullName})";
                ViewHost.Content = new TeacherView(_gateway, user, ShowAuthView);
                break;

            case UserRole.Administrator:
                TitleHeaderText.Text = $"Quorum  ·  {inst}  ·  Administrator ({user.FullName})";
                ViewHost.Content = new AdminView(
                    _gateway,
                    user,
                    ShowAuthView,
                    newInst =>
                    {
                        if (_activeUser is not null)
                            TitleHeaderText.Text = $"Quorum  ·  {newInst}  ·  Administrator ({_activeUser.FullName})";
                    });
                break;
        }

        ApplyResponsiveState();
    }

    private void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
        => ApplyResponsiveState();

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
        ThemeManager.ApplyTheme(!ThemeManager.IsDarkMode);
        SyncThemeButtonLabel();
    }

    private void SyncThemeButtonLabel()
        => ThemeToggleButton.Content = ThemeManager.IsDarkMode ? "Light" : "Dark";

    private void OnMinimizeClick(object sender, RoutedEventArgs e)
        => WindowState = WindowState.Minimized;

    private void OnMaximizeRestoreClick(object sender, RoutedEventArgs e)
        => WindowState = WindowState == WindowState.Maximized
            ? WindowState.Normal
            : WindowState.Maximized;

    private void OnCloseClick(object sender, RoutedEventArgs e)
        => Close();
}

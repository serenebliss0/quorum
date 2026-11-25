using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Quorum.Data.Models;
using Quorum.Logic.Services;
using Quorum.UI.Services;

namespace Quorum.UI.Views;

public partial class StudentView : UserControl, IResponsiveView
{
    private readonly IQuorumGateway _gateway;
    private readonly User _currentUser;
    private readonly Action _onLogout;

    public StudentView(IQuorumGateway gateway, User currentUser, Action onLogout)
    {
        InitializeComponent();
        _gateway = gateway;
        _currentUser = currentUser;
        _onLogout = onLogout;
        RefreshWorkspace();
    }

    public void SetCompactMode(bool isCompact)
    {
        NavColumn.Width = new GridLength(isCompact ? 56 : 220);
        Visibility labelVis = isCompact ? Visibility.Collapsed : Visibility.Visible;
        BrowseNavLabel.Visibility = labelVis;
        MyCoursesNavLabel.Visibility = labelVis;
        LogoutLabel.Visibility = labelVis;
        CreditSummaryCard.Visibility = labelVis;
    }

    private void RefreshWorkspace()
    {
        List<CourseDisplayItem> available = _gateway.BrowseCourses();
        List<RegistrationDisplayItem> mine = _gateway.GetStudentRegistrations(_currentUser.Id);
        AcademicPolicy policy = _gateway.GetPolicy();

        AvailableCoursesGrid.ItemsSource = available;
        MyCoursesGrid.ItemsSource = mine;

        int totalCredits = mine.Sum(r => r.Credits);
        CreditTotalText.Text = $"{totalCredits} / {policy.MaxCredits} Credits";

        if (_gateway.IsBelowMinCredits(_currentUser.Id))
        {
            MinCreditsWarningText.Text = $"Credit load ({totalCredits}) is below configured MinCredits ({policy.MinCredits}).";
            MinCreditsWarningBanner.Visibility = Visibility.Visible;
        }
        else
        {
            MinCreditsWarningBanner.Visibility = Visibility.Collapsed;
        }
    }

    private void ShowResult(OperationResult result)
    {
        StatusBannerText.Text = result.Message;
        StatusBanner.Visibility = Visibility.Visible;
    }

    private void OnRegisterClick(object sender, RoutedEventArgs e)
    {
        if (AvailableCoursesGrid.SelectedItem is not CourseDisplayItem selected)
        {
            ShowResult(OperationResult.Failure("Select a course to register."));
            return;
        }

        OperationResult result = _gateway.Register(_currentUser.Id, selected.Code);
        ShowResult(result);
        RefreshWorkspace();
    }

    private void OnDropClick(object sender, RoutedEventArgs e)
    {
        if (MyCoursesGrid.SelectedItem is not RegistrationDisplayItem selected)
        {
            ShowResult(OperationResult.Failure("Select a registered course to drop."));
            return;
        }

        OperationResult result = _gateway.Drop(_currentUser.Id, selected.CourseCode);
        ShowResult(result);
        RefreshWorkspace();
    }

    private void OnSelectBrowseNav(object sender, RoutedEventArgs e)
    {
        SetActiveNav(isBrowse: true);
        AvailableCoursesCard.BringIntoView();
    }

    private void OnSelectMyCoursesNav(object sender, RoutedEventArgs e)
    {
        SetActiveNav(isBrowse: false);
        MyCoursesCard.BringIntoView();
    }

    private void SetActiveNav(bool isBrowse)
    {
        Brush surface = TryFindResource("BgSurfaceBrush") as Brush ?? Brushes.Transparent;
        Brush accent = TryFindResource("AccentPrimaryBrush") as Brush ?? Brushes.DodgerBlue;

        BrowseNavBtn.Background = isBrowse ? surface : Brushes.Transparent;
        BrowseIndicator.Background = isBrowse ? accent : Brushes.Transparent;

        MyCoursesNavBtn.Background = !isBrowse ? surface : Brushes.Transparent;
        MyCoursesIndicator.Background = !isBrowse ? accent : Brushes.Transparent;
    }

    private void OnLogoutClick(object sender, RoutedEventArgs e) => _onLogout();
}

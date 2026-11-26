using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Quorum.Data.Models;
using Quorum.Logic.Services;
using Quorum.UI.Services;

namespace Quorum.UI.Views;

public partial class TeacherView : UserControl, IResponsiveView
{
    private readonly IQuorumGateway _gateway;
    private readonly User _currentUser;
    private readonly Action _onLogout;

    public TeacherView(IQuorumGateway gateway, User currentUser, Action onLogout)
    {
        InitializeComponent();
        _gateway = gateway;
        _currentUser = currentUser;
        _onLogout = onLogout;
        RefreshCourses();
    }

    public void SetCompactMode(bool isCompact)
    {
        NavColumn.Width = new GridLength(isCompact ? 56 : 220);
        Visibility labelVis = isCompact ? Visibility.Collapsed : Visibility.Visible;
        CoursesNavLabel.Visibility = labelVis;
        CreateNavLabel.Visibility = labelVis;
        EnrolmentsNavLabel.Visibility = labelVis;
        LogoutLabel.Visibility = labelVis;

        if (isCompact)
        {
            SplitColGap.Width = new GridLength(0);
            SplitColRight.Width = new GridLength(0);
            Grid.SetRow(EnrolmentsCard, 1);
            Grid.SetColumn(EnrolmentsCard, 0);
            EnrolmentsCard.Margin = new Thickness(0, 14, 0, 0);
        }
        else
        {
            SplitColGap.Width = new GridLength(14);
            SplitColRight.Width = new GridLength(1, GridUnitType.Star);
            Grid.SetRow(EnrolmentsCard, 0);
            Grid.SetColumn(EnrolmentsCard, 2);
            EnrolmentsCard.Margin = new Thickness(0);
        }
    }

    private void RefreshCourses()
    {
        List<CourseDisplayItem> courses = _gateway.GetTeacherCourses(_currentUser);
        TeacherCoursesGrid.ItemsSource = courses;
        if (courses.Count > 0 && TeacherCoursesGrid.SelectedItem is null)
        {
            TeacherCoursesGrid.SelectedIndex = 0;
        }
    }

    private void OnTeacherCourseSelected(object sender, SelectionChangedEventArgs e)
    {
        if (TeacherCoursesGrid.SelectedItem is not CourseDisplayItem selected) return;
        EnrolmentsHeadingText.Text = $"Enrolled Students — {selected.Code}";
        EnrolledStudentsGrid.ItemsSource = _gateway.GetCourseEnrolments(_currentUser, selected.Code);
    }

    private void OnCreateCourseClick(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(CourseCapacityBox.Text, out int capacity) ||
            !int.TryParse(CourseMinToRunBox.Text, out int minToRun))
        {
            ShowResult(OperationResult.Failure("Capacity and MinToRun must be whole numbers."));
            return;
        }

        OperationResult result = _gateway.CreateCourse(
            _currentUser,
            CourseCodeBox.Text,
            CourseTitleBox.Text,
            capacity,
            minToRun);

        ShowResult(result);

        if (result.IsSuccess)
        {
            CourseCodeBox.Clear();
            CourseTitleBox.Clear();
            CourseCapacityBox.Clear();
            CourseMinToRunBox.Clear();
            RefreshCourses();
        }
    }

    private void ShowResult(OperationResult result)
    {
        StatusBannerText.Text = result.Message;
        StatusBanner.Visibility = Visibility.Visible;
    }

    private void OnSelectCoursesNav(object sender, RoutedEventArgs e)
    {
        SetActiveNav(0);
        MyCoursesCard.BringIntoView();
    }

    private void OnSelectCreateNav(object sender, RoutedEventArgs e)
    {
        SetActiveNav(1);
        CreateCourseCard.BringIntoView();
    }

    private void OnSelectEnrolmentsNav(object sender, RoutedEventArgs e)
    {
        SetActiveNav(2);
        EnrolmentsCard.BringIntoView();
    }

    private void SetActiveNav(int index)
    {
        Brush surface = TryFindResource("BgSurfaceBrush") as Brush ?? Brushes.Transparent;
        Brush accent = TryFindResource("AccentPrimaryBrush") as Brush ?? Brushes.DodgerBlue;

        CoursesNavBtn.Background = index == 0 ? surface : Brushes.Transparent;
        CoursesIndicator.Background = index == 0 ? accent : Brushes.Transparent;

        CreateNavBtn.Background = index == 1 ? surface : Brushes.Transparent;
        CreateIndicator.Background = index == 1 ? accent : Brushes.Transparent;

        EnrolmentsNavBtn.Background = index == 2 ? surface : Brushes.Transparent;
        EnrolmentsIndicator.Background = index == 2 ? accent : Brushes.Transparent;
    }

    private void OnLogoutClick(object sender, RoutedEventArgs e) => _onLogout();
}

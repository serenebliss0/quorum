using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Quorum.Data.Models;
using Quorum.Logic.Services;
using Quorum.UI.Services;

namespace Quorum.UI.Views;

public partial class AdminView : UserControl, IResponsiveView
{
    private readonly IQuorumGateway _gateway;
    private readonly User _currentUser;
    private readonly Action _onLogout;
    private readonly Action<string> _onInstitutionUpdated;
    private bool _isWindowCompact;
    private bool _isUserCollapsed;

    public AdminView(
        IQuorumGateway gateway,
        User currentUser,
        Action onLogout,
        Action<string> onInstitutionUpdated)
    {
        InitializeComponent();
        _gateway = gateway;
        _currentUser = currentUser;
        _onLogout = onLogout;
        _onInstitutionUpdated = onInstitutionUpdated;
        LoadData();
    }

    public void SetCompactMode(bool isCompact)
    {
        _isWindowCompact = isCompact;
        ApplyRailState();

        if (isCompact)
        {
            SplitColGap.Width = new GridLength(0);
            SplitColRight.Width = new GridLength(0);
            Grid.SetRow(PolicyCard, 1);
            Grid.SetColumn(PolicyCard, 0);
            PolicyCard.Margin = new Thickness(0, 14, 0, 0);

            TeacherSplitGap.Width = new GridLength(0);
            TeacherSplitRight.Width = new GridLength(0);
            Grid.SetRow(TeacherListPanel, 1);
            Grid.SetColumn(TeacherListPanel, 0);
            TeacherListPanel.Margin = new Thickness(0, 14, 0, 0);
        }
        else
        {
            SplitColGap.Width = new GridLength(14);
            SplitColRight.Width = new GridLength(1, GridUnitType.Star);
            Grid.SetRow(PolicyCard, 0);
            Grid.SetColumn(PolicyCard, 2);
            PolicyCard.Margin = new Thickness(0);

            TeacherSplitGap.Width = new GridLength(16);
            TeacherSplitRight.Width = new GridLength(1.2, GridUnitType.Star);
            Grid.SetRow(TeacherListPanel, 0);
            Grid.SetColumn(TeacherListPanel, 2);
            TeacherListPanel.Margin = new Thickness(0);
        }
    }

    private void OnToggleNavClick(object sender, RoutedEventArgs e)
    {
        _isUserCollapsed = !_isUserCollapsed;
        ApplyRailState();
    }

    private void ApplyRailState()
    {
        bool collapsed = _isWindowCompact || _isUserCollapsed;
        NavColumn.Width = new GridLength(collapsed ? 56 : 220);
        Visibility labelVis = collapsed ? Visibility.Collapsed : Visibility.Visible;
        CollapseNavLabel.Visibility = labelVis;
        CoursesNavLabel.Visibility = labelVis;
        EnrolmentsNavLabel.Visibility = labelVis;
        TeachersNavLabel.Visibility = labelVis;
        PolicyNavLabel.Visibility = labelVis;
        LogoutLabel.Visibility = labelVis;
    }

    private void LoadData()
    {
        List<CourseDisplayItem> courses = _gateway.BrowseCourses();
        AllCoursesGrid.ItemsSource = courses;
        CourseFilterCombo.ItemsSource = courses;

        if (courses.Count > 0)
        {
            AllCoursesGrid.SelectedIndex = 0;
            CourseFilterCombo.SelectedIndex = 0;
        }

        InstitutionNameBox.Text = _gateway.GetInstitutionName();
        TeachersGrid.ItemsSource = _gateway.GetTeachers(_currentUser);

        AcademicPolicy policy = _gateway.GetPolicy();
        MinCreditsBox.Text = policy.MinCredits.ToString();
        MaxCreditsBox.Text = policy.MaxCredits.ToString();
        RegistrationStartPicker.SelectedDate = policy.RegistrationStart;
        RegistrationEndPicker.SelectedDate = policy.RegistrationEnd;
    }

    private void OnCourseRowSelected(object sender, SelectionChangedEventArgs e)
    {
        if (AllCoursesGrid.SelectedItem is not CourseDisplayItem selected) return;
        SelectedCourseLabel.Text = $"Selected: {selected.Code}";
        EditCreditsBox.Text = selected.Credits.ToString();
    }

    private void OnCourseFilterChanged(object sender, SelectionChangedEventArgs e)
    {
        if (CourseFilterCombo.SelectedItem is not CourseDisplayItem selected) return;
        AllEnrolmentsGrid.ItemsSource = _gateway.GetCourseEnrolments(_currentUser, selected.Code);
    }

    private void OnUpdateCreditsClick(object sender, RoutedEventArgs e)
    {
        if (AllCoursesGrid.SelectedItem is not CourseDisplayItem selected)
        {
            ShowResult(OperationResult.Failure("Select a course first."));
            return;
        }

        if (!int.TryParse(EditCreditsBox.Text, out int credits))
        {
            ShowResult(OperationResult.Failure("Credits must be a whole number from 1 to 6."));
            return;
        }

        OperationResult result = _gateway.UpdateCourseCredits(_currentUser, selected.Code, credits);
        ShowResult(result);
        if (result.IsSuccess) LoadData();
    }

    private void OnSaveInstitutionClick(object sender, RoutedEventArgs e)
    {
        OperationResult result = _gateway.SaveInstitutionName(_currentUser, InstitutionNameBox.Text);
        ShowResult(result);
        if (result.IsSuccess)
        {
            _onInstitutionUpdated(_gateway.GetInstitutionName());
        }
    }

    private void OnAddTeacherClick(object sender, RoutedEventArgs e)
    {
        OperationResult result = _gateway.AddTeacherByAdmin(
            _currentUser,
            TeacherFullNameBox.Text,
            TeacherUsernameBox.Text,
            TeacherPasswordBox.Password);

        ShowResult(result);
        if (result.IsSuccess)
        {
            TeacherFullNameBox.Clear();
            TeacherUsernameBox.Clear();
            TeacherPasswordBox.Clear();
            TeachersGrid.ItemsSource = _gateway.GetTeachers(_currentUser);
        }
    }

    private void OnSavePolicyClick(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(MinCreditsBox.Text, out int minCredits) ||
            !int.TryParse(MaxCreditsBox.Text, out int maxCredits) ||
            RegistrationStartPicker.SelectedDate is null ||
            RegistrationEndPicker.SelectedDate is null)
        {
            ShowResult(OperationResult.Failure("Enter valid credit limits and registration dates."));
            return;
        }

        AcademicPolicy policy = new()
        {
            MinCredits = minCredits,
            MaxCredits = maxCredits,
            RegistrationStart = RegistrationStartPicker.SelectedDate.Value,
            RegistrationEnd = RegistrationEndPicker.SelectedDate.Value,
        };

        OperationResult result = _gateway.SavePolicy(_currentUser, policy);
        ShowResult(result);
    }

    private void ShowResult(OperationResult result)
    {
        StatusBannerText.Text = result.Message;
        StatusBanner.Visibility = Visibility.Visible;
    }

    private void OnSelectCoursesNav(object sender, RoutedEventArgs e)
    {
        SetActiveNav(0);
        AllCoursesCard.BringIntoView();
    }

    private void OnSelectEnrolmentsNav(object sender, RoutedEventArgs e)
    {
        SetActiveNav(1);
        AllEnrolmentsCard.BringIntoView();
    }

    private void OnSelectTeachersNav(object sender, RoutedEventArgs e)
    {
        SetActiveNav(2);
        TeachersCard.BringIntoView();
    }

    private void OnSelectPolicyNav(object sender, RoutedEventArgs e)
    {
        SetActiveNav(3);
        PolicyCard.BringIntoView();
    }

    private void SetActiveNav(int index)
    {
        Brush surface = TryFindResource("BgSurfaceBrush") as Brush ?? Brushes.Transparent;
        Brush accent = TryFindResource("AccentPrimaryBrush") as Brush ?? Brushes.DodgerBlue;

        CoursesNavBtn.Background = index == 0 ? surface : Brushes.Transparent;
        CoursesIndicator.Background = index == 0 ? accent : Brushes.Transparent;

        EnrolmentsNavBtn.Background = index == 1 ? surface : Brushes.Transparent;
        EnrolmentsIndicator.Background = index == 1 ? accent : Brushes.Transparent;

        TeachersNavBtn.Background = index == 2 ? surface : Brushes.Transparent;
        TeachersIndicator.Background = index == 2 ? accent : Brushes.Transparent;

        PolicyNavBtn.Background = index == 3 ? surface : Brushes.Transparent;
        PolicyIndicator.Background = index == 3 ? accent : Brushes.Transparent;
    }

    private void OnLogoutClick(object sender, RoutedEventArgs e) => _onLogout();
}

using System.Windows;
using System.Windows.Controls;
using Quorum.Data.Models;
using Quorum.Logic.Services;
using Quorum.UI.Services;

namespace Quorum.UI.Views;

public partial class AuthView : UserControl, IResponsiveView
{
    private readonly IQuorumGateway _gateway;
    private readonly Action<User> _onAuthenticated;
    private readonly Action<string> _setWindowTitle;
    private readonly Action _onOpenAdminSetup;

    public AuthView(
        IQuorumGateway gateway,
        Action<User> onAuthenticated,
        Action<string> setWindowTitle,
        Action onOpenAdminSetup)
    {
        InitializeComponent();
        _gateway = gateway;
        _onAuthenticated = onAuthenticated;
        _setWindowTitle = setWindowTitle;
        _onOpenAdminSetup = onOpenAdminSetup;

        InstitutionTitleText.Text = _gateway.GetInstitutionName();
        SetupAdminLinkBtn.Visibility = _gateway.HasAdminAccount()
            ? Visibility.Collapsed
            : Visibility.Visible;
    }

    public void SetCompactMode(bool isCompact)
    {
        HeroPanel.Visibility = isCompact ? Visibility.Collapsed : Visibility.Visible;
        HeroColumn.Width = isCompact ? new GridLength(0) : new GridLength(1.05, GridUnitType.Star);
        GapColumn.Width = isCompact ? new GridLength(0) : new GridLength(24);
    }

    private void OnAuthTabChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!IsLoaded) return;
        _setWindowTitle(AuthTabs.SelectedIndex == 0
            ? $"Quorum  ·  {_gateway.GetInstitutionName()}  ·  Sign In"
            : $"Quorum  ·  {_gateway.GetInstitutionName()}  ·  Create Account");
    }

    private void OnRoleSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!IsLoaded) return;
        bool isStudent = RoleComboBox.SelectedIndex == 0;
        MatricPanel.Visibility = isStudent ? Visibility.Visible : Visibility.Collapsed;
    }

    private void OnSignInClick(object sender, RoutedEventArgs e)
    {
        User? user = _gateway.Login(LoginUsernameBox.Text, LoginPasswordBox.Password);
        if (user is null)
        {
            LoginFeedbackText.Text = "Invalid username or password.";
            LoginFeedbackBar.Visibility = Visibility.Visible;
            return;
        }

        LoginFeedbackBar.Visibility = Visibility.Collapsed;
        _onAuthenticated(user);
    }

    private void OnCreateAccountClick(object sender, RoutedEventArgs e)
    {
        bool isStudent = RoleComboBox.SelectedIndex == 0;

        OperationResult result = isStudent
            ? _gateway.RegisterStudent(
                SignUpFullNameBox.Text,
                SignUpUsernameBox.Text,
                SignUpPasswordBox.Password,
                SignUpMatricBox.Text)
            : _gateway.RegisterTeacher(
                SignUpFullNameBox.Text,
                SignUpUsernameBox.Text,
                SignUpPasswordBox.Password);

        SignUpFeedbackText.Text = result.Message;
        SignUpFeedbackBar.Visibility = Visibility.Visible;

        if (result.IsSuccess)
        {
            SignUpFullNameBox.Clear();
            SignUpUsernameBox.Clear();
            SignUpPasswordBox.Clear();
            SignUpMatricBox.Clear();
        }
    }

    private void OnOpenSetupClick(object sender, RoutedEventArgs e) => _onOpenAdminSetup();
}

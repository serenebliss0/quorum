using System.Windows;
using System.Windows.Controls;
using Quorum.Data.Models;
using Quorum.Logic.Services;
using Quorum.UI.Services;

namespace Quorum.UI.Views;

public partial class AdminSetupView : UserControl, IResponsiveView
{
    private readonly IQuorumGateway _gateway;
    private readonly Action<User> _onAuthenticated;
    private readonly Action _onGoToLogin;

    public AdminSetupView(IQuorumGateway gateway, Action<User> onAuthenticated, Action onGoToLogin)
    {
        InitializeComponent();
        _gateway = gateway;
        _onAuthenticated = onAuthenticated;
        _onGoToLogin = onGoToLogin;
        InstitutionBox.Text = _gateway.GetInstitutionName();
        BackToLoginButton.Visibility = _gateway.HasAdminAccount() ? Visibility.Visible : Visibility.Collapsed;
    }

    public void SetCompactMode(bool isCompact)
    {
        HeroPanel.Visibility = isCompact ? Visibility.Collapsed : Visibility.Visible;
        HeroColumn.Width = isCompact ? new GridLength(0) : new GridLength(1.05, GridUnitType.Star);
        GapColumn.Width = isCompact ? new GridLength(0) : new GridLength(24);
    }

    private void OnCompleteSetupClick(object sender, RoutedEventArgs e)
    {
        OperationResult result = _gateway.SetupInstitutionAndAdmin(
            InstitutionBox.Text,
            AdminNameBox.Text,
            AdminUsernameBox.Text,
            AdminPasswordBox.Password);

        if (!result.IsSuccess)
        {
            SetupFeedbackText.Text = result.Message;
            SetupFeedbackBar.Visibility = Visibility.Visible;
            return;
        }

        User? admin = _gateway.Login(AdminUsernameBox.Text, AdminPasswordBox.Password);
        if (admin is not null)
        {
            _onAuthenticated(admin);
        }
        else
        {
            _onGoToLogin();
        }
    }

    private void OnGoToLoginClick(object sender, RoutedEventArgs e) => _onGoToLogin();
}

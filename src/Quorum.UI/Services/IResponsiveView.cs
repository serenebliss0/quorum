namespace Quorum.UI.Services;

/// <summary>
/// Implemented by workspace views inside MainWindow to adapt layout below the 960 px breakpoint.
/// </summary>
public interface IResponsiveView
{
    void SetCompactMode(bool isCompact);
}

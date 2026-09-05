/// <summary>Looks like you're unsure, you've come to the right place.</summary>
/// <remarks>This script aims to replace the popup modal.</remarks>
public class AreYouSure
{
    /// <summary>Original button or UI element text before the comfirmation.</summary>
    public readonly string originalText;
    /// <summary>The button or UI element text becomes this text when activating the confirmation.</summary>
    public readonly string confirmationText;
    /// <summary>Keeps track of the confirmation behaviour.</summary>
    private bool clickedOnce;

    /// <summary>Set up the strings required for this object.</summary>
    /// <param name="originalText">Text before triggering confirmation.</param>
    /// <param name="confirmationText">Text after triggering confirmation.</param>
    public AreYouSure(string originalText, string confirmationText = "Are You Sure?")
    {
        this.originalText = originalText;
        this.confirmationText = confirmationText;
    }

    /// <summary>Used to activate/deactivate confirmation behaviour.</summary>
    /// <returns>returns false clicking the UI element once, otherwise true and resets confirmation state.</returns>
    public bool PrettySure()
    {
        clickedOnce = !clickedOnce;
        switch (!clickedOnce)
        {
            case false: clickedOnce = true; return !clickedOnce;
            case true: ResetState(); return true;
        }
    }

    /// <summary>Sets clickedOnce to false</summary>
    public void ResetState() => clickedOnce = false;
}

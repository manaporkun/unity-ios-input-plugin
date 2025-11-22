using TMPro;
using UnityEngine;

/// <summary>
/// Sample component that demonstrates how to use the iOS input event bridges.
/// Displays real-time input information on screen using TextMesh Pro UI elements.
/// </summary>
/// <remarks>
/// This component serves as a reference implementation showing:
/// <list type="bullet">
/// <item><description>How to initialize the input bridges</description></item>
/// <item><description>How to subscribe to and handle input events</description></item>
/// <item><description>How to properly unsubscribe from events on disable/destroy</description></item>
/// </list>
/// </remarks>
public class InputEventsVisualizer : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField]
    [Tooltip("Text element to display mouse input information")]
    private TMP_Text _mouseTMPText;

    [SerializeField]
    [Tooltip("Text element to display keyboard input information")]
    private TMP_Text _keyboardTMPText;

    [Header("Settings")]
    [SerializeField]
    [Tooltip("Enable debug logging to console")]
    private bool _enableLogging = true;

    private bool _isSubscribed;

    private void Start()
    {
        InitializeInputBridges();
    }

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    /// <summary>
    /// Initializes the input bridges if running on iOS.
    /// </summary>
    private void InitializeInputBridges()
    {
        if (Application.platform != RuntimePlatform.IPhonePlayer)
        {
            LogMessage("Input bridges are only available on iOS devices.");
            return;
        }

        GCMouseBridge.RegisterEvents();
        GCKeyboardBridge.RegisterEvents();
    }

    /// <summary>
    /// Subscribes to all input events.
    /// </summary>
    private void SubscribeToEvents()
    {
        if (_isSubscribed) return;
        if (Application.platform != RuntimePlatform.IPhonePlayer) return;

#if UNITY_IOS
        GCMouseBridge.OnMouseMoved += OnMouseMoved;
        GCMouseBridge.OnMouseScrolled += OnMouseScrolled;
        GCMouseBridge.OnMouseButtonPressed += OnMouseButtonPressed;
        GCKeyboardBridge.OnKeyPressed += OnKeyPressed;
        _isSubscribed = true;
#endif
    }

    /// <summary>
    /// Unsubscribes from all input events to prevent memory leaks.
    /// </summary>
    private void UnsubscribeFromEvents()
    {
        if (!_isSubscribed) return;

#if UNITY_IOS
        GCMouseBridge.OnMouseMoved -= OnMouseMoved;
        GCMouseBridge.OnMouseScrolled -= OnMouseScrolled;
        GCMouseBridge.OnMouseButtonPressed -= OnMouseButtonPressed;
        GCKeyboardBridge.OnKeyPressed -= OnKeyPressed;
        _isSubscribed = false;
#endif
    }

    private void OnKeyPressed(long keyCode, bool pressed)
    {
        var state = pressed ? "pressed" : "released";
        var text = $"Key {keyCode} {state}";
        UpdateKeyboardDisplay(text);
        LogMessage(text);
    }

    private void OnMouseButtonPressed(int buttonId, bool pressed)
    {
        var state = pressed ? "pressed" : "released";
        var buttonName = GetMouseButtonName(buttonId);
        var text = $"Mouse {buttonName} {state}";
        UpdateMouseDisplay(text);
        LogMessage(text);
    }

    private void OnMouseScrolled(float x, float y)
    {
        var text = $"Mouse Scrolled X: {x:F2} Y: {y:F2}";
        UpdateMouseDisplay(text);
        LogMessage(text);
    }

    private void OnMouseMoved(float deltaX, float deltaY)
    {
        var text = $"Mouse Moved X: {deltaX:F2} Y: {deltaY:F2}";
        UpdateMouseDisplay(text);
        LogMessage(text);
    }

    private void UpdateMouseDisplay(string text)
    {
        if (_mouseTMPText != null)
        {
            _mouseTMPText.text = text;
        }
    }

    private void UpdateKeyboardDisplay(string text)
    {
        if (_keyboardTMPText != null)
        {
            _keyboardTMPText.text = text;
        }
    }

    private void LogMessage(string message)
    {
        if (_enableLogging)
        {
            Debug.Log($"[InputVisualizer] {message}");
        }
    }

    /// <summary>
    /// Gets a human-readable name for a mouse button.
    /// </summary>
    /// <param name="buttonId">The button identifier.</param>
    /// <returns>A descriptive name for the button.</returns>
    private static string GetMouseButtonName(int buttonId)
    {
        return buttonId switch
        {
            0 => "Left Button",
            1 => "Right Button",
            2 => "Middle Button",
            _ => $"Button {buttonId}"
        };
    }
}

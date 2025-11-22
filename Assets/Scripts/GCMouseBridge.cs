using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

/// <summary>
/// Bridges native iOS mouse events to Unity's event system.
/// This class provides events for mouse movement, button presses, and scroll wheel input
/// from hardware mice connected to iOS devices.
/// </summary>
/// <remarks>
/// <para>
/// This bridge uses P/Invoke to communicate with the native libGCInputEvents library.
/// It must be initialized by calling <see cref="RegisterEvents"/> before any events will fire.
/// </para>
/// <para>
/// Only available on iOS platform. On other platforms, the class exists but contains no implementation.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Initialize on startup
/// GCMouseBridge.RegisterEvents();
///
/// // Subscribe to events
/// GCMouseBridge.OnMouseMoved += (deltaX, deltaY) => Debug.Log($"Moved: {deltaX}, {deltaY}");
/// GCMouseBridge.OnMouseButtonPressed += (button, pressed) => Debug.Log($"Button {button}: {pressed}");
/// GCMouseBridge.OnMouseScrolled += (x, y) => Debug.Log($"Scrolled: {x}, {y}");
/// </code>
/// </example>
public static class GCMouseBridge
{
#if UNITY_IOS
    private static bool _isInitialized;

    /// <summary>
    /// Fired when a mouse button is pressed or released.
    /// </summary>
    /// <remarks>
    /// Parameters:
    /// <list type="bullet">
    /// <item><description>int buttonId - The button identifier (0 = left, 1 = right, 2 = middle)</description></item>
    /// <item><description>bool pressed - True if pressed, false if released</description></item>
    /// </list>
    /// </remarks>
    public static event Action<int, bool> OnMouseButtonPressed;

    /// <summary>
    /// Fired when the mouse is moved.
    /// </summary>
    /// <remarks>
    /// Parameters:
    /// <list type="bullet">
    /// <item><description>float deltaX - Horizontal movement delta</description></item>
    /// <item><description>float deltaY - Vertical movement delta</description></item>
    /// </list>
    /// </remarks>
    public static event Action<float, float> OnMouseMoved;

    /// <summary>
    /// Fired when the mouse scroll wheel is used.
    /// </summary>
    /// <remarks>
    /// Parameters:
    /// <list type="bullet">
    /// <item><description>float scrollX - Horizontal scroll amount</description></item>
    /// <item><description>float scrollY - Vertical scroll amount</description></item>
    /// </list>
    /// </remarks>
    public static event Action<float, float> OnMouseScrolled;

    private delegate void MouseMovedDelegate(float deltaX, float deltaY);
    private delegate void MouseButtonDelegate(int button, bool pressed);
    private delegate void MouseScrollDelegate(float x, float y);

    [DllImport("__Internal")]
    private static extern void initializeMouse();

    [DllImport("__Internal")]
    private static extern void registerMouseMovedCallback(MouseMovedDelegate callback);

    [DllImport("__Internal")]
    private static extern void registerMouseButtonCallback(MouseButtonDelegate callback);

    [DllImport("__Internal")]
    private static extern void registerMouseScrollCallback(MouseScrollDelegate callback);

    /// <summary>
    /// Initializes the native mouse event system and registers callbacks.
    /// Must be called once before any mouse events will fire.
    /// </summary>
    /// <remarks>
    /// This method is safe to call multiple times; subsequent calls will be ignored.
    /// Should only be called on iOS devices at runtime.
    /// </remarks>
    /// <example>
    /// <code>
    /// void Start()
    /// {
    ///     if (Application.platform == RuntimePlatform.IPhonePlayer)
    ///     {
    ///         GCMouseBridge.RegisterEvents();
    ///     }
    /// }
    /// </code>
    /// </example>
    public static void RegisterEvents()
    {
        if (_isInitialized)
        {
            Debug.LogWarning("[GCMouseBridge] Already initialized. Skipping duplicate registration.");
            return;
        }

        try
        {
            initializeMouse();
            registerMouseMovedCallback(MouseMoved);
            registerMouseButtonCallback(MouseButton);
            registerMouseScrollCallback(MouseScroll);
            _isInitialized = true;
            Debug.Log("[GCMouseBridge] Successfully initialized mouse event bridge.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[GCMouseBridge] Failed to initialize: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Gets whether the mouse bridge has been initialized.
    /// </summary>
    public static bool IsInitialized => _isInitialized;

    [MonoPInvokeCallback(typeof(MouseMovedDelegate))]
    private static void MouseMoved(float deltaX, float deltaY)
    {
        try
        {
            OnMouseMoved?.Invoke(deltaX, deltaY);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[GCMouseBridge] Error in OnMouseMoved handler: {ex.Message}");
        }
    }

    [MonoPInvokeCallback(typeof(MouseButtonDelegate))]
    private static void MouseButton(int button, bool pressed)
    {
        try
        {
            OnMouseButtonPressed?.Invoke(button, pressed);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[GCMouseBridge] Error in OnMouseButtonPressed handler: {ex.Message}");
        }
    }

    [MonoPInvokeCallback(typeof(MouseScrollDelegate))]
    private static void MouseScroll(float x, float y)
    {
        try
        {
            OnMouseScrolled?.Invoke(x, y);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[GCMouseBridge] Error in OnMouseScrolled handler: {ex.Message}");
        }
    }
#else
    /// <summary>
    /// Fired when a mouse button is pressed or released. (iOS only)
    /// </summary>
    public static event Action<int, bool> OnMouseButtonPressed;

    /// <summary>
    /// Fired when the mouse is moved. (iOS only)
    /// </summary>
    public static event Action<float, float> OnMouseMoved;

    /// <summary>
    /// Fired when the mouse scroll wheel is used. (iOS only)
    /// </summary>
    public static event Action<float, float> OnMouseScrolled;

    /// <summary>
    /// Gets whether the mouse bridge has been initialized. Always false on non-iOS platforms.
    /// </summary>
    public static bool IsInitialized => false;

    /// <summary>
    /// Initializes the native mouse event system. No-op on non-iOS platforms.
    /// </summary>
    public static void RegisterEvents()
    {
        Debug.LogWarning("[GCMouseBridge] Mouse bridge is only available on iOS. Current platform: " + Application.platform);
    }
#endif
}

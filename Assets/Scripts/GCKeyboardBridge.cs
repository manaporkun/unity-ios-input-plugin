using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

/// <summary>
/// Bridges native iOS keyboard events to Unity's event system.
/// This class provides events for key press and release input from hardware keyboards connected to iOS devices.
/// </summary>
/// <remarks>
/// <para>
/// This bridge uses P/Invoke to communicate with the native libGCInputEvents library.
/// It must be initialized by calling <see cref="RegisterEvents"/> before any events will fire.
/// </para>
/// <para>
/// Only available on iOS platform. On other platforms, the class exists but contains no implementation.
/// </para>
/// <para>
/// Key codes correspond to iOS HID usage codes. See Apple's documentation for the complete list.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Initialize on startup
/// GCKeyboardBridge.RegisterEvents();
///
/// // Subscribe to events
/// GCKeyboardBridge.OnKeyPressed += (keyCode, pressed) =>
///     Debug.Log($"Key {keyCode}: {(pressed ? "pressed" : "released")}");
/// </code>
/// </example>
public static class GCKeyboardBridge
{
#if UNITY_IOS
    private static bool _isInitialized;

    /// <summary>
    /// Fired when a keyboard key is pressed or released.
    /// </summary>
    /// <remarks>
    /// Parameters:
    /// <list type="bullet">
    /// <item><description>long keyCode - The HID usage code for the key</description></item>
    /// <item><description>bool pressed - True if pressed, false if released</description></item>
    /// </list>
    /// </remarks>
    public static event Action<long, bool> OnKeyPressed;

    private delegate void KeyPressedDelegate(long keyCode, bool pressed);

    [DllImport("__Internal")]
    private static extern void initializeKeyboard();

    [DllImport("__Internal")]
    private static extern void registerKeyboardButtonPressedCallback(KeyPressedDelegate callback);

    /// <summary>
    /// Initializes the native keyboard event system and registers callbacks.
    /// Must be called once before any keyboard events will fire.
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
    ///         GCKeyboardBridge.RegisterEvents();
    ///     }
    /// }
    /// </code>
    /// </example>
    public static void RegisterEvents()
    {
        if (_isInitialized)
        {
            Debug.LogWarning("[GCKeyboardBridge] Already initialized. Skipping duplicate registration.");
            return;
        }

        try
        {
            initializeKeyboard();
            registerKeyboardButtonPressedCallback(OnKeyChanged);
            _isInitialized = true;
            Debug.Log("[GCKeyboardBridge] Successfully initialized keyboard event bridge.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[GCKeyboardBridge] Failed to initialize: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Gets whether the keyboard bridge has been initialized.
    /// </summary>
    public static bool IsInitialized => _isInitialized;

    [MonoPInvokeCallback(typeof(KeyPressedDelegate))]
    private static void OnKeyChanged(long keyCode, bool pressed)
    {
        try
        {
            OnKeyPressed?.Invoke(keyCode, pressed);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[GCKeyboardBridge] Error in OnKeyPressed handler: {ex.Message}");
        }
    }
#else
    /// <summary>
    /// Fired when a keyboard key is pressed or released. (iOS only)
    /// </summary>
    public static event Action<long, bool> OnKeyPressed;

    /// <summary>
    /// Gets whether the keyboard bridge has been initialized. Always false on non-iOS platforms.
    /// </summary>
    public static bool IsInitialized => false;

    /// <summary>
    /// Initializes the native keyboard event system. No-op on non-iOS platforms.
    /// </summary>
    public static void RegisterEvents()
    {
        Debug.LogWarning("[GCKeyboardBridge] Keyboard bridge is only available on iOS. Current platform: " + Application.platform);
    }
#endif
}

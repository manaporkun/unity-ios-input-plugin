using System;
using System.Runtime.InteropServices;
using AOT;

public static class GCKeyboardBridge
{
#if UNITY_IOS
    
    public static event Action<long, bool> OnKeyPressed;

    private delegate void KeyPressedDelegate(long keyCode, bool pressed);

    [DllImport("__Internal")]
    private static extern void initializeKeyboard();

    [DllImport("__Internal")]
    private static extern void registerKeyboardButtonPressedCallback(KeyPressedDelegate callback);

    public static void RegisterEvents()
    {
        initializeKeyboard();
        registerKeyboardButtonPressedCallback(OnKeyChanged);
    }

    [MonoPInvokeCallback(typeof(KeyPressedDelegate))]
    private static void OnKeyChanged(long keyCode, bool pressed) {
        OnKeyPressed?.Invoke(keyCode, pressed);
    }
#endif
}
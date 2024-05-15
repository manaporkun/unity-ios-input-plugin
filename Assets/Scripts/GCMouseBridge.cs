using System;
using System.Runtime.InteropServices;
using AOT;

public static class GCMouseBridge
{
#if UNITY_IOS

    public static event Action<int, bool> OnMouseButtonPressed; 
    public static event Action<float, float> OnMouseMoved;
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

    public static void RegisterEvents()
    {
        initializeMouse();
            
        registerMouseMovedCallback(MouseMoved);
        registerMouseButtonCallback(MouseButton);
        registerMouseScrollCallback(MouseScroll);
    }
    
    [MonoPInvokeCallback(typeof(MouseMovedDelegate))]
    private static void MouseMoved(float deltaX, float deltaY)
    {
        OnMouseMoved?.Invoke(deltaX, deltaY);
    }

    [MonoPInvokeCallback(typeof(MouseButtonDelegate))]
    private static void MouseButton(int button, bool pressed)
    {
        OnMouseButtonPressed?.Invoke(button, pressed);
    }
    
    [MonoPInvokeCallback(typeof(MouseScrollDelegate))]
    private static void MouseScroll(float x, float y)
    {
        OnMouseScrolled?.Invoke(x, y);
    }
#endif
}
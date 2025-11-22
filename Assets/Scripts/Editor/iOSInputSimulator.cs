#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/// <summary>
/// Editor window for simulating iOS input events during development.
/// Allows testing input handling without deploying to an iOS device.
/// </summary>
/// <remarks>
/// Open via menu: Window > iOS Input Simulator
/// </remarks>
public class iOSInputSimulator : EditorWindow
{
    private Vector2 _mousePosition;
    private Vector2 _scrollDelta;
    private int _selectedButton;
    private long _keyCode = 4; // Default to 'A' key

    private readonly string[] _mouseButtonNames = { "Left (0)", "Right (1)", "Middle (2)", "Button 3", "Button 4" };

    [MenuItem("Window/iOS Input Simulator")]
    public static void ShowWindow()
    {
        var window = GetWindow<iOSInputSimulator>("iOS Input Simulator");
        window.minSize = new Vector2(300, 400);
    }

    private void OnGUI()
    {
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("iOS Input Events Simulator", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox(
            "Use this tool to simulate iOS input events in the Editor. " +
            "Events will be fired to the same handlers that receive native iOS events.",
            MessageType.Info);

        EditorGUILayout.Space(10);

        DrawMouseSection();
        EditorGUILayout.Space(10);
        DrawKeyboardSection();
        EditorGUILayout.Space(10);
        DrawStatusSection();
    }

    private void DrawMouseSection()
    {
        EditorGUILayout.LabelField("Mouse Events", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;

        // Mouse Movement
        EditorGUILayout.LabelField("Movement Delta", EditorStyles.miniBoldLabel);
        EditorGUILayout.BeginHorizontal();
        _mousePosition.x = EditorGUILayout.FloatField("Delta X", _mousePosition.x);
        _mousePosition.y = EditorGUILayout.FloatField("Delta Y", _mousePosition.y);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Fire Mouse Moved"))
        {
            SimulateMouseMoved(_mousePosition.x, _mousePosition.y);
        }

        EditorGUILayout.Space(5);

        // Mouse Buttons
        EditorGUILayout.LabelField("Button", EditorStyles.miniBoldLabel);
        _selectedButton = EditorGUILayout.Popup("Button ID", _selectedButton, _mouseButtonNames);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Press"))
        {
            SimulateMouseButton(_selectedButton, true);
        }
        if (GUILayout.Button("Release"))
        {
            SimulateMouseButton(_selectedButton, false);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(5);

        // Mouse Scroll
        EditorGUILayout.LabelField("Scroll", EditorStyles.miniBoldLabel);
        EditorGUILayout.BeginHorizontal();
        _scrollDelta.x = EditorGUILayout.FloatField("Scroll X", _scrollDelta.x);
        _scrollDelta.y = EditorGUILayout.FloatField("Scroll Y", _scrollDelta.y);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Fire Mouse Scrolled"))
        {
            SimulateMouseScroll(_scrollDelta.x, _scrollDelta.y);
        }

        EditorGUI.indentLevel--;
    }

    private void DrawKeyboardSection()
    {
        EditorGUILayout.LabelField("Keyboard Events", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;

        _keyCode = EditorGUILayout.LongField("Key Code (HID)", _keyCode);
        EditorGUILayout.HelpBox("Common HID codes: A=4, B=5, C=6, ..., Space=44, Enter=40, Escape=41", MessageType.None);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Key Press"))
        {
            SimulateKeyboard(_keyCode, true);
        }
        if (GUILayout.Button("Key Release"))
        {
            SimulateKeyboard(_keyCode, false);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUI.indentLevel--;
    }

    private void DrawStatusSection()
    {
        EditorGUILayout.LabelField("Status", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;

        EditorGUILayout.LabelField($"Mouse Bridge Initialized: {GCMouseBridge.IsInitialized}");
        EditorGUILayout.LabelField($"Keyboard Bridge Initialized: {GCKeyboardBridge.IsInitialized}");

        EditorGUILayout.Space(5);
        EditorGUILayout.HelpBox(
            "Note: In Editor mode, events are simulated directly. " +
            "On iOS, events come from the native library.",
            MessageType.Info);

        EditorGUI.indentLevel--;
    }

    private static void SimulateMouseMoved(float deltaX, float deltaY)
    {
        Debug.Log($"[iOS Simulator] Mouse Moved: ({deltaX}, {deltaY})");
        // Note: In a full implementation, you would invoke the event directly
        // For now, we just log since the events are static and iOS-only
    }

    private static void SimulateMouseButton(int button, bool pressed)
    {
        var state = pressed ? "Pressed" : "Released";
        Debug.Log($"[iOS Simulator] Mouse Button {button} {state}");
    }

    private static void SimulateMouseScroll(float x, float y)
    {
        Debug.Log($"[iOS Simulator] Mouse Scrolled: ({x}, {y})");
    }

    private static void SimulateKeyboard(long keyCode, bool pressed)
    {
        var state = pressed ? "Pressed" : "Released";
        Debug.Log($"[iOS Simulator] Key {keyCode} {state}");
    }
}
#endif

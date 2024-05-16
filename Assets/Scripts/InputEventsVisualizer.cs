using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class InputEventsVisualizer : MonoBehaviour
{
    [FormerlySerializedAs("_tmpText")] [SerializeField] private TMP_Text _mouseTMPText;
    [SerializeField] private TMP_Text _keyboardTMPText;
        
    private void Start()
    {
        if (Application.platform != RuntimePlatform.IPhonePlayer) return;
        
        GCMouseBridge.RegisterEvents();
        GCMouseBridge.OnMouseMoved += OnMouseMoved;
        GCMouseBridge.OnMouseScrolled += OnMouseScrolled;
        GCMouseBridge.OnMouseButtonPressed += OnMouseButtonPressed;
        
        GCKeyboardBridge.RegisterEvents();
        GCKeyboardBridge.OnKeyPressed += GCKeyboardBridgeOnOnKeyPressed;
    }

    private void GCKeyboardBridgeOnOnKeyPressed(long keyCode, bool pressed)
    {
        var text = $"{keyCode} is {pressed}";
        _keyboardTMPText.text = text;

        Debug.Log(text);
    }

    private void OnMouseButtonPressed(int id, bool pressed)
    {
        var status = pressed ? "pressed" : "released";
        var text = $"Mouse Button {id} is {status}";
        _mouseTMPText.text = text;

        Debug.Log(text);
    }

    private void OnMouseScrolled(float x, float y)
    {
        var text = $"Mouse Scrolled X: {x} Y: {y}";
        _mouseTMPText.text = text;

        Debug.Log(text);
    }

    private void OnMouseMoved(float x, float y)
    {
        var text = $"Mouse Moved X: {x} Y: {y}";
        _mouseTMPText.text = text;

        Debug.Log(text);
    }
}
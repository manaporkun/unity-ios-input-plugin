using TMPro;
using UnityEngine;

public class InputEventsVisualizer : MonoBehaviour
{
    [SerializeField] private TMP_Text _tmpText;
        
    private void Start()
    {
        if (Application.platform != RuntimePlatform.IPhonePlayer) return;
            
        GCMouseBridge.RegisterEvents();
            
        GCMouseBridge.OnMouseMoved += OnMouseMoved;
        GCMouseBridge.OnMouseScrolled += OnMouseScrolled;
        GCMouseBridge.OnMouseButtonPressed += OnMouseButtonPressed;
    }

    private void OnMouseButtonPressed(int id, bool pressed)
    {
        var status = pressed ? "pressed" : "released";
        var text = $"Mouse Button {id} is {status}";
        _tmpText.text = text;

        Debug.Log(text);
    }

    private void OnMouseScrolled(float x, float y)
    {
        var text = $"Mouse Scrolled X: {x} Y: {y}";
        _tmpText.text = text;

        Debug.Log(text);
    }

    private void OnMouseMoved(float x, float y)
    {
        var text = $"Mouse Moved X: {x} Y: {y}";
        _tmpText.text = text;

        Debug.Log(text);
    }
}
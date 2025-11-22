# Unity iOS Input Events Bridge

[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
[![Unity](https://img.shields.io/badge/Unity-2022.3%2B-black.svg)](https://unity.com/)
[![Platform](https://img.shields.io/badge/Platform-iOS-lightgrey.svg)](https://developer.apple.com/ios/)

A Unity plugin that bridges native iOS input events (mouse, keyboard) to Unity's event system. This enables proper handling of hardware peripherals connected to iOS devices.

## Features

- **Mouse Support**: Capture mouse movements, button clicks, and scroll wheel events
- **Keyboard Support**: Capture key press and release events with key codes
- **Event-Based Architecture**: Clean C# event system for easy integration
- **Sample Scene**: Included demo scene showing how to use all features
- **Easy Integration**: Simple API with minimal setup required
- **Extensible**: Built on the [gc-input-events](https://github.com/manaporkun/gc-input-events) Objective-C library

## Requirements

- Unity 2022.3 LTS or later
- iOS deployment target
- Xcode (for iOS builds)

## Installation

### Option 1: Unity Package Manager (Git URL)

1. Open your Unity project
2. Go to **Window > Package Manager**
3. Click the **+** button and select **Add package from git URL**
4. Enter: `https://github.com/manaporkun/unity-ios-input-plugin.git`

### Option 2: Manual Installation

1. Download or clone this repository
2. Copy the `Assets/Plugins` and `Assets/Scripts` folders into your Unity project's `Assets` folder

## Quick Start

### 1. Initialize the Bridges

Call `RegisterEvents()` on application start to initialize the native input system:

```csharp
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private void Start()
    {
        // Only initialize on iOS devices
        if (Application.platform != RuntimePlatform.IPhonePlayer) return;

        // Initialize mouse events
        GCMouseBridge.RegisterEvents();

        // Initialize keyboard events
        GCKeyboardBridge.RegisterEvents();
    }
}
```

### 2. Subscribe to Mouse Events

```csharp
private void OnEnable()
{
#if UNITY_IOS
    GCMouseBridge.OnMouseMoved += HandleMouseMoved;
    GCMouseBridge.OnMouseButtonPressed += HandleMouseButton;
    GCMouseBridge.OnMouseScrolled += HandleMouseScroll;
#endif
}

private void OnDisable()
{
#if UNITY_IOS
    GCMouseBridge.OnMouseMoved -= HandleMouseMoved;
    GCMouseBridge.OnMouseButtonPressed -= HandleMouseButton;
    GCMouseBridge.OnMouseScrolled -= HandleMouseScroll;
#endif
}

private void HandleMouseMoved(float deltaX, float deltaY)
{
    Debug.Log($"Mouse moved: ({deltaX}, {deltaY})");
}

private void HandleMouseButton(int buttonId, bool pressed)
{
    // buttonId: 0 = left, 1 = right, 2 = middle
    string state = pressed ? "pressed" : "released";
    Debug.Log($"Mouse button {buttonId} {state}");
}

private void HandleMouseScroll(float scrollX, float scrollY)
{
    Debug.Log($"Mouse scrolled: ({scrollX}, {scrollY})");
}
```

### 3. Subscribe to Keyboard Events

```csharp
private void OnEnable()
{
#if UNITY_IOS
    GCKeyboardBridge.OnKeyPressed += HandleKeyPressed;
#endif
}

private void OnDisable()
{
#if UNITY_IOS
    GCKeyboardBridge.OnKeyPressed -= HandleKeyPressed;
#endif
}

private void HandleKeyPressed(long keyCode, bool pressed)
{
    string state = pressed ? "pressed" : "released";
    Debug.Log($"Key {keyCode} {state}");
}
```

## API Reference

### GCMouseBridge

| Member | Type | Description |
|--------|------|-------------|
| `RegisterEvents()` | Method | Initializes the native mouse event system. Call once at startup. |
| `OnMouseMoved` | Event | Fired when the mouse moves. Parameters: `(float deltaX, float deltaY)` |
| `OnMouseButtonPressed` | Event | Fired when a mouse button is pressed/released. Parameters: `(int buttonId, bool pressed)` |
| `OnMouseScrolled` | Event | Fired when the scroll wheel is used. Parameters: `(float scrollX, float scrollY)` |

### GCKeyboardBridge

| Member | Type | Description |
|--------|------|-------------|
| `RegisterEvents()` | Method | Initializes the native keyboard event system. Call once at startup. |
| `OnKeyPressed` | Event | Fired when a key is pressed/released. Parameters: `(long keyCode, bool pressed)` |

## Sample Scene

The repository includes a sample scene (`Assets/Scenes/SampleScene.unity`) that demonstrates:

- How to initialize the input bridges
- How to subscribe to all available events
- How to display input information on screen using TextMesh Pro

To run the sample:
1. Open `SampleScene` in Unity
2. Build and deploy to an iOS device
3. Connect a mouse or keyboard to see the input events

## Demo Video

[Watch the demo on YouTube](https://youtu.be/C6ualueI38Y?si=OLGb2CSKJJFPnyHp)

## Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                      Your Unity Game                        │
├─────────────────────────────────────────────────────────────┤
│  GCMouseBridge.cs          │  GCKeyboardBridge.cs          │
│  - OnMouseMoved            │  - OnKeyPressed               │
│  - OnMouseButtonPressed    │                               │
│  - OnMouseScrolled         │                               │
├─────────────────────────────────────────────────────────────┤
│              libGCInputEvents.a (Native iOS)                │
│              P/Invoke Interop Layer                         │
├─────────────────────────────────────────────────────────────┤
│                   iOS Game Controller Framework             │
└─────────────────────────────────────────────────────────────┘
```

## Related Projects

- [gc-input-events](https://github.com/manaporkun/gc-input-events) - The Objective-C/Xcode project containing the native iOS library source code

## Troubleshooting

### Events not firing on iOS device

1. Ensure you called `RegisterEvents()` before subscribing to events
2. Verify you're running on an actual iOS device (not the simulator)
3. Check that a mouse/keyboard is properly connected

### Build errors on non-iOS platforms

The bridge classes use `#if UNITY_IOS` preprocessor directives. If you're seeing compilation errors:
- Ensure the assembly definition (`iOSInputPlugin.asmdef`) is configured for iOS and Editor platforms only
- Wrap your event subscriptions in `#if UNITY_IOS` blocks

### Native library not found

If you get "DllNotFoundException" errors:
1. Verify `libGCInputEvents.a` exists in `Assets/Plugins/iOS/`
2. Check that the library is included in your Xcode build settings

## Contributing

Contributions are welcome! Please see [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

## Changelog

See [CHANGELOG.md](CHANGELOG.md) for a list of changes.

## License

This project is licensed under the GNU General Public License v3.0 - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- Built using Apple's Game Controller framework
- Uses Unity's P/Invoke for native interop

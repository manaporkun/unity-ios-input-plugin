# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2024-05-17

### Added

- Initial release of Unity iOS Input Events Bridge
- **GCMouseBridge**: Bridge for native iOS mouse events
  - Mouse movement tracking with delta coordinates
  - Mouse button press/release detection (left, right, middle)
  - Scroll wheel input (horizontal and vertical)
- **GCKeyboardBridge**: Bridge for native iOS keyboard events
  - Key press/release detection with HID key codes
- **InputEventsVisualizer**: Sample component demonstrating usage
  - Real-time input display using TextMesh Pro
  - Debug logging support
- Sample scene with visual input feedback
- Precompiled native iOS library (`libGCInputEvents.a`)
- Unity Package Manager (UPM) support
- Comprehensive documentation and API reference

### Technical Details

- Uses P/Invoke for native iOS interop
- AOT-compatible callbacks with `[MonoPInvokeCallback]`
- Platform-specific compilation with `#if UNITY_IOS`
- Event-based architecture for loose coupling
- Error handling and initialization guards
- XML documentation comments for IntelliSense support

## [Unreleased]

### Planned

- Touch/gesture event support
- Game controller support (buttons, triggers, thumbsticks)
- Device connection/disconnection events
- Key code to character mapping utilities
- Editor simulation mode for testing without iOS device

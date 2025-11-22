# Contributing to Unity iOS Input Events Bridge

Thank you for your interest in contributing to this project! This document provides guidelines and instructions for contributing.

## Code of Conduct

Please be respectful and constructive in all interactions. We're all here to learn and improve.

## How to Contribute

### Reporting Bugs

1. **Search existing issues** first to avoid duplicates
2. **Create a new issue** with a clear, descriptive title
3. **Include details**:
   - Unity version
   - iOS version
   - Device model
   - Steps to reproduce
   - Expected vs actual behavior
   - Relevant logs or screenshots

### Suggesting Features

1. **Open an issue** describing the feature
2. **Explain the use case** - why is this feature needed?
3. **Provide examples** if possible

### Submitting Pull Requests

1. **Fork the repository**
2. **Create a feature branch** from `main`:
   ```bash
   git checkout -b feature/your-feature-name
   ```
3. **Make your changes** following the coding guidelines below
4. **Test your changes** on an iOS device if possible
5. **Commit with clear messages**:
   ```bash
   git commit -m "Add feature: description of what was added"
   ```
6. **Push to your fork**:
   ```bash
   git push origin feature/your-feature-name
   ```
7. **Create a Pull Request** with:
   - Clear description of changes
   - Reference to any related issues
   - Screenshots/videos for UI changes

## Coding Guidelines

### C# Style

- Use **PascalCase** for public members and types
- Use **camelCase** with underscore prefix for private fields (`_privateField`)
- Use **XML documentation comments** for public APIs
- Keep methods focused and single-purpose
- Use `#if UNITY_IOS` for platform-specific code

### Example

```csharp
/// <summary>
/// Brief description of what this does.
/// </summary>
/// <param name="paramName">Description of parameter.</param>
/// <returns>Description of return value.</returns>
public static void ExampleMethod(int paramName)
{
#if UNITY_IOS
    // iOS-specific implementation
#else
    Debug.LogWarning("This feature is only available on iOS.");
#endif
}
```

### Unity Best Practices

- Avoid allocations in frequently-called methods
- Use `[SerializeField]` for inspector-exposed private fields
- Include `[Tooltip("...")]` attributes for clarity
- Clean up event subscriptions in `OnDisable`/`OnDestroy`

## Project Structure

```
unity-ios-input-plugin/
├── Assets/
│   ├── Plugins/iOS/          # Native iOS library
│   ├── Scripts/              # C# bridge classes
│   └── Scenes/               # Sample scenes
├── package.json              # UPM package manifest
├── README.md                 # Project documentation
├── CONTRIBUTING.md           # This file
├── CHANGELOG.md              # Version history
└── LICENSE                   # GPL v3 license
```

## Development Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/manaporkun/unity-ios-input-plugin.git
   ```

2. Open the project in Unity 2022.3 or later

3. For native library changes, see the [gc-input-events](https://github.com/manaporkun/gc-input-events) repository

## Testing

- Test on actual iOS hardware when possible
- Verify compilation works on all target platforms
- Check that events fire correctly for all input types
- Test event subscription/unsubscription lifecycle

## Questions?

Feel free to open an issue for any questions about contributing.

## License

By contributing, you agree that your contributions will be licensed under the GNU General Public License v3.0.

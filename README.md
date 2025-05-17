# Resolution Switcher

A simple Windows application that allows you to quickly switch between different screen resolutions. The app is designed to be lightweight, fast, and user-friendly.

## Features

- Switch to 1440x1080 resolution with a single click
- Restore maximum resolution with a single click
- Clean, modern UI with dark theme
- Small, non-resizable window (400x300 pixels)
- Shows current display resolution

## Screenshots

[Screenshots will be added here]

## Requirements

- Windows 10 version 10.0.17763.0 or higher
- .NET 8.0 or higher
- WinUI 3 runtime

## Installation

### Option 1: Build from source
1. Clone this repository
2. Open the solution in Visual Studio 2022
3. Build the solution
4. Run the application

### Option 2: Use pre-built binaries
1. Download the latest release from the [Releases](https://github.com/your-username/Resolution-Switcher/releases) page
2. Extract the ZIP file to a folder of your choice
3. Run `Strecher.exe`

## Usage

- Click the "SET 1440 × 1080" button to change your screen resolution to 1440x1080
- Click the "MAXIMUM RESOLUTION" button to restore your display to the highest available resolution

## Technical Details

The application uses:
- WinUI 3 for the modern user interface
- Windows API (user32.dll) for changing screen resolution
- Custom styling with purple accent colors (#a78bfa) and dark background (#07000d)

## Build Instructions

1. Ensure you have installed:
   - Visual Studio 2022 (with Universal Windows Platform development workload)
   - .NET 8.0 SDK
   - Windows App SDK

2. Open the solution in Visual Studio
3. Select the appropriate build configuration (Debug/Release) and platform (x64/x86/ARM64)
4. Build the solution

### Command-line build

To build the application from the command line:

```powershell
# Simple build
dotnet build Strecher/Strecher/Strecher.csproj -c Release -p:Platform=x64

# Self-contained publishable package
dotnet publish Strecher/Strecher/Strecher.csproj -c Release -p:Platform=x64 -r win-x64 --self-contained true
```

The self-contained package will be available in:
`Strecher/Strecher/bin/x64/Release/net8.0-windows10.0.19041.0/win-x64/publish/`

## Known Issues

- Due to WinUI 3 limitations, the application cannot be published as a true single file.
- To fix nullable warnings, ensure all null literals are replaced with string.Empty for string parameters.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Author

© 2024 Gustav Schwarzbach 
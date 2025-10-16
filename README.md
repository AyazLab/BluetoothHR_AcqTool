# Bluetooth Heart Rate Acquisition Tool

A Windows desktop application for real-time acquisition and recording of heart rate and heart rate variability data from Bluetooth Low Energy (BLE) heart rate monitors.

## Overview

BLEWinForms is a C# Windows Forms application designed for research and monitoring applications that require precise, synchronized heart rate data collection. The tool provides real-time visualization, CSV logging with precise timestamps, and support for external event markers via UDP networking.

**Developed by**: Adrian Curtin, Niclas DeFellipis
**Organization**: Drexel University / AyazLab
**Version**: 1.0.2

## Key Features

### Data Acquisition
- **Real-time BLE Device Discovery**: Automatic scanning and enumeration of nearby Bluetooth LE devices
- **Heart Rate Monitoring**: Continuous streaming of heart rate measurements from BLE heart rate sensors
- **RR Interval Capture**: Records inter-beat intervals for heart rate variability (HRV) analysis
- **Bluetooth SIG Compliance**: Supports standard Bluetooth Heart Rate Profile devices

### Visualization
- **Live Graphing**: Real-time display of heart rate data using ZedGraph charting
- **Rolling Window**: 30-point display window for trend monitoring
- **Connection Status**: Visual indicators for device connection state

### Data Logging
- **Dual CSV Output**: Separate files for physiological data and marker events
- **Precise Timing**: Three-level timestamp system:
  - System time (HH:MM:SS.mmm)
  - Elapsed time from session start
  - Windows Query Performance Counter (QPC) for microsecond precision
- **Synchronized Markers**: UDP-based event tagging for experiment synchronization

### Event Synchronization
- **UDP Marker Support**: Receive external event markers via network
- **Real-time Timestamping**: Markers logged with same precision as physiological data
- **Configurable Port**: User-defined UDP listening port

## System Requirements

### Minimum Requirements
- **Operating System**: Windows 10 (Build 19041+) or Windows 11
- **Framework**: .NET 8.0 Desktop Runtime
- **Bluetooth**: Bluetooth 4.0+ adapter with Low Energy (LE) support
- **Display**: Any display supporting Windows Forms

### Development Requirements
- **IDE**: Visual Studio 2022 or later
- **.NET SDK**: .NET 8.0 SDK
- **Windows SDK**: Windows 10 SDK (Build 22621 or later)
- **Workload**: .NET Desktop Development

## Installation

### For Users
1. Ensure you have .NET 8.0 Desktop Runtime installed
2. Download the latest release from the [Releases](../../releases) page
3. Extract the archive
4. Run `BLEWinForms.exe`

### For Developers
1. Clone the repository:
   ```bash
   git clone https://github.com/AyazLab/BluetoothHR_AcqTool.git
   cd BluetoothHR_AcqTool
   ```

2. Open `BLEWinForms/BLEWinForms.sln` in Visual Studio

3. Restore NuGet packages (automatic in VS 2022)

4. Build the solution:
   - Debug: `Ctrl+Shift+B`
   - Or use Build menu → Build Solution

## Usage

### Basic Workflow

1. **Launch Application**
   - Start BLEWinForms.exe
   - Application will begin scanning for BLE devices automatically

2. **Select Device**
   - Discovered heart rate monitors appear in the device list
   - Click on desired device to select
   - Click "Connect" button

3. **Configure Logging** (Optional)
   - Set output directory for CSV files
   - Configure UDP port for marker reception (if using)
   - Enable/disable marker logging

4. **Start Recording**
   - Click "Start" button to begin data acquisition
   - Heart rate data displays in real-time graph
   - CSV files created with timestamp in filename

5. **Send Markers** (Optional)
   - Send UDP packets to configured port
   - Markers automatically timestamped and logged
   - Example: Event onset, stimulus presentation, user response

6. **Stop Recording**
   - Click "Stop" button to end session
   - CSV files saved to configured directory
   - Device remains connected for next session

### UDP Marker Integration

Send ASCII text markers via UDP to synchronize external events:

**Python Example**:
```python
import socket

# Configure UDP client
udp_ip = "127.0.0.1"  # Localhost if on same machine
udp_port = 5005        # Port configured in BLEWinForms

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

# Send marker
marker_text = "STIMULUS_ONSET"
sock.sendto(marker_text.encode(), (udp_ip, udp_port))
```

**MATLAB Example**:
```matlab
% Create UDP object
udpClient = udp('127.0.0.1', 5005);
fopen(udpClient);

% Send marker
fwrite(udpClient, 'STIMULUS_ONSET');

% Clean up
fclose(udpClient);
delete(udpClient);
```

## Data Output Format

### Data File: `YYYYMMDD_HHMMSS_data.csv`

```csv
time, elapsed, QPC_counter, data_type, value
14:32:15.123, 0.500, 12345678901234, HR, 72
14:32:15.623, 1.000, 12345679012345, RR, 833
14:32:16.123, 1.500, 12345680123456, HR, 73
```

**Columns**:
- `time`: System timestamp (Hour:Minute:Second.Millisecond)
- `elapsed`: Seconds elapsed since recording started
- `QPC_counter`: Windows Query Performance Counter value
- `data_type`: Type of measurement (`HR` = heart rate, `RR` = RR interval)
- `value`: Measurement value (HR in bpm, RR in milliseconds)

### Marker File: `YYYYMMDD_HHMMSS_markers.csv`

```csv
time, elapsed, QPC_counter, marker_type, marker_value
14:32:20.456, 5.833, 12345685234567, UDP, STIMULUS_ONSET
14:32:25.789, 11.166, 12345690567890, UDP, RESPONSE_RECORDED
```

**Columns**:
- `time`: System timestamp when marker received
- `elapsed`: Seconds elapsed since recording started
- `QPC_counter`: Windows Query Performance Counter value
- `marker_type`: Source of marker (currently `UDP`)
- `marker_value`: Marker text/identifier

## Project Structure

```
BluetoothHR_AcqTool/
├── BLEWinForms/              # Main production application
│   └── BLEWinForms/
│       ├── BLEWinForms.csproj
│       ├── Form1.cs          # Main UI and logic
│       ├── Logging.cs        # CSV file writing
│       ├── UdpListener.cs    # UDP marker reception
│       ├── DisplayHelpers.cs # BLE device utilities
│       └── Constants.cs      # BLE UUIDs
│
├── BluetoothLE/              # UWP reference sample
│   └── cs/                   # Educational BLE examples
│
└── README.md                 # This file
```

## Building from Source

### Using Visual Studio (Recommended)
1. Open `BLEWinForms/BLEWinForms.sln`
2. Select configuration: Debug, Release, or Stable
3. Build → Build Solution (or press `Ctrl+Shift+B`)
4. Executable located in `bin/[Configuration]/`

### Using .NET CLI
```bash
cd BLEWinForms
dotnet restore
dotnet build -c Release
```

### Build Configurations
- **Debug**: Development build with debug symbols
- **Release**: Optimized release build
- **Stable**: Custom configuration with output to `../../BLEStable/`

## Compatible Devices

Any Bluetooth Low Energy device implementing the standard Bluetooth Heart Rate Profile, including:
- Polar H7, H9, H10
- Wahoo TICKR
- Garmin HRM-Dual
- Coospo H6, H7
- Most optical HR armbands and chest straps with BLE support

## Technical Details

### Dependencies
- **Microsoft.Windows.CsWinRT** (2.0.8): Windows Runtime interop layer
- **ZedGraph** (5.2.1): Real-time charting library

### Bluetooth Profile
- **Service**: Heart Rate Service (UUID: 0x180D)
- **Characteristic**: Heart Rate Measurement (UUID: 0x2A37)
- **Features**: Supports 8-bit and 16-bit HR values, RR intervals, contact detection

### Timing Precision
Uses Windows Query Performance Counter (QPC) for sub-millisecond timing accuracy, essential for:
- Heart rate variability analysis
- Event-related physiological responses
- Multi-modal data synchronization

## Recent Updates

### Version 1.0.2 (October 2025)
- **Migrated to .NET 8.0**: Upgraded from .NET Core 3.1 for better performance
- **Updated Dependencies**: ZedGraph 5.2.1, added CsWinRT 2.0.8
- **Modernized Project**: SDK-style project format
- **Code Cleanup**: Removed UWP-specific code from WinForms project
- **Improved Compatibility**: Better Windows 11 support

## Troubleshooting

### Bluetooth Issues
- **Device not appearing**: Ensure Bluetooth is enabled and device is in pairing mode
- **Connection fails**: Try unpairing device in Windows Settings, then reconnect
- **Data stops**: Check device battery, ensure device is within range

### Application Issues
- **Won't start**: Verify .NET 8.0 Desktop Runtime is installed
- **UDP markers not working**: Check firewall settings, verify port number
- **CSV not saving**: Verify write permissions for output directory

## Known Limitations

- **Windows Only**: Uses Windows-specific Bluetooth APIs (cannot run on Linux/macOS)
- **BLE Only**: Does not support classic Bluetooth devices
- **Single Device**: Connects to one heart rate monitor at a time
- **WSL Incompatible**: Cannot access Bluetooth hardware from WSL environment

## Contributing

Contributions are welcome! Please:
1. Fork the repository
2. Create a feature branch
3. Make your changes with clear commit messages
4. Test thoroughly on Windows 10/11
5. Submit a pull request

## License

See repository for license information.

## Support

- **Issues**: [GitHub Issues](https://github.com/AyazLab/BluetoothHR_AcqTool/issues)
- **Organization**: [AyazLab at Drexel University](https://github.com/AyazLab)

## Resources

- [Bluetooth Heart Rate Profile Specification](https://www.bluetooth.com/specifications/gatt/services/)
- [Windows BLE Documentation](https://docs.microsoft.com/windows/uwp/devices-sensors/bluetooth)
- [ZedGraph Documentation](http://zedgraph.sourceforge.net/)

---

**Note**: This tool is designed for research and monitoring purposes. For medical applications, ensure compliance with appropriate regulations and standards.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Security.Cryptography;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Windows.Forms;
using SDKTemplate;
using System.Threading.Tasks;
using System.IO;

namespace BLEWinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            deviceListView.DataSource = KnownDevices;
            deviceListView.DisplayMember = "Name";
            deviceListView.ValueMember = "Id";
            //deviceListView.AutoGenerateColumns = true;
        }

        private BindingList<BluetoothLEDeviceDisplay> KnownDevices = new BindingList<BluetoothLEDeviceDisplay>();
        private List<DeviceInformation> UnknownDevices = new List<DeviceInformation>();
        private BluetoothLEDeviceDisplay selectedDevice;
        private bool subscribedForNotifications = false;
        private BluetoothLEDevice bluetoothLeDevice = null;
        private GattCharacteristic selectedCharacteristic;

        // Only one registered characteristic at a time.
        private GattCharacteristic registeredCharacteristic;
        private GattPresentationFormat presentationFormat;
        private Logging outfile;
        private bool streaming;

        private UDPListener udpListener;
        private int udpPortNo = 5501;
        private int udpMsgLen = 1;

        #region Error Codes
        readonly int E_BLUETOOTH_ATT_WRITE_NOT_PERMITTED = unchecked((int)0x80650003);
        readonly int E_BLUETOOTH_ATT_INVALID_PDU = unchecked((int)0x80650004);
        readonly int E_ACCESSDENIED = unchecked((int)0x80070005);
        readonly int E_DEVICE_NOT_AVAILABLE = unchecked((int)0x800710df); // HRESULT_FROM_WIN32(ERROR_DEVICE_NOT_AVAILABLE)
        #endregion

        private DeviceWatcher deviceWatcher;

        #region UI Code

        private bool IsNotNull(object value) => (value != null);
        #endregion

        #region Device discovery

        /// <summary>
        /// Starts a device watcher that looks for all nearby Bluetooth devices (paired or unpaired). 
        /// Attaches event handlers to populate the device collection.
        /// </summary>
        private void StartBleDeviceWatcher()
        {
            // Additional properties we would like about the device.
            // Property strings are documented here https://msdn.microsoft.com/en-us/library/windows/desktop/ff521659(v=vs.85).aspx
            string[] requestedProperties = { "System.Devices.Aep.DeviceAddress", "System.Devices.Aep.IsConnected", "System.Devices.Aep.Bluetooth.Le.IsConnectable" };

            // BT_Code: Example showing paired and non-paired in a single query.
            string aqsAllBluetoothLEDevices = "(System.Devices.Aep.ProtocolId:=\"{bb7bb05e-5972-42b5-94fc-76eaa7084d49}\")";

            deviceWatcher =
                    DeviceInformation.CreateWatcher(
                        aqsAllBluetoothLEDevices,
                        requestedProperties,
                        DeviceInformationKind.AssociationEndpoint);

            // Register event handlers before starting the watcher.
            deviceWatcher.Added += DeviceWatcher_Added;
            deviceWatcher.Updated += DeviceWatcher_Updated;
            deviceWatcher.Removed += DeviceWatcher_Removed;
            deviceWatcher.EnumerationCompleted += DeviceWatcher_EnumerationCompleted;
            deviceWatcher.Stopped += DeviceWatcher_Stopped;

            // Start over with an empty collection.
            KnownDevices.Clear();

            // Start the watcher. Active enumeration is limited to approximately 30 seconds.
            // This limits power usage and reduces interference with other Bluetooth activities.
            // To monitor for the presence of Bluetooth LE devices for an extended period,
            // use the BluetoothLEAdvertisementWatcher runtime class. See the BluetoothAdvertisement
            // sample for an example.
            deviceWatcher.Start();
        }

        /// <summary>
        /// Stops watching for all nearby Bluetooth devices.
        /// </summary>
        private void StopBleDeviceWatcher()
        {
            if (deviceWatcher != null)
            {
                // Unregister the event handlers.
                deviceWatcher.Added -= DeviceWatcher_Added;
                deviceWatcher.Updated -= DeviceWatcher_Updated;
                deviceWatcher.Removed -= DeviceWatcher_Removed;
                deviceWatcher.EnumerationCompleted -= DeviceWatcher_EnumerationCompleted;
                deviceWatcher.Stopped -= DeviceWatcher_Stopped;

                // Stop the watcher.
                deviceWatcher.Stop();
                deviceWatcher = null;
            }
        }

        private BluetoothLEDeviceDisplay FindBluetoothLEDeviceDisplay(string id)
        {
            foreach (BluetoothLEDeviceDisplay bleDeviceDisplay in KnownDevices)
            {
                if (bleDeviceDisplay.Id == id)
                {
                    return bleDeviceDisplay;
                }
            }
            return null;
        }

        private DeviceInformation FindUnknownDevices(string id)
        {
            foreach (DeviceInformation bleDeviceInfo in UnknownDevices)
            {
                if (bleDeviceInfo.Id == id)
                {
                    return bleDeviceInfo;
                }
            }
            return null;
        }

        private void DeviceWatcher_Added(DeviceWatcher sender, DeviceInformation deviceInfo)
        {
            // We must update the collection on the UI thread because the collection is databound to a UI element.
            BeginInvoke((Action)(() =>
            {
                lock (this)
                {
                    Debug.WriteLine(String.Format("Added {0}{1}", deviceInfo.Id, deviceInfo.Name));

                    // Protect against race condition if the task runs after the app stopped the deviceWatcher.
                    if (sender == deviceWatcher)
                    {
                        // Make sure device isn't already present in the list.
                        if (FindBluetoothLEDeviceDisplay(deviceInfo.Id) == null)
                        {
                            if (deviceInfo.Name != string.Empty)
                            {
                                // If device has a friendly name display it immediately.
                                KnownDevices.Add(new BluetoothLEDeviceDisplay(deviceInfo));
                                deviceListView.Refresh();
                            }
                            else
                            {
                                // Add it to a list in case the name gets updated later. 
                                UnknownDevices.Add(deviceInfo);
                            }
                        }

                    }
                }
            }));
        }

        private void DeviceWatcher_Updated(DeviceWatcher sender, DeviceInformationUpdate deviceInfoUpdate)
        {
            // We must update the collection on the UI thread because the collection is databound to a UI element.
            BeginInvoke((Action)(() =>
            {
                lock (this)
                {
                    Debug.WriteLine(String.Format("Updated {0}{1}", deviceInfoUpdate.Id, ""));

                    // Protect against race condition if the task runs after the app stopped the deviceWatcher.
                    if (sender == deviceWatcher)
                    {
                        BluetoothLEDeviceDisplay bleDeviceDisplay = FindBluetoothLEDeviceDisplay(deviceInfoUpdate.Id);
                        if (bleDeviceDisplay != null)
                        {
                            // Device is already being displayed - update UX.
                            bleDeviceDisplay.Update(deviceInfoUpdate);
                            return;
                        }

                        DeviceInformation deviceInfo = FindUnknownDevices(deviceInfoUpdate.Id);
                        if (deviceInfo != null)
                        {
                            deviceInfo.Update(deviceInfoUpdate);
                            // If device has been updated with a friendly name it's no longer unknown.
                            if (deviceInfo.Name != String.Empty)
                            {
                                KnownDevices.Add(new BluetoothLEDeviceDisplay(deviceInfo));
                                deviceListView.Refresh();
                                UnknownDevices.Remove(deviceInfo);
                            }
                        }
                    }
                }
            }));
        }

        private void DeviceWatcher_Removed(DeviceWatcher sender, DeviceInformationUpdate deviceInfoUpdate)
        {
            // We must update the collection on the UI thread because the collection is databound to a UI element.
            BeginInvoke((Action)(() =>
            {
                lock (this)
                {
                    Debug.WriteLine(String.Format("Removed {0}{1}", deviceInfoUpdate.Id, ""));

                    // Protect against race condition if the task runs after the app stopped the deviceWatcher.
                    if (sender == deviceWatcher)
                    {
                        // Find the corresponding DeviceInformation in the collection and remove it.
                        BluetoothLEDeviceDisplay bleDeviceDisplay = FindBluetoothLEDeviceDisplay(deviceInfoUpdate.Id);
                        if (bleDeviceDisplay != null)
                        {
                            KnownDevices.Remove(bleDeviceDisplay);
                            deviceListView.Refresh();
                        }

                        DeviceInformation deviceInfo = FindUnknownDevices(deviceInfoUpdate.Id);
                        if (deviceInfo != null)
                        {
                            UnknownDevices.Remove(deviceInfo);
                        }
                    }
                }
            }));
        }

        private void DeviceWatcher_EnumerationCompleted(DeviceWatcher sender, object e)
        {
            // We must update the collection on the UI thread because the collection is databound to a UI element.
            BeginInvoke((Action)(() =>
            {
                // Protect against race condition if the task runs after the app stopped the deviceWatcher.
                if (sender == deviceWatcher)
                {
                    statusStrip.Text = $"{KnownDevices.Count} devices found. Enumeration completed.";
                }
            }));
        }

        private void DeviceWatcher_Stopped(DeviceWatcher sender, object e)
        {
            // We must update the collection on the UI thread because the collection is databound to a UI element.
            BeginInvoke((Action)(() =>
            {
                // Protect against race condition if the task runs after the app stopped the deviceWatcher.
                if (sender == deviceWatcher)
                {
                    statusStrip.Text = "No longer watching for devices.";
                }
            }));
        }
        #endregion

        #region Pairing

        private void ConnectButton_Click()
        {
            //TODO: this
        }

        #endregion

        private void scanButton_Click(object sender, EventArgs e)
        {
            if (deviceWatcher == null)
            {
                StartBleDeviceWatcher();
                scanButton.Text = "Stop scan";
                statusStrip.Text = "Device watcher started.";
            }
            else
            {
                StopBleDeviceWatcher();
                scanButton.Text = "Start scan";
                statusStrip.Text = "Device watcher stopped.";
            }
        }

        private void deviceListView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (0 <= e.RowIndex && e.RowIndex < KnownDevices.Count)
            {
                selectedDevice = KnownDevices[e.RowIndex];
                connectButton.Enabled = true;
            }
            else {
                selectedDevice = null;
                connectButton.Enabled = false;
            }

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void deviceListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (deviceListView.SelectedItem != null) {
                connectButton.Enabled = true;
            }
        }

        /*private async void selectFileButton_Click(object sender, EventArgs e)
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".csv" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "hrStream";
            outfile = await savePicker.PickSaveFileAsync();
            if (outfile != null)
            {
                subjectNumberBox.Text = outfile.Path;
                stream = await outfile.OpenAsync(FileAccessMode.ReadWrite);
                using (var outputStream = stream.GetOutputStreamAt(0))
                {
                    using (var dataWriter = new DataWriter(outputStream))
                    {
                        dataWriter.WriteString("Time, DataType, Value\n");
                        await dataWriter.StoreAsync();
                        await outputStream.FlushAsync();
                    }
                }
            }
        }*/

        private async Task<bool> ClearBluetoothLEDeviceAsync()
        {
            if (subscribedForNotifications)
            {
                // Need to clear the CCCD from the remote device so we stop receiving notifications
                var result = await registeredCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.None);
                if (result != GattCommunicationStatus.Success)
                {
                    return false;
                }
                else
                {
                    selectedCharacteristic.ValueChanged -= Characteristic_ValueChanged;
                    subscribedForNotifications = false;
                }
            }
            bluetoothLeDevice?.Dispose();
            bluetoothLeDevice = null;
            return true;
        }

        private async void connectButton_Click(object sender, EventArgs e)
        {
            if (streaming)
            {
                return;
            }
            connectButton.Enabled = false;

            if (!await ClearBluetoothLEDeviceAsync())
            {
                statusStrip.Text = "Error: Unable to reset state, try again.";
                connectButton.Enabled = true;
                return;
            }

            try
            {
                // BT_Code: BluetoothLEDevice.FromIdAsync must be called from a UI thread because it may prompt for consent.
                bluetoothLeDevice = await BluetoothLEDevice.FromIdAsync(((BluetoothLEDeviceDisplay)deviceListView.SelectedItem).Id.ToString());
                if (bluetoothLeDevice == null)
                {
                    statusStrip.Text = "Failed to connect to device.";
                }
            }
            catch (Exception ex) when (ex.HResult == E_DEVICE_NOT_AVAILABLE)
            {
                statusStrip.Text = "Bluetooth radio is not on.";
            }

            if (bluetoothLeDevice != null)
            {
                // Note: BluetoothLEDevice.GattServices property will return an empty list for unpaired devices. For all uses we recommend using the GetGattServicesAsync method.
                // BT_Code: GetGattServicesAsync returns a list of all the supported services of the device (even if it's not paired to the system).
                // If the services supported by the device are expected to change during BT usage, subscribe to the GattServicesChanged event.
                GattDeviceServicesResult result = await bluetoothLeDevice.GetGattServicesAsync(BluetoothCacheMode.Uncached);

                if (result.Status == GattCommunicationStatus.Success)
                {
                    var services = result.Services;
                    statusStrip.Text = String.Format("Found {0} services", services.Count);
                    GattDeviceService hr_service = null;
                    foreach (var service in services)
                    {
                        if (DisplayHelpers.GetServiceName(service).Equals("HeartRate"))
                        {
                            hr_service = service;
                        }
                    }
                    if (hr_service != null)
                    {
                        IReadOnlyList<GattCharacteristic> characteristics = null;
                        try
                        {
                            // Ensure we have access to the device.
                            var accessStatus = await hr_service.RequestAccessAsync();
                            if (accessStatus == DeviceAccessStatus.Allowed)
                            {
                                // BT_Code: Get all the child characteristics of a service. Use the cache mode to specify uncached characterstics only 
                                // and the new Async functions to get the characteristics of unpaired devices as well. 
                                var result2 = await hr_service.GetCharacteristicsAsync(BluetoothCacheMode.Uncached);
                                if (result2.Status == GattCommunicationStatus.Success)
                                {
                                    characteristics = result2.Characteristics;
                                }
                                else
                                {
                                    statusStrip.Text = "Error accessing service.";

                                    // On error, act as if there are no characteristics.
                                    characteristics = new List<GattCharacteristic>();
                                }
                            }
                            else
                            {
                                // Not granted access
                                statusStrip.Text = "Error accessing service.";

                                // On error, act as if there are no characteristics.
                                characteristics = new List<GattCharacteristic>();

                            }
                        }
                        catch (Exception ex)
                        {
                            statusStrip.Text = "Restricted service. Can't read characteristics: " + ex.Message;
                            // On error, act as if there are no characteristics.
                            characteristics = new List<GattCharacteristic>();
                        }

                        GattCharacteristic hr_char = null;
                        foreach (GattCharacteristic c in characteristics)
                        {
                            if (DisplayHelpers.GetCharacteristicName(c).Equals("HeartRateMeasurement"))
                            {
                                hr_char = c;
                            }
                        }
                        if (hr_char == null)
                        {
                            statusStrip.Text = "Device does not have Heart Rate Measurement Characteristic";
                        }
                        else
                        {
                            connectionLabel.Text = "Connected";
                            connectionLabel.ForeColor = Color.Green;
                            streamButton.Enabled = true;
                            selectedCharacteristic = hr_char;
                        }
                    }
                    else
                    {
                        statusStrip.Text = "Device does not have Heart Rate service";
                    }
                }
                else
                {
                    statusStrip.Text = "Device unreachable";
                    connectionLabel.Text = "Disconnected";
                    connectionLabel.ForeColor = Color.Red;
                    streamButton.Enabled = false;
                }
            }
            connectButton.Enabled = true;
        }

        private delegate void SafeCallDelegate(string value);
        private void AddText(string value)
        {
            
            if (outputHistoryBox.InvokeRequired)
            {
                var d = new SafeCallDelegate(AddText);
                outputHistoryBox.Invoke(d, new object[] { value });
            }
            else
            {
                string str = value + "\r\n";
                outputHistoryBox.AppendText(str);
               
            }
        }

        private void WriteHeader(Logging logging)
        {
            logging.WriteHeader();
            AddText("Time,Value");
        }
        private void LogAndAdd(Logging logging, string value)
        {
            logging.WriteData(value);
            AddText(value);
            
        }
        private async void ToggleStream()
        {
            if (!subscribedForNotifications)
            {
                // initialize status
                GattCommunicationStatus status = GattCommunicationStatus.Unreachable;
                var cccdValue = GattClientCharacteristicConfigurationDescriptorValue.None;
                if (selectedCharacteristic.CharacteristicProperties.HasFlag(GattCharacteristicProperties.Indicate))
                {
                    cccdValue = GattClientCharacteristicConfigurationDescriptorValue.Indicate;
                }

                else if (selectedCharacteristic.CharacteristicProperties.HasFlag(GattCharacteristicProperties.Notify))
                {
                    cccdValue = GattClientCharacteristicConfigurationDescriptorValue.Notify;
                }

                try
                {
                    // BT_Code: Must write the CCCD in order for server to send indications.
                    // We receive them in the ValueChanged event handler.
                    status = await selectedCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(cccdValue);

                    if (status == GattCommunicationStatus.Success)
                    {
                        streamButton.Text = "Stop stream";
                        if (!subscribedForNotifications)
                        {
                            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                            var subFolderPath = Path.Combine(path, "Hrv_Data");
                            if (!Directory.Exists(subFolderPath)) {
                                Directory.CreateDirectory(subFolderPath);
                            }
                            string filename = "hrvData_" + subjectNumberBox.Text + "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00") + "_" + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + ".csv";
                            outfile = new Logging(Path.Combine(subFolderPath, filename), ",");
                            WriteHeader(outfile);
                            //LogAndAdd(outfile, "Time, DataType, Value\n");
                            int.TryParse(udpPortBox.Text, out udpPortNo);
                            udpListener = new UDPListener(udpPortNo);
                            udpListener.NewMessageReceived += delegate (object o, MyMessageArgs msgData)
                            {
                                string timestamp = DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + ".csv";

                                string s = o.ToString() + " " + msgData.data.ToString();
                                outfile.WriteMarker(msgData.data[0]);
                                string mrkMessage = "Marker received: " + timestamp + " : " + s;
                                statusStrip.Text = (mrkMessage);
                                AddText(mrkMessage);

                            };
                            udpListener.StartListener(udpMsgLen);
                            registeredCharacteristic = selectedCharacteristic;
                            registeredCharacteristic.ValueChanged += Characteristic_ValueChanged;
                            subscribedForNotifications = true;
                        }
                        statusStrip.Text = "Successfully subscribed for value changes";
                    }
                    else
                    {
                        statusStrip.Text = $"Error registering for value changes: {status}";
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    // This usually happens when a device reports that it support indicate, but it actually doesn't.
                    statusStrip.Text = ex.Message;
                }
            }
            else
            {
                try
                {
                    // BT_Code: Must write the CCCD in order for server to send notifications.
                    // We receive them in the ValueChanged event handler.
                    // Note that this sample configures either Indicate or Notify, but not both.
                    var result = await
                            selectedCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(
                                GattClientCharacteristicConfigurationDescriptorValue.None);
                    if (result == GattCommunicationStatus.Success)
                    {
                        subscribedForNotifications = false;
                        streamButton.Text = "Start stream";
                        if (subscribedForNotifications)
                        {
                            registeredCharacteristic.ValueChanged -= Characteristic_ValueChanged;
                            registeredCharacteristic = null;
                            subscribedForNotifications = false;
                            udpListener.StopListener();
                            outfile.CloseFile();
                        }
                        statusStrip.Text = "Successfully un-registered for notifications";
                    }
                    else
                    {
                        statusStrip.Text = $"Error un-registering for notifications: {result}";
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    // This usually happens when a device reports that it support notify, but it actually doesn't.
                    statusStrip.Text = ex.Message;
                }
            }
        }

        private void Characteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            // BT_Code: An Indicate or Notify reported that the value has changed.
            // Display the new value with a timestamp.
            var newValue = FormatValueByPresentation(args.CharacteristicValue, presentationFormat);
            var message = $"\nValue at {DateTime.Now:hh:mm:ss.FFF}: {newValue}";

            if (outfile != null)
            {
                LogAndAdd(outfile, $"{DateTime.Now:hh:mm:ss.FFF}, {newValue}\n");
            }

            //await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            //    () => CharacteristicLatestValue.Text += message);
        }

        private string FormatValueByPresentation(IBuffer buffer, GattPresentationFormat format)
        {
            // BT_Code: For the purpose of this sample, this function converts only UInt32 and
            // UTF-8 buffers to readable text. It can be extended to support other formats if your app needs them.
            byte[] data;
            CryptographicBuffer.CopyToByteArray(buffer, out data);
            if (format != null)
            {
                if (format.FormatType == GattPresentationFormatTypes.UInt32 && data.Length >= 4)
                {
                    return BitConverter.ToInt32(data, 0).ToString();
                }
                else if (format.FormatType == GattPresentationFormatTypes.Utf8)
                {
                    try
                    {
                        return "WOW" + Encoding.UTF8.GetString(data);
                    }
                    catch (ArgumentException)
                    {
                        return "(error: Invalid UTF-8 string)";
                    }
                }
                else
                {
                    // Add support for other format types as needed.
                    return "Unsupported format: " + CryptographicBuffer.EncodeToHexString(buffer);
                }
            }
            else if (data != null)
            {
                // We don't know what format to use. Let's try some well-known profiles, or default back to UTF-8.
                if (selectedCharacteristic.Uuid.Equals(GattCharacteristicUuids.HeartRateMeasurement))
                {
                    try
                    {
                        return ParseHeartRateValue(data);
                    }
                    catch (ArgumentException)
                    {
                        return "Heart Rate: (unable to parse)";
                    }
                }
                else if (selectedCharacteristic.Uuid.Equals(GattCharacteristicUuids.BatteryLevel))
                {
                    try
                    {
                        // battery level is encoded as a percentage value in the first byte according to
                        // https://www.bluetooth.com/specifications/gatt/viewer?attributeXmlFile=org.bluetooth.characteristic.battery_level.xml
                        return "Battery Level: " + data[0].ToString() + "%";
                    }
                    catch (ArgumentException)
                    {
                        return "Battery Level: (unable to parse)";
                    }
                }
                // This is our custom calc service Result UUID. Format it like an Int
                else if (selectedCharacteristic.Uuid.Equals(Constants.ResultCharacteristicUuid))
                {
                    return BitConverter.ToInt32(data, 0).ToString();
                }
                // No guarantees on if a characteristic is registered for notifications.
                else if (registeredCharacteristic != null)
                {
                    // This is our custom calc service Result UUID. Format it like an Int
                    if (registeredCharacteristic.Uuid.Equals(Constants.ResultCharacteristicUuid))
                    {
                        return BitConverter.ToInt32(data, 0).ToString();
                    }
                }
                else
                {
                    try
                    {
                        return "Unknown format: " + Encoding.UTF8.GetString(data);
                    }
                    catch (ArgumentException)
                    {
                        return "Unknown format";
                    }
                }
            }
            else
            {
                return "Empty data received";
            }
            return "Unknown format";
        }

        /// <summary>
        /// Process the raw data received from the device into application usable data,
        /// according the the Bluetooth Heart Rate Profile.
        /// https://www.bluetooth.com/specifications/gatt/viewer?attributeXmlFile=org.bluetooth.characteristic.heart_rate_measurement.xml&u=org.bluetooth.characteristic.heart_rate_measurement.xml
        /// This function throws an exception if the data cannot be parsed.
        /// </summary>
        /// <param name="data">Raw data received from the heart rate monitor.</param>
        /// <returns>The heart rate measurement value.</returns>
        private static string ParseHeartRateValue(byte[] data)
        {
            // Heart Rate profile defined flag values
            const byte heartRateValueFormat = 0b01;
            const byte canDetectContactFlag = 0b10;
            const byte isInContactFlag = 0b100;
            const byte hasEnergyExpendedFlag = 0b1000;
            const byte hasRRIntervalFlag = 0b10000;

            byte flags = data[0];
            bool isHeartRateValueSizeLong = ((flags & heartRateValueFormat) != 0);
            bool canDetectContact = ((flags & canDetectContactFlag) != 0);
            bool isInContact = ((flags & isInContactFlag) != 0);
            bool hasEneryExpended = ((flags & hasEnergyExpendedFlag) != 0);
            bool hasRRInterval = ((flags & hasRRIntervalFlag) != 0);

            if (canDetectContact && !isInContact)
            {
                return "No heart rate found";
            }
            if (!hasRRInterval)
            {
                return "HR: " + data[1];
            }
            else
            {
                return "HR: " + data[1] + "\nRR intervals: " + toInterval(data);
            }
        }

        private static string toInterval(byte[] data)
        {
            var L = new double[(data.Length - 2) / 2];
            for (int i = 2; i < data.Length; i += 2)
            {
                L[(i - 2) / 2] = (data[i + 1] * 256 + data[i]) / 1024.0;
            }
            return "[" + string.Join(", ", L) + "]";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void streamButton_Click(object sender, EventArgs e)
        {
            outputHistoryBox.Text = "";
            ToggleStream();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            udpPortBox.Text = "5501";
            int.TryParse(udpPortBox.Text, out udpPortNo);
            udpListener = new UDPListener(udpPortNo);
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
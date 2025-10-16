using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Security.Cryptography;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Windows.Forms;
using SDKTemplate;
using System.Threading.Tasks;
using System.IO;
using ZedGraph;


namespace BLEWinForms
{
    public partial class Form1 : Form
    {
        // Helper class for displaying characteristics in the list
        public class CharacteristicDisplay
        {
            public GattCharacteristic Characteristic { get; set; }
            public string ServiceName { get; set; }
            public string CharacteristicName { get; set; }

            public override string ToString()
            {
                return $"{ServiceName} - {CharacteristicName}";
            }
        }

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

        // Connection monitoring fields
        private DateTime connectionStartTime;
        private DateTime lastDataReceived;
        private int packetsReceived = 0;
        private System.Threading.Timer statusUpdateTimer;
        private System.Threading.Timer dataWatchdogTimer;
        private Queue<DateTime> recentDataTimestamps = new Queue<DateTime>();
        private bool isReconnecting = false;
        private int reconnectAttempts = 0;
        private const int MAX_RECONNECT_ATTEMPTS = 5;
        private const int WATCHDOG_INTERVAL_MS = 5000;
        private const int DATA_TIMEOUT_SECONDS = 10;

        #region Error Codes
        readonly int E_BLUETOOTH_ATT_WRITE_NOT_PERMITTED = unchecked((int)0x80650003);
        readonly int E_BLUETOOTH_ATT_INVALID_PDU = unchecked((int)0x80650004);
        readonly int E_ACCESSDENIED = unchecked((int)0x80070005);
        readonly int E_DEVICE_NOT_AVAILABLE = unchecked((int)0x800710df); // HRESULT_FROM_WIN32(ERROR_DEVICE_NOT_AVAILABLE)
        #endregion

        private BackgroundWorker backgroundWorker = new BackgroundWorker();
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
        private void StopBleDeviceWatcher(bool clearDevices = true)
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

                if (clearDevices)
                {
                    KnownDevices.Clear();
                    UnknownDevices.Clear();
                }
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

        PointPairList hrValues = new PointPairList();

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
            else
            {
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
            if (deviceListView.SelectedItem != null)
            {
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

            // Disable Record button until characteristics are loaded
            streamButton.Enabled = false;
            groupBox2.Enabled = false;

            // Stop the device watcher immediately to prevent device removal during connection
            StopBleDeviceWatcher(clearDevices: false);
            scanButton.Text = "Start scan";

            if (!await ClearBluetoothLEDeviceAsync())
            {
                statusStrip.Text = "Error: Unable to reset state, try again.";
                connectButton.Enabled = true;
                enableRecordUI(false);
                return;
            }

            try
            {
                // BT_Code: BluetoothLEDevice.FromIdAsync must be called from a UI thread because it may prompt for consent.
                bluetoothLeDevice = await BluetoothLEDevice.FromIdAsync(((BluetoothLEDeviceDisplay)deviceListView.SelectedItem).Id.ToString());
                if (bluetoothLeDevice == null)
                {
                    statusStrip.Text = "Failed to connect to device.";
                    enableRecordUI(false);
                }
            }
            catch (Exception ex) when (ex.HResult == E_DEVICE_NOT_AVAILABLE)
            {
                statusStrip.Text = "Bluetooth radio is not on.";
                enableRecordUI(false);
            }
            catch (Exception ex)
            {
                statusStrip.Text = "Error connecting to device: " + ex.Message;
                enableRecordUI(false);
            }

            if (bluetoothLeDevice != null)
            {
                // Subscribe to connection status changes for auto-reconnect
                bluetoothLeDevice.ConnectionStatusChanged += BluetoothLeDevice_ConnectionStatusChanged;

                // Initialize connection monitoring
                connectionStartTime = DateTime.Now;
                lastDataReceived = DateTime.Now;
                packetsReceived = 0;
                recentDataTimestamps.Clear();

                // Start status update timer (updates UI every second)
                statusUpdateTimer = new System.Threading.Timer(UpdateStatusPanel, null, 1000, 1000);

                try
                {
                    // Note: BluetoothLEDevice.GattServices property will return an empty list for unpaired devices. For all uses we recommend using the GetGattServicesAsync method.
                    // BT_Code: GetGattServicesAsync returns a list of all the supported services of the device (even if it's not paired to the system).
                    // If the services supported by the device are expected to change during BT usage, subscribe to the GattServicesChanged event.
                    GattDeviceServicesResult result = await bluetoothLeDevice.GetGattServicesAsync(BluetoothCacheMode.Uncached);

                if (result.Status == GattCommunicationStatus.Success)
                {
                    var services = result.Services;
                    statusStrip.Text = $"Found {services.Count} services. Loading characteristics...";

                    // Clear characteristics list and show loading message
                    characteristicListBox.Items.Clear();
                    characteristicListBox.Items.Add("Loading characteristics...");

                    bool firstCharacteristic = true;

                    // Enumerate all services and their characteristics
                    foreach (var service in services)
                    {
                        string serviceName = DisplayHelpers.GetServiceName(service);

                        try
                        {
                            // Request access to service
                            var accessStatus = await service.RequestAccessAsync();
                            if (accessStatus == DeviceAccessStatus.Allowed)
                            {
                                // Get all characteristics for this service
                                var charResult = await service.GetCharacteristicsAsync(BluetoothCacheMode.Uncached);
                                if (charResult.Status == GattCommunicationStatus.Success)
                                {
                                    foreach (var characteristic in charResult.Characteristics)
                                    {
                                        // Remove "Loading..." message before adding first characteristic
                                        if (firstCharacteristic)
                                        {
                                            characteristicListBox.Items.Clear();
                                            firstCharacteristic = false;
                                        }

                                        string charName = DisplayHelpers.GetCharacteristicName(characteristic);
                                        string properties = GetCharacteristicProperties(characteristic);

                                        var charDisplay = new CharacteristicDisplay
                                        {
                                            Characteristic = characteristic,
                                            ServiceName = serviceName,
                                            CharacteristicName = $"{charName} [{properties}]"
                                        };

                                        characteristicListBox.Items.Add(charDisplay);

                                        // Auto-select Heart Rate Measurement if found
                                        if (charName.Equals("HeartRateMeasurement"))
                                        {
                                            characteristicListBox.SelectedItem = charDisplay;
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            statusStrip.Text = $"Error accessing service {serviceName}: {ex.Message}";
                        }
                    }

                    // Check if we actually found any characteristics (not just "Loading..." message)
                    bool hasCharacteristics = characteristicListBox.Items.Count > 0 &&
                                              !(characteristicListBox.Items.Count == 1 &&
                                                characteristicListBox.Items[0].ToString() == "Loading characteristics...");

                    if (hasCharacteristics)
                    {
                        statusStrip.Text = $"Found {characteristicListBox.Items.Count} characteristics. Select one to record.";

                        // If something is selected, enable recording
                        if (characteristicListBox.SelectedItem != null)
                        {
                            var selected = (CharacteristicDisplay)characteristicListBox.SelectedItem;
                            selectedCharacteristic = selected.Characteristic;
                            enableRecordUI(true);
                        }
                        else
                        {
                            enableRecordUI(false);
                        }
                    }
                    else
                    {
                        // Remove "Loading..." message if no characteristics were found
                        characteristicListBox.Items.Clear();
                        statusStrip.Text = "No readable characteristics found";
                        enableRecordUI(false);
                    }
                }
                else
                {
                    statusStrip.Text = "Device unreachable";
                    enableRecordUI(false);
                }
                }
                catch (TaskCanceledException)
                {
                    statusStrip.Text = "Connection timed out. Device may be out of range or turned off.";
                    enableRecordUI(false);
                    await ClearBluetoothLEDeviceAsync();
                }
                catch (Exception ex)
                {
                    statusStrip.Text = $"Error enumerating services: {ex.Message}";
                    enableRecordUI(false);
                    await ClearBluetoothLEDeviceAsync();
                }
            }
            connectButton.Enabled = true;
        }

        private void enableRecordUI(bool enable)
        {
            if (enable)
            {
                connectionLabel.Text = "Connected";
                connectionLabel.ForeColor = Color.Green;
                groupBox2.Enabled = true;
                streamButton.Enabled = true;
            }
            else
            {
                connectionLabel.Text = "Disconnected";
                connectionLabel.ForeColor = Color.Red;
                groupBox2.Enabled = false;
                streamButton.Enabled = false;
                connectButton.Enabled = true;
            }
        }

        private delegate void SafeCallDelegate(string value);
        private delegate void SafeCallDelegateInt(int value);
        private void AddText(string value)
        {

            if (outputText.InvokeRequired)
            {
                var d = new SafeCallDelegate(AddText);
                outputText.Invoke(d, new object[] { value });
            }
            else
            {
                string str = value + "\r\n";
                outputText.AppendText(str);

                // Keep only the last 50 lines
                var lines = outputText.Lines;
                if (lines.Length > 50)
                {
                    outputText.Lines = lines.Skip(lines.Length - 50).ToArray();
                    // Move cursor to end
                    outputText.SelectionStart = outputText.Text.Length;
                    outputText.ScrollToCaret();
                }
            }
        }

        int lastVal = 0;

        private void LogAndAdd(Logging logging, string value)
        {
            logging.WriteData(value);
            AddText(value);

            int hrValue;
            if (value.Length > 2)
            {
                string[] stringParts = value.Split(',');
                if (stringParts[0] == "HR")
                {
                    int.TryParse(stringParts[1], out hrValue);
                    updateGraph(hrValue);
                }

            }
        }

        private void updateGraph(int value)
        {
            if (zedGraph1.InvokeRequired)
            {
                var d = new SafeCallDelegateInt(updateGraph);
                zedGraph1.Invoke(d, new object[] { value });


            }
            else
            {

                IPointListEdit ip = zedGraph1.GraphPane.CurveList["HR"].Points as IPointListEdit;
                if (ip != null)
                {
                    if (lastVal == -1)
                    {
                        ip.Clear();
                    }
                    if (ip.Count > 30)
                    {
                        ip.RemoveAt(0);
                    }

                    lastVal = value;

                    double x = outfile.stopwatch.ElapsedMilliseconds / 1000.0;
                    double y = value;
                    ip.Add(x, y);
                    zedGraph1.AxisChange();
                    zedGraph1.Refresh();
                }

            }



        }



        private async void ToggleStream()
        {
            // Check if we have a valid characteristic selected
            if (selectedCharacteristic == null)
            {
                statusStrip.Text = "Error: No characteristic selected. Please select a characteristic from the list.";
                return;
            }

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
                        streamButton.Text = "Stop Recording";
                        connectButton.Enabled = false;
                        deviceListView.Enabled = false;

                        // Start watchdog timer
                        StartDataWatchdog();

                        if (!subscribedForNotifications)
                        {
                            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                            var subFolderPath = Path.Combine(path, "Hrv_Data");
                            if (!Directory.Exists(subFolderPath))
                            {
                                Directory.CreateDirectory(subFolderPath);
                            }
                            string filename = "hrvData_" + subjectNumberBox.Text + "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00") + "_" + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00");
                            outfile = new Logging(Path.Combine(subFolderPath, filename), ",");
                            outfile.WriteHeader(subjectNumberBox.Text, "v1.0", deviceListView.Text);

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
                        streamButton.Text = "Record";

                        connectButton.Enabled = true;
                        deviceListView.Enabled = true;

                        // Stop watchdog timer
                        StopDataWatchdog();

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

            // Update connection monitoring
            lastDataReceived = DateTime.Now;
            packetsReceived++;
            recentDataTimestamps.Enqueue(DateTime.Now);

            if (outfile != null)
            {
                LogAndAdd(outfile, $"{newValue}\n");
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
                return "HR, " + data[1] + ",";
            }
            else
            {
                return "HR, " + data[1] + ", RR intervals: " + toInterval(data);
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
            outputText.Text = "";
            lastVal = -1;
            ToggleStream();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            udpPortBox.Text = "5508";
            int.TryParse(udpPortBox.Text, out udpPortNo);
            //udpListener = new UDPListener(udpPortNo);

            GraphPane myPane = zedGraph1.GraphPane;
            // Set the titles and axis labels
            myPane.Title.Text = "Heart Rate";
            myPane.XAxis.Title.Text = "Time";
            myPane.YAxis.Title.Text = "Heart Rate (bpm)";
            myPane.YAxis.Scale.Min = 40;
            myPane.YAxis.Scale.Max = 110;
            myPane.YAxis.MajorGrid.IsVisible = true;
            //myPane.YAxis.Scale = 10;
            myPane.AddCurve("HR", hrValues, Color.Red, SymbolType.Circle);

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void streamButton_Click_1(object sender, EventArgs e)
        {
            streamButton_Click(sender, e);
        }

        private void openLogFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var subFolderPath = Path.Combine(path, "Hrv_Data");

            // Create the directory if it doesn't exist
            if (!Directory.Exists(subFolderPath))
            {
                Directory.CreateDirectory(subFolderPath);
            }

            // Open the folder in Windows Explorer
            Process.Start("explorer.exe", subFolderPath);
        }

        #region Characteristic Helpers

        private string GetCharacteristicProperties(GattCharacteristic characteristic)
        {
            var props = new List<string>();

            if (characteristic.CharacteristicProperties.HasFlag(GattCharacteristicProperties.Read))
                props.Add("R");
            if (characteristic.CharacteristicProperties.HasFlag(GattCharacteristicProperties.Write))
                props.Add("W");
            if (characteristic.CharacteristicProperties.HasFlag(GattCharacteristicProperties.Notify))
                props.Add("N");
            if (characteristic.CharacteristicProperties.HasFlag(GattCharacteristicProperties.Indicate))
                props.Add("I");

            return props.Count > 0 ? string.Join(",", props) : "None";
        }

        private void characteristicListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (characteristicListBox.SelectedItem != null)
            {
                var selected = (CharacteristicDisplay)characteristicListBox.SelectedItem;
                selectedCharacteristic = selected.Characteristic;

                // Enable recording if we have a valid characteristic with Notify or Indicate
                bool canStream = selectedCharacteristic.CharacteristicProperties.HasFlag(GattCharacteristicProperties.Notify) ||
                                selectedCharacteristic.CharacteristicProperties.HasFlag(GattCharacteristicProperties.Indicate);

                if (canStream)
                {
                    enableRecordUI(true);
                    statusStrip.Text = $"Selected: {selected.ServiceName} - {selected.CharacteristicName}";
                }
                else
                {
                    enableRecordUI(false);
                    statusStrip.Text = "Selected characteristic does not support Notify/Indicate (cannot stream)";
                }
            }
        }

        #endregion

        #region Connection Monitoring and Auto-Reconnect

        private void StartDataWatchdog()
        {
            // Start watchdog timer (checks every 5 seconds)
            dataWatchdogTimer = new System.Threading.Timer(CheckDataFlow, null, WATCHDOG_INTERVAL_MS, WATCHDOG_INTERVAL_MS);
        }

        private void StopDataWatchdog()
        {
            dataWatchdogTimer?.Dispose();
            dataWatchdogTimer = null;
        }

        private void CheckDataFlow(object state)
        {
            if (!streaming)
                return;

            var timeSinceLastData = (DateTime.Now - lastDataReceived).TotalSeconds;

            if (timeSinceLastData > DATA_TIMEOUT_SECONDS)
            {
                BeginInvoke((Action)(() =>
                {
                    statusStrip.Text = $"Warning: No data received for {timeSinceLastData:F0} seconds";
                }));
            }
        }

        private void UpdateStatusPanel(object state)
        {
            if (bluetoothLeDevice == null)
                return;

            BeginInvoke((Action)(() =>
            {
                // Update uptime
                var uptime = DateTime.Now - connectionStartTime;
                statusUptimeLabel.Text = $"{(int)uptime.TotalMinutes}:{uptime.Seconds:D2}";

                // Update packet count
                statusPacketsLabel.Text = packetsReceived.ToString();

                // Update last data timestamp
                var timeSinceLastData = DateTime.Now - lastDataReceived;
                if (timeSinceLastData.TotalSeconds < 60)
                {
                    statusLastDataLabel.Text = $"{timeSinceLastData.TotalSeconds:F1}s ago";
                }
                else
                {
                    statusLastDataLabel.Text = $"{timeSinceLastData.TotalMinutes:F0}m ago";
                }

                // Calculate and update data rate (packets per second over last 60 seconds)
                var now = DateTime.Now;
                while (recentDataTimestamps.Count > 0 && (now - recentDataTimestamps.Peek()).TotalSeconds > 60)
                {
                    recentDataTimestamps.Dequeue();
                }

                double dataRate = recentDataTimestamps.Count / 60.0;
                statusDataRateLabel.Text = $"{dataRate:F1} Hz";

                // Change color based on data flow
                if (streaming && timeSinceLastData.TotalSeconds > DATA_TIMEOUT_SECONDS)
                {
                    statusLastDataLabel.ForeColor = Color.Red;
                }
                else if (streaming)
                {
                    statusLastDataLabel.ForeColor = Color.Green;
                }
                else
                {
                    statusLastDataLabel.ForeColor = Color.Black;
                }
            }));
        }

        private void BluetoothLeDevice_ConnectionStatusChanged(BluetoothLEDevice sender, object args)
        {
            BeginInvoke((Action)(async () =>
            {
                if (sender.ConnectionStatus == BluetoothConnectionStatus.Disconnected && !isReconnecting)
                {
                    statusStrip.Text = "Device disconnected! Attempting to reconnect...";
                    connectionLabel.Text = "Reconnecting...";
                    connectionLabel.ForeColor = Color.Orange;
                    await AttemptReconnectAsync();
                }
            }));
        }

        private async Task AttemptReconnectAsync()
        {
            if (isReconnecting || selectedDevice == null)
                return;

            isReconnecting = true;
            reconnectAttempts = 0;

            while (reconnectAttempts < MAX_RECONNECT_ATTEMPTS && isReconnecting)
            {
                reconnectAttempts++;
                statusStrip.Text = $"Reconnect attempt {reconnectAttempts}/{MAX_RECONNECT_ATTEMPTS}...";

                try
                {
                    // Wait with exponential backoff
                    int delayMs = Math.Min(1000 * (int)Math.Pow(2, reconnectAttempts - 1), 10000);
                    await Task.Delay(delayMs);

                    // Try to reconnect
                    await ClearBluetoothLEDeviceAsync();
                    bluetoothLeDevice = await BluetoothLEDevice.FromIdAsync(selectedDevice.Id);

                    if (bluetoothLeDevice != null && bluetoothLeDevice.ConnectionStatus == BluetoothConnectionStatus.Connected)
                    {
                        // Reconnection successful
                        statusStrip.Text = "Reconnected successfully!";
                        connectionLabel.Text = "Connected";
                        connectionLabel.ForeColor = Color.Green;

                        // Re-subscribe to connection status
                        bluetoothLeDevice.ConnectionStatusChanged += BluetoothLeDevice_ConnectionStatusChanged;

                        // Reset connection stats
                        connectionStartTime = DateTime.Now;
                        reconnectAttempts = 0;
                        isReconnecting = false;

                        // If we were streaming, try to re-establish subscription
                        if (subscribedForNotifications)
                        {
                            await ReestablishNotificationSubscriptionAsync();
                        }

                        return;
                    }
                }
                catch (Exception ex)
                {
                    statusStrip.Text = $"Reconnect attempt {reconnectAttempts} failed: {ex.Message}";
                }
            }

            // Max attempts reached
            isReconnecting = false;
            statusStrip.Text = "Failed to reconnect. Please try connecting manually.";
            connectionLabel.Text = "Disconnected";
            connectionLabel.ForeColor = Color.Red;
            enableRecordUI(false);
        }

        private async Task ReestablishNotificationSubscriptionAsync()
        {
            try
            {
                if (selectedCharacteristic == null)
                    return;

                var cccdValue = GattClientCharacteristicConfigurationDescriptorValue.None;
                if (selectedCharacteristic.CharacteristicProperties.HasFlag(GattCharacteristicProperties.Indicate))
                {
                    cccdValue = GattClientCharacteristicConfigurationDescriptorValue.Indicate;
                }
                else if (selectedCharacteristic.CharacteristicProperties.HasFlag(GattCharacteristicProperties.Notify))
                {
                    cccdValue = GattClientCharacteristicConfigurationDescriptorValue.Notify;
                }

                var status = await selectedCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(cccdValue);

                if (status == GattCommunicationStatus.Success)
                {
                    statusStrip.Text = "Successfully re-established data subscription";
                }
                else
                {
                    statusStrip.Text = "Warning: Could not re-establish data subscription";
                }
            }
            catch (Exception ex)
            {
                statusStrip.Text = $"Error re-establishing subscription: {ex.Message}";
            }
        }

        #endregion

        private void groupBox1_Enter_1(object sender, EventArgs e)
        {

        }
    }
}
using GsmComm.GsmCommunication;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormSMSMultipleInstance
{
    public partial class Main : Form
    {
        public static List<SmsModem> ConnectedDevices = new List<SmsModem>();

        public static string MessageSentToService = ConfigurationManager.AppSettings["Message_Send_Service"];
        public static string MessageShow = ConfigurationManager.AppSettings["Message_Show"];
        public List<Device> DeviceList = new List<Device>();
        public bool IsUpdatingDevice;
        public string RgxInt = "\\d+((.|,)\\d+)?";

        public Main()
        {
            InitializeComponent();
            UpdateListDeviceList();
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            //Windows Message Device is updated
            if (m.Msg == 537)
                UpdateListDeviceList();

            base.WndProc(ref m);
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            UpdateListDeviceList();
        }

        private void UpdateListDeviceList()
        {
            if (IsUpdatingDevice) return;

            try
            {
                DeviceList.Clear();

                var task = Task.Factory.StartNew(UpdateDeviceList);
                Task.WaitAll(task);

                RemoveNotExitingDevice();

                if (DeviceList.Count != 0)
                    foreach (var device in DeviceList.Distinct().ToArray())
                    {
                        var portNumber = GetPortNumber(device.Port);
                        var selectedDevice = ConnectedDevices.FirstOrDefault(c => c.Modem.Port == portNumber);

                        if (selectedDevice == null || !selectedDevice.OGsmModem.IsConnected())
                        {
                            selectedDevice?.TryDisconnect();
                        }
                        else
                        {
                            var selected = DeviceList.FirstOrDefault(c => c.Port == device.Port);

                            if (selected == null) continue;
                            selected.Status = "Connected";
                            selected.DeviceNumber = selectedDevice.GetOwnNumber();
                            RefreshGridView();
                        }
                    }
                grdDevices.DataSource = null;
                grdDevices.DataSource = DeviceList;

                RefreshGridView();
                RefreshButton();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void RemoveNotExitingDevice()
        {
            var notExistDevice =
                ConnectedDevices.Where(c => !DeviceList.Exists(b => c.Modem.Port == GetPortNumber(b.Port)));

            foreach (var device in notExistDevice)
                device.TryDisconnect();

            ConnectedDevices.RemoveAll(c => !DeviceList.Exists(b => c.Modem.Port == GetPortNumber(b.Port)));
        }

        private static void ExecuteBatchFileDriver()
        {
            try
            {
                using (var process = new Process())
                {
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    process.StartInfo.FileName = "SwitchBatch.bat";
                    process.Start();
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateDeviceList()
        {
            IsUpdatingDevice = true;

            try
            {
                var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity");
                var deviceList = searcher.Get();
                foreach (var device in deviceList)
                    try
                    {
                        var deviceName = device["Name"].ToString();

                        if (deviceName.Contains("USBModem Disk"))
                            ExecuteBatchFileDriver();

                        if (!deviceName.Contains("UART") && !deviceName.Contains("Application")) continue;

                        var i = deviceName.IndexOf("COM", StringComparison.Ordinal);
                        var arr = deviceName.ToCharArray();
                        var str = "COM" + arr[i + 3];
                        if (arr[i + 4] != ')')
                            str += arr[i + 4];

                        //UPDATE LIST OF MODEM
                        DeviceList.Add(new Device(str));
                    }
                    catch
                    {
                        //do noting sometime throw error on device name
                    }
            }
            catch
            {
                //do noting sometimes throw error fetch device list
            }

            IsUpdatingDevice = false;
        }

        private int GetPortNumber(string portName)
        {
            var rgx = new Regex(RgxInt, RegexOptions.Compiled);
            var deviceName = portName;
            var matches = rgx.Matches(deviceName)[0].Value;
            int.TryParse(matches, out int portNumber);

            return portNumber;
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow selected in grdDevices.SelectedRows)
                Task.Factory.StartNew(() => ConnectDevice(selected.Index));
        }

        private void RefreshGridView()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker) RefreshGridView);
                return;
            }
            lblDevicesCount.Text = DeviceList.Count.ToString();
            grdDevices.Enabled = DeviceList.Count != 0;

            grdDevices.Refresh();
        }

        private void ConnectDevice(int index)
        {
            var device = new SmsModem();

            try
            {
                var deviceName = grdDevices.Rows[index].Cells[0].Value.ToString();
                var portNumber = GetPortNumber(deviceName);

                var selectedDevice = ConnectedDevices.FirstOrDefault(c => c.Modem.Port == portNumber);

                if (selectedDevice != null && selectedDevice.OGsmModem.IsConnected()) return;

                DeviceList[index].Status = "Connecting...";
                RefreshGridView();

                var modem = new ModemConfig
                {
                    Port = portNumber,
                    BaudRate = GsmCommMain.DefaultBaudRate,
                    Timeout = GsmCommMain.DefaultTimeout
                };

                device = new SmsModem
                {
                    Modem = modem
                };
                device.ConnectModem();

                device.OGsmModem.MessageReceived += Comm_MessageReceived;
                //update Status
                ConnectedDevices.Add(device);

                DeviceList[index].Status = "Connected";
                DeviceList[index].DeviceNumber = device.GetOwnNumber();
                RefreshGridView();
            }
            catch (Exception error)
            {
                DeviceList[index].Status = error.Message;
                RefreshGridView();
                device.TryDisconnect();
            }

            RefreshButton();
        }

        private static void Comm_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            try
            {
                var modemNewSms =
                    ConnectedDevices.FirstOrDefault(c => c.Modem.Port == ((GsmCommMain) sender).PortNumber);

                Task.Factory.StartNew(() =>
                {
                    if (modemNewSms != null) ReadUnreadMessages(modemNewSms);
                });
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private static void ReadUnreadMessages(SmsModem device)
        {
            if (device == null) return;
            var messages = device.ReadMessageUnread();

            foreach (var sms in messages)
            {
                bool.TryParse(MessageSentToService, out bool sendtoservice);
                bool.TryParse(MessageShow, out bool messageshow);

                if (sendtoservice)
                    SmsPostClient.PostSms(sms);

                if (messageshow)
                    MessageBox.Show(
                        $@"Device {device.Modem.Port} Has New Message From ({sms.Sender}), Contains: {sms.Message}",
                        device.GetOwnNumber());

                device.DeleteMessage(sms.Index);
            }
        }

        private void BtnDisconnect_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow selected in grdDevices.SelectedRows)
            {
                var deviceName = grdDevices.Rows[selected.Index].Cells[0].Value.ToString();
                var portNumber = GetPortNumber(deviceName);

                var selectedDevice = ConnectedDevices.FirstOrDefault(c => c.Modem.Port == portNumber);

                if (selectedDevice == null) return;
                if (!DisconnectDevice(selectedDevice)) continue;

                DeviceList[selected.Index].Status = "Ready to Connect";
                DeviceList[selected.Index].DeviceNumber = string.Empty;
                grdDevices.Refresh();
                RefreshButton();
            }
        }

        private static bool DisconnectDevice(SmsModem device)
        {
            var isDisconnected = false;

            device.TryDisconnect();
            ConnectedDevices.Remove(device);

            device.OGsmModem.MessageReceived -= Comm_MessageReceived;

            if (!device.OGsmModem.IsConnected())
                isDisconnected = true;

            return isDisconnected;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var device in ConnectedDevices)
                device.TryDisconnect();
        }

        private void BtnUpdateNumber_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(UpdateContacNumber);
        }

        private void UpdateContacNumber()
        {
            var newContact = string.Empty;
            var index = grdDevices.SelectedRows[0].Index;
            var deviceName = grdDevices.SelectedRows[0].Cells[0].Value.ToString();
            var portNumber = GetPortNumber(deviceName);

            var selectedDevice = ConnectedDevices.FirstOrDefault(c => c.Modem.Port == portNumber);
            InputBox.ShowInputDialog(ref newContact);

            if (selectedDevice != null && newContact != string.Empty)
            {
                DeviceList[index].DeviceNumber = "Updating..";

                RefreshGridView();

                newContact = selectedDevice.UpdateOwnContact(newContact) ? newContact : "* " + newContact;

                selectedDevice.SetOwnNumber(newContact);

                DeviceList[index].DeviceNumber = selectedDevice.GetOwnNumber();
            }

            RefreshGridView();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow selected in grdDevices.SelectedRows)
                Task.Factory.StartNew(() => UpdateDevice(selected.Index));
        }

        private void BtnUpdateAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow selected in grdDevices.Rows)
                Task.Factory.StartNew(() => UpdateDevice(selected.Index));
        }

        private void UpdateDevice(int index)
        {
            try
            {
                if (DeviceList[index].Status == "Connected") return;

                var deviceName = grdDevices.Rows[index].Cells[0].Value.ToString();
                DeviceList[index].Status = "Checking...";
                RefreshGridView();

                var portNumber = GetPortNumber(deviceName);
                var modem = new ModemConfig
                {
                    Port = portNumber,
                    BaudRate = GsmCommMain.DefaultBaudRate,
                    Timeout = GsmCommMain.DefaultTimeout
                };

                var device = new SmsModem
                {
                    Modem = modem
                };

                if (device.TryConnectModem())
                {
                    DeviceList[index].Status = "Ready to Connect";
                    RefreshGridView();
                }
                else
                {
                    DeviceList[index].Status = "N/A";
                    RefreshGridView();
                }
            }
            catch (Exception ex)
            {
                DeviceList[index].Status = ex.Message;
                RefreshGridView();
            }
        }

        private void grdDevices_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var index = grdDevices.SelectedRows[0].Index;
            var deviceName = grdDevices.Rows[index].Cells[0].Value.ToString();
            var portNumber = GetPortNumber(deviceName);

            var selectedDevice = ConnectedDevices.FirstOrDefault(c => c.Modem.Port == portNumber);

            if (selectedDevice == null)
            {
                Task.Factory.StartNew(() => ConnectDevice(index));
                return;
            }

            if (!DisconnectDevice(selectedDevice)) return;

            DeviceList[index].Status = "Ready to Connect";
            DeviceList[index].DeviceNumber = string.Empty;
            grdDevices.Refresh();
            RefreshButton();
        }

        private void grdDevices_SelectionChanged(object sender, EventArgs e)
        {
            RefreshButton();
        }

        private void RefreshButton()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker) RefreshButton);
                return;
            }

            try
            {
                var numberOfSelectedDevice = grdDevices.SelectedRows.Count;

                var deviceName = grdDevices.SelectedRows[0].Cells[0].Value.ToString();
                var portNumber = GetPortNumber(deviceName);

                var selectedDevice = ConnectedDevices.FirstOrDefault(c => c.Modem.Port == portNumber);
                if (selectedDevice != null)
                {
                    btnConnect.Enabled = !selectedDevice.OGsmModem.IsConnected();
                    btnUpdateNumber.Enabled = selectedDevice.OGsmModem.IsConnected();
                    btnDisconnect.Enabled = selectedDevice.OGsmModem.IsConnected();
                }
                else
                {
                    btnConnect.Enabled = true;
                    btnUpdateNumber.Enabled = false;
                    btnDisconnect.Enabled = false;
                }

                btnUpdateNumber.Enabled = btnUpdateNumber.Enabled && numberOfSelectedDevice == 1;
            }
            catch
            {
                //do nothing
            }
        }

        private void grdDevices_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow selected in grdDevices.SelectedRows)
                Task.Factory.StartNew(() => ReadMessage(selected.Index));
        }

        private void ReadMessage(int index)
        {
            try
            {
                var deviceName = grdDevices.Rows[index].Cells[0].Value.ToString();
                var portNumber = GetPortNumber(deviceName);
                var selectedDevice = ConnectedDevices.FirstOrDefault(c => c.Modem.Port == portNumber);

                if (selectedDevice == null) return;

                var allmessaged = selectedDevice.ReadAllMessage();

                foreach (var sms in allmessaged)
                {
                    MessageBox.Show(
                        $@"Device {selectedDevice.Modem.Port} Has New Message From ({sms.Sender}), Contains: {
                                sms.Message
                            }", selectedDevice.GetOwnNumber());
                    selectedDevice.DeleteMessage(sms.Index);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
    }
}
using GsmComm.GsmCommunication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Management;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormSMSMultipleInstance
{
    public partial class Main : Form
    {
        public List<string> DeviceList = new List<string>();
        private Object Lock = new Object();
        public DataTable dtMessages { get; set; }
        public string ownNumber;
        public static List<SMSModem> devices = new List<SMSModem> { };
        public string rgxInt = "\\d+((.|,)\\d+)?";
        
        public Main()
        {
            InitializeComponent();
            UpdateListDeviceList();
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            //Windows Message
            switch (m.Msg)
            {
                case 537: //DETECT VM_DEVICE Manger Changes
                    UpdateListDeviceList();
                    break;
            }
            base.WndProc(ref m);
        }

        private void UpdateListDeviceList()
        {
            try
            {
                lstAvailableDevice.Items.Clear();
                DeviceList.Clear();

                Task task1 = Task.Factory.StartNew(() => UpdateDeviceList());
                Task.WaitAll(task1);

                RemoveNotExitingDevice();

                foreach (var device in DeviceList.Distinct().ToArray())
                {
                    var isConnected = false;
                    var portNumber = GetPortNumber(device);
                    var SelectedDevice = devices.Where(c => c.Modem.Port == portNumber).FirstOrDefault();
                    if (SelectedDevice != null)
                    {
                        isConnected = SelectedDevice.OGsmModem.IsConnected();
                    }
                    
                    if (!(lstAvailableDevice.Items.Contains(device) || lstAvailableDevice.Items.Contains("*" + device)))
                    {
                        lstAvailableDevice.Items.Add( isConnected ? "*" + device : device);
                    }
                }

                if (lstAvailableDevice.Items.Count > 0)
                    lstAvailableDevice.SetSelected(0, true);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void RemoveNotExitingDevice()
        {
            var notExistDevice = devices.Where(c => !DeviceList.Exists(b => c.Modem.Port == GetPortNumber(b)));

            foreach (var device in notExistDevice)
            {
                device.TryDisconnect();
            }

            devices.RemoveAll(c => !DeviceList.Exists(b => c.Modem.Port == GetPortNumber(b)));
        }

        private void UpdateDeviceList()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity");
                //ERROR ON THIS IF SAME THREAD
                var deviceList = searcher.Get();
                foreach (var device in deviceList)
                {
                    try
                    {
                        var DeviceName = device["Name"].ToString();
                        if (DeviceName.Contains("Application"))
                        {
                            int i = DeviceName.IndexOf("COM");
                            char[] arr = DeviceName.ToCharArray();
                            var str = "COM" + arr[i + 3];
                            if (arr[i + 4] != ')')
                            {
                                str += arr[i + 4];
                            }

                            //UPDATE LIST OF MODEM
                            DeviceList.Add(str);
                        }
                    }
                    catch
                    {
                        //do noting
                    }
                }
            }
            catch (Exception e)
            {
                //do noting
            }
        }

        private int GetPortNumber(string portName)
        {
            var rgx = new Regex(rgxInt, RegexOptions.Compiled);
            var DeviceName = portName;
            var matches = rgx.Matches(DeviceName)[0].Value;
            int.TryParse(matches, out int portNumber);

            return portNumber;
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            var device = new SMSModem();

            try
            {
                var DeviceName = lstAvailableDevice.SelectedItem.ToString().Replace("*", "");
                var portNumber = GetPortNumber(DeviceName);
                var modem = new ModemConfig
                {
                    Port = portNumber,
                    BaudRate = GsmCommMain.DefaultBaudRate,
                    Timeout = GsmCommMain.DefaultTimeout
                };

                device = new SMSModem
                {
                    Modem = modem
                };
                device.ConnectModem();

                device.OGsmModem.MessageReceived += new MessageReceivedEventHandler(Comm_MessageReceived);
                devices.Add(device);

                lstAvailableDevice.Items[lstAvailableDevice.SelectedIndex] = device.OGsmModem.IsConnected() ? "*" + DeviceName : DeviceName;

                btnConnect.Enabled = !device.OGsmModem.IsConnected();
                btnDisconnect.Enabled = device.OGsmModem.IsConnected();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
                device.TryDisconnect();
            }
        }

        private void Comm_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            try
            {
                var modemNewSms = devices.Where(c => c.Modem.Port == ((GsmCommMain)sender).PortNumber).FirstOrDefault();
                var messages = modemNewSms.ReadMessageUnread();

                foreach (var sms in messages)
                {
                    MessageBox.Show($"Device {modemNewSms.Modem.Port} Has New Message From ({sms.Sender}), Contains: {sms.Message}",modemNewSms.OwnNumber);
                    modemNewSms.DeleteMessage(sms.Index);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
        
        private void BtnCheckSMS_Click(object sender, EventArgs e)
        {
            //CheckMessage();
        }

        private void BtnDisconnect_Click(object sender, EventArgs e)
        {
            var DeviceName = lstAvailableDevice.SelectedItem.ToString();
            var portNumber = GetPortNumber(DeviceName);

            var SelectedDevice = devices.Where(c => c.Modem.Port == portNumber).FirstOrDefault();

            if (DisconnectDevice(SelectedDevice))
            {
                lstAvailableDevice.Items[lstAvailableDevice.SelectedIndex] = $"{DeviceName.Replace("*", "")}";
            }
        }

        private bool DisconnectDevice(SMSModem device)
        {
            var isDisconnected = false;
            
            device.TryDisconnect();
            devices.Remove(device);

            device.OGsmModem.MessageReceived -= new MessageReceivedEventHandler(Comm_MessageReceived);

            if (!device.OGsmModem.IsConnected())
            {
                isDisconnected = true;
            }

            return isDisconnected;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var device in devices)
            {
                device.TryDisconnect();
            }
        }

        private void LstAvailableDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (lstAvailableDevice.SelectedIndex != -1)
                {
                    var DeviceName = lstAvailableDevice.SelectedItem.ToString();
                    var portNumber = GetPortNumber(DeviceName);

                    var SelectedDevice = devices.Where(c => c.Modem.Port == portNumber).FirstOrDefault();
                    if (SelectedDevice != null)
                    {   
                        btnConnect.Enabled = !SelectedDevice.OGsmModem.IsConnected();
                        btnDisconnect.Enabled = SelectedDevice.OGsmModem.IsConnected();
                    }
                    else
                    {
                        btnConnect.Enabled = true;
                        btnDisconnect.Enabled = false;
                    }
                }
            }
            catch
            {
                //do nothing
            }
        }
    }
}

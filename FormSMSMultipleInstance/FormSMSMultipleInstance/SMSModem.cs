using System;
using System.Linq;
using GsmComm.GsmCommunication;
using GsmComm.PduConverter;
using System.Data;
using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace FormSMSMultipleInstance
{
    public class SMSModem : IDisposable
    {
        public ModemConfig Modem { get; set; }
        public GsmCommMain OGsmModem { get; private set; }
        public string OwnNumber { get; private set; }

        bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        public void ConnectModem()
        {
            OGsmModem = new GsmCommMain(Modem.Port, Modem.BaudRate, Modem.Timeout);

            OGsmModem.Open();
            OGsmModem.EnableMessageNotifications();
            var ownContactDetails = OGsmModem.GetPhonebook(PhoneStorageType.Sim).FirstOrDefault();
            var x = OGsmModem.GetPhonebook(PhoneStorageType.Sim);
            var y = x.Where(c => c.Number.Contains("Own"));

            OwnNumber = ownContactDetails.Number.Split(',')[0].Replace("\"", "");
            ////OGsmModem.PhoneConnected += new EventHandler(comm_PhoneConnected);
            //OGsmModem.MessageReceived += new MessageReceivedEventHandler(comm_MessageReceived);
        }

        public List<SMSContext> ReadMessageUnread()
        {
            var Messages = new List<SMSContext>();

            var SimSMS = OGsmModem.ReadMessages(PhoneMessageStatus.ReceivedUnread, PhoneStorageType.Sim);

            foreach (var sms in SimSMS)
            {
                Messages.Add(ShowMessage(sms.Data, sms.Index));
            }

            return Messages;
        }

        private SMSContext ShowMessage(SmsPdu pdu, int index)
        {
            var SmsDeliverPdu = (SmsDeliverPdu)pdu;

            var sms = new SMSContext()
            {
                Index = index,
                Sender = SmsDeliverPdu.SmscAddress,
                Message = SmsDeliverPdu.UserDataText,
                Datetime = SmsDeliverPdu.SCTimestamp.ToString(),
            };

            return sms;
        }

        public void DeleteMessage(int index)
        {
            OGsmModem.DeleteMessage(index, PhoneStorageType.Sim);
        }

        public void DeleteMessages()
        {
            OGsmModem.DeleteMessages(DeleteScope.All, PhoneStorageType.Sim);
        }

        public void TryDisconnect()
        {
            if (OGsmModem != null && OGsmModem.IsOpen())
            {
                OGsmModem.Close();
                Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
            }

            disposed = true;
        }

        //private void Comm_MessageReceived(object sender, MessageReceivedEventArgs e)
        //{
        //    try
        //    {
        //        var messages = OGsmModem.ReadMessages(PhoneMessageStatus.ReceivedUnread, PhoneStorageType.Sim);

        //        foreach (var message in messages)
        //        {
        //            // error on thread disregard
        //            ShowMessage(message.Data, message.Index);

        //        }

        //    }
        //    catch (Exception error)
        //    {
        //        //MessageBox.Show(error.Message);
        //    }
        //}

        //private void CheckMessage()
        //{
        //    try
        //    {
        //        var SimSMS = OGsmModem.ReadMessages(PhoneMessageStatus.All, PhoneStorageType.Sim);

        //        foreach (var message in SimSMS)
        //        {
        //            ShowMessage(message.Data, message.Index);
        //        }
        //    }
        //    catch (Exception error)
        //    {
        //        //MessageBox.Show(error.Message);
        //    }
        //}

        //private SMSContext ShowMessage(SmsPdu pdu, int index)
        //{
        //    //DECRIPT SMS
        //    var SmsDeliverPdu = (SmsDeliverPdu)pdu;

        //    var sms = new SMSContext()
        //    {
        //        Index = index,
        //        Sender = SmsDeliverPdu.SmscAddress,
        //        Message = SmsDeliverPdu.UserDataText,
        //        Datetime = SmsDeliverPdu.SCTimestamp.ToString(),
        //    };

        //    return sms;

        //    //if ((pdu.GetType() == SmsSubmitPdu))
        //    //{
        //    //    //  Stored (sent/unsent) message
        //    //    SmsSubmitPdu data;
        //    //    pdu;
        //    //    SmsSubmitPdu;
        //    //    return;
        //    //}

        //    //if ((pdu.GetType() == SmsDeliverPdu))
        //    //{
        //    //    //  Received message
        //    //    SmsDeliverPdu data;
        //    //    pdu;
        //    //    SmsDeliverPdu;
        //    //    BindGridsim(pdu, index);
        //    //    return;
        //    //}

        //    //if ((pdu.GetType() == SmsStatusReportPdu))
        //    //{
        //    //    //  Status report
        //    //    SmsStatusReportPdu data;
        //    //    pdu;
        //    //    SmsStatusReportPdu;
        //    //    return;
        //    //}

        //}

        //private void Comm_PhoneConnected(object sender, EventArgs e)
        //{
        //    //throw new NotImplementedException();
        //}

    }
}

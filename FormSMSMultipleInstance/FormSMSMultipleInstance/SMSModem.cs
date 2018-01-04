﻿using System;
using System.Linq;
using GsmComm.GsmCommunication;
using GsmComm.PduConverter;
using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace FormSMSMultipleInstance
{
    public class SmsModem : IDisposable
    {
        public ModemConfig Modem { get; set; }
        public GsmCommMain OGsmModem { get; private set; }
        public string SimStorage = PhoneStorageType.Sim;

        private string _ownNumber;

        //public string GetOwnNumber()
        //{
        //    return ownNumber;
        //}

        public void SetOwnNumber(string value)
        {
            _ownNumber = value;
        }

        bool _disposed;
        readonly SafeHandle _handle = new SafeFileHandle(IntPtr.Zero, true);

        public void ConnectModem()
        {
            OGsmModem = new GsmCommMain(Modem.Port, Modem.BaudRate, Modem.Timeout);
            OGsmModem.Open();
            OGsmModem.EnableMessageNotifications();
            GetOwnNumber();
            ////OGsmModem.PhoneConnected += new EventHandler(comm_PhoneConnected);
            //OGsmModem.MessageReceived += new MessageReceivedEventHandler(comm_MessageReceived);
        }

        public bool TryConnectModem()
        {
            OGsmModem = new GsmCommMain(Modem.Port, Modem.BaudRate, Modem.Timeout); 
            OGsmModem.Open();
            var isAvailable = OGsmModem.IsConnected();
            OGsmModem.Close();

            return isAvailable;
        }

        public string GetOwnNumber()
        {
            if (_ownNumber != null) return _ownNumber;

            //var ownContactDetails = OGsmModem.GetPhonebook(PhoneStorageType.Sim).FirstOrDefault();
            
            var contactList = OGsmModem.GetPhonebook(PhoneStorageType.Sim);
            var contactOwnNumber = contactList.FirstOrDefault(c => c.Text.Contains("Own"));

            SetOwnNumber(contactOwnNumber == null ? "No Own Contact, Please Update!" : contactOwnNumber.Number);

            //SetOwnNumber(ownContactDetails.Number.Split(',')[0].Replace("\"", ""));

            return _ownNumber;
        }

        public void UpdateOwnContact(string OwnContact)
        {
            var ownContactDetails = OGsmModem.GetPhonebook(PhoneStorageType.Sim).FirstOrDefault();
            OGsmModem.DeleteAllPhonebookEntries(SimStorage);
            var ownContact = new PhonebookEntry(1, OwnContact, 145, "Own");           

            OGsmModem.CreatePhonebookEntry(ownContact, SimStorage);            

        }

        public List<SMSContext> ReadMessageUnread()
        {
            var simSms = OGsmModem.ReadMessages(PhoneMessageStatus.ReceivedUnread, SimStorage);

            return simSms.Select(sms => ShowMessage(sms.Data, sms.Index)).ToList();
        }

        private static SMSContext ShowMessage(SmsPdu pdu, int index)
        {
            var smsDeliverPdu = (SmsDeliverPdu)pdu;

            var sms = new SMSContext()
            {
                Index = index,
                Sender = smsDeliverPdu.SmscAddress,
                Message = smsDeliverPdu.UserDataText,
                Datetime = smsDeliverPdu.SCTimestamp.ToString(),
            };

            return sms;
        }

        public void DeleteMessage(int index)
        {
            OGsmModem.DeleteMessage(index, SimStorage);
        }

        public void DeleteMessages()
        {
            OGsmModem.DeleteMessages(DeleteScope.All, SimStorage);
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
            if (_disposed)
                return;

            if (disposing)
            {
                _handle.Dispose();
            }

            _disposed = true;
        }

        //private void Comm_MessageReceived(object sender, MessageReceivedEventArgs e)
        //{
        //    try
        //    {
        //        var messages = OGsmModem.ReadMessages(PhoneMessageStatus.ReceivedUnread, SimStorage);

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
        //        var SimSMS = OGsmModem.ReadMessages(PhoneMessageStatus.All, SimStorage);

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

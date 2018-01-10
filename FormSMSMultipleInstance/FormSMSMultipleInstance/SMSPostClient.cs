using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;

namespace FormSMSMultipleInstance
{
    internal static class SmsPostClient
    {
        public static void PostSms(SMSContext sms)
        {
            var hostCommandCenter = ConfigurationManager.AppSettings["HOST_COMMAND_CENTER"];

            var client = new HttpClient { Timeout = new TimeSpan(0, 0, 10) };
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("to", sms.Reciever),
                new KeyValuePair<string, string>("content", sms.Message),
                new KeyValuePair<string, string>("from", sms.Sender),
            });
            using (var rsp = client.PostAsync($"{hostCommandCenter}/api/sms/post", content).Result)
            {
                var page = rsp.Content.ReadAsStringAsync().Result;
            }
        }
    }
}

using System;
using System.Net;
using System.Text;
using OrderTracking.Dao.Domain;

namespace OrderTrackingAdmin.Mvc.Utilities
{
    public static class SmsCommands
    {
        public static string RunAdminSmsSetup(string smsUserName, string smsPassword, string smsTo, string smsFrom, string smsMessage)
        {
            //"http://www.24x.com/sendsms/sendsms.aspx?user=sck02411&password=QLQXFS&smsto=447949483082&smsfrom=447949483082&smsmsg=Hello"

            var send = string.Format(
                "http://www.24x.com/sendsms/sendsms.aspx?user={0}&password={1}&smsto={2}&smsfrom={3}&smsmsg={4}", smsUserName, smsPassword, smsTo, smsFrom, smsMessage);

            var request = (HttpWebRequest)
                WebRequest.Create(send);

            var sb = new StringBuilder();
            var buf = new byte[8192];
            var resStream = request.GetResponse().GetResponseStream();
            var count = 0;

            do
            {
                count = resStream.Read(buf, 0, buf.Length);

                if (count != 0)
                {
                    sb.Append(Encoding.ASCII.GetString(buf, 0, count));
                }
            }
            while (count > 0);

            //todo: error check the return codes???

            //http://www.24x.com/sendsms/sendsms.aspx?user=sck02411&password=QLQXFS&smsto=447949483082&smsfrom=447949483082&smsmsg=Hello

            return sb.ToString();
        }

        public static string BuildCommand(Tracker tracker, TrackerCommand trackerCommand)
        {
            var cmd = new StringBuilder();

            //note: this is coded purely for GT30
            cmd.Append(trackerCommand.Command);

            if (trackerCommand.Command.Contains("{deviceId}"))
                cmd.Replace("{deviceId}", tracker.Name);

            if (trackerCommand.Command.Contains("{apnName}"))
                cmd.Replace("{apnName}", tracker.Apn.Name);

            if (trackerCommand.Command.Contains("{apnUserName}"))
                cmd.Replace("{apnUserName}", tracker.Apn.Username);

            if (trackerCommand.Command.Contains("{apnPassword}"))
                cmd.Replace("{apnPassword}", tracker.Apn.Password);    

            return cmd.ToString();
        }
    }
}

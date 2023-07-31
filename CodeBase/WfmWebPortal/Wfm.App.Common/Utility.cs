using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Wfm.App.Logging;
using static Wfm.App.Core.Enums;

namespace Wfm.App.Common
{
    public class Utility
    {

        public static List<string> NavBarActions = new List<string> { "MyProfile", "ChangePassword" };

        public static void LogMessagesNLog(LogLevels loglevel,string ApplicationId ,string Namespace, string Classname, string Functionname, string area, string message)
        {
            NLogWrap.LogMessage("WFMUI",loglevel, ApplicationId, Namespace,Classname,Functionname,area,message);
        }

        public static string FormatDate(DateTime? date)
        {
            if(date != null)
                return date.Value.ToString("dd/MM/yyyy");

            return string.Empty;
        }

        public static string FormatDate(DateTime date)
        {
            if (date != null)
                return date.ToString("dd/MM/yyyy");

            return string.Empty;
        }

        public static Guid GetLoggedInUserId()
        {
            return SessionHelper.Get<Guid>("UserId");
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}

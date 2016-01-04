using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDashboard.Admin
{
    public class Obfuscator
    {
        public static string encryptString(string number)
        {
            string encryptedTempString = "";

            try
            {
                string tempString = number;
                DateTime tempDate = DateTime.UtcNow;
                string[] keyParts = new string[3];
                if (tempString != null)
                {
                    keyParts[0] = tempString.Substring(0, 2); // first 2
                    tempString = tempString.Substring(2);
                    keyParts[1] = tempString.Substring(tempString.Length - 1); // last digit
                    tempString = tempString.Remove(tempString.Length - 1);
                    keyParts[2] = tempString; // site ID

                }

                encryptedTempString = Obfuscator.exactString((int.Parse(keyParts[2]) + 1000 + tempDate.Year).ToString(), 6) + Obfuscator.exactString((int.Parse(keyParts[0]) + tempDate.Day).ToString(), 2) + exactString((int.Parse(keyParts[1]) + tempDate.Month).ToString(), 2);
            }
            catch { }
                
            return encryptedTempString;
        }


        public static string decryptString(string encryptedString)
        {
            string decryptedString = "";

            try
            {
                string[] decryptedArray = new string[3];
                if (encryptedString != null)
                {
                    decryptedArray[0] = encryptedString.Substring(0, 6); // first 2
                    decryptedArray[0] = decryptedArray[0].Replace("A", "");
                    encryptedString = encryptedString.Substring(6);
                    decryptedArray[1] = encryptedString.Substring(0, 2); // last digit
                    decryptedArray[1] = decryptedArray[1].Replace("A", "");
                    encryptedString = encryptedString.Substring(2);
                    decryptedArray[2] = encryptedString; // site ID
                    decryptedArray[2] = decryptedArray[2].Replace("A", "");

                }

                // Calcualte dates
                DateTime todaysDate = DateTime.UtcNow;
                DateTime yesterdaysDate = todaysDate.AddDays(-1);
                DateTime tomorrowsDate = todaysDate.AddDays(+1);

                // This is the actual date/time that we will use for decryption
                DateTime decryptDateTime = todaysDate;

                // We can use the first two characters of the encrypted string to determine what day the string was encrypted.
                // Convert the first two characters to an integer
                int startChunk = 0;
                if (Int32.TryParse(decryptedArray[1], out startChunk))
                {
                    // 54 is added to the day when encrypted
                    startChunk -= 54;

                    // Was the encrypted string encrypted today?
                    if (startChunk == todaysDate.Day)
                    {
                        // Use todays date to decrypt
                        decryptDateTime = todaysDate;
                    }
                    // Was the string encrypted yesterday?
                    else if (startChunk == yesterdaysDate.Day)
                    {
                        // Use yesterdays date to decrypt
                        decryptDateTime = yesterdaysDate;
                    }
                    // Was the string encrypted tommorrow?
                    else if (startChunk == tomorrowsDate.Day)
                    {
                        // Use tomorrows date to decrypt
                        decryptDateTime = tomorrowsDate;
                    }
                }

                decryptedString =
                    (int.Parse(decryptedArray[1]) - decryptDateTime.Day).ToString() +
                    (int.Parse(decryptedArray[0]) - 1000 - decryptDateTime.Year).ToString() +
                    (int.Parse(decryptedArray[2]) - decryptDateTime.Month).ToString();
            }
            catch { }

            return decryptedString;
        }

        private static string exactString(string str, int dimension)
        {
            string newString = str;
            if (str != null)
            {
                for (int i = 0; i < (dimension - str.Length); i++)
                    newString = newString + "A";
            }
            return newString;
        }

    }
}
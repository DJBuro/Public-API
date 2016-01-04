using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MVCAndromeda.Models
{
    public static class CultureName
    {
        private static Dictionary<string, string> culture = null;
        /*new Dictionary<string, string>()
        {
        {"Guam","en-US"},
        {"United States","en-US"},
        {"Russian Federation","ru-RU"},
        {"Turkey","tr"},
        {"United Kingdom","en-GB"},
        {"Azerbaijan","az"},
        {"Canada","en-Ca"}
        };*/

        //---This will probably get updated only on the application re-launch as it is all static
        public static string getName(string country)
        {
            if (culture == null)
            {
                string filePath = HttpContext.Current.Server.MapPath("~/Content/CultureName.txt");
                culture = new Dictionary<string, string>();

                using (StreamReader sr = new StreamReader(filePath))
                {
                    while (!sr.EndOfStream)
                    {
                        string[] res = sr.ReadLine().Split(',');
                        culture.Add(res[0], res[1]);
                    }
                }
            }
            return culture[country];
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;
using System.IO;
using System.Text;
using System.Threading;

namespace WebSiteList.Controllers
{
    public class SiteListController : ApiController
    {
        public string Get()
        {
            string filePath = HostingEnvironment.MapPath("~/app_data");
            string file = filePath + "/SiteList.txt";
            if (!File.Exists(file)) return "";
            try
            {
                return File.ReadAllText(file);
            }
            catch
            {
                return "";
            }
        }

        public void Post([FromBody] string value)
        {
            string siteList = value ;
            string filePath=HostingEnvironment.MapPath("~/app_data");
            string tempfile = filePath + "/SiteList."+Guid.NewGuid().ToString();
            string outfile = filePath + "/SiteList.txt";
            File.WriteAllText(outfile, tempfile);
            Overwrite(tempfile, outfile);
        }

        //public void Put(string value)
        //{
        //    string filePath = HostingEnvironment.MapPath("~/app_data");
        //    string file = filePath + "/SiteList.txt";
        //    File.WriteAllText(file, value);
        //}

        private void Overwrite(string infile, string outfile)
        {
            // If outfile exists, delete it
            int loop=0;
            while (loop < 5)
            {
                try
                {
                    if (File.Exists(outfile))
                    {
                        File.Delete(outfile);
                        File.Move(infile, outfile);
                    }
                    else
                        File.Move(infile, outfile);
                    return;
                }
                catch
                {

                }
                Thread.Sleep(1000);
                loop++;
            }
            // Move infile to outfile
        }
    }
}

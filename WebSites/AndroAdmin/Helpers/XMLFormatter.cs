using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace AndroAdmin.Helpers
{
    public class XMLFormatter
    {
        public static string FormatXml(string input)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(input);

            using (StringWriter buffer = new StringWriter())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;

                using (XmlWriter writer = XmlWriter.Create(buffer, settings))
                {
                    doc.WriteTo(writer);

                    writer.Flush();
                }

                buffer.Flush();

                return buffer.ToString();
            }
        }
    }
}
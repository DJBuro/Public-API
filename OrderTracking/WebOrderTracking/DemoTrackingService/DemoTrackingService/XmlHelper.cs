using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml.XPath;

namespace DemoTrackingService
{
    public class XmlHelper
    {
        internal static List<Order> ExtractOrdersFromXml(string orderXml)
        {
            List<Order> orders = new List<Order>();

            using (StringReader stream = new StringReader(orderXml))
            {
                XPathDocument document = new XPathDocument(stream);
                XPathNavigator navigator = document.CreateNavigator();
                XPathNodeIterator orderNodes = navigator.Select("/Orders/Order");

                while (orderNodes.MoveNext())
                {
                    // Get the tracker details out of the xml
                    string storeLatitude = orderNodes.Current.GetAttribute("storeLat", "");
                    string storeLongitude = orderNodes.Current.GetAttribute("storeLon", "");
                    string customerLatitude = orderNodes.Current.GetAttribute("custLat", "");
                    string customerLongitude = orderNodes.Current.GetAttribute("custLon", "");
                    string trackerLatitude = orderNodes.Current.GetAttribute("lat", "");
                    string trackerLongitude = orderNodes.Current.GetAttribute("lon", "");
                    string personProcessing = orderNodes.Current.GetAttribute("personProcessing", "");
                    string status = orderNodes.Current.GetAttribute("status", "");

                    orders.Add(new Order(storeLatitude, storeLongitude, customerLatitude, customerLongitude, trackerLatitude, trackerLongitude, personProcessing, status));                   
                }
            }

            return orders;
        }
    }
}
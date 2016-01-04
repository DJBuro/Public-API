using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AndroCloudDataAccess.Domain;
using System.Runtime.Serialization;

namespace AndroCloudServices.Domain
{
    [XmlRoot(ElementName="Sites")]
    public class Sites : List<AndroCloudDataAccess.Domain.Site>
    {
    }
}

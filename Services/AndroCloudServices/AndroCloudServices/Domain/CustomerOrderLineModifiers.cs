﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AndroCloudDataAccess.Domain;
using System.Runtime.Serialization;

namespace AndroCloudServices.Domain
{
    [XmlRoot(ElementName = "Modifiers")]
    public class CustomerOrderLineModifiers : List<CustomerOrderLineModifier>
    {
    }
}

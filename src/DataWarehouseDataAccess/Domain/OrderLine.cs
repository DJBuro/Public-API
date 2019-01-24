﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;

namespace DataWarehouseDataAccess.Domain
{
    public class OrderLine
    {
        public int MenuId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string ChefNotes { get; set; }
        public string Person { get; set; }
        public List<Modifier> Modifiers { get; set; }
        public List<OrderLine> ChildOrderLines { get; set; }
        public string Cat1 { get; set; }
        public string Cat2 { get; set; }
    }
}

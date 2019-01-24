using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AndroCloudServices.Domain
{
    [XmlType("Voucher")]
    public class Voucher
    {
        public string VoucherCode { get; set; }
        public bool IsValid { get; set; }
        public int ErrorCode { get; set; }
        public string EffectType { get; set; }
        public decimal EffectValue { get; set; }
    }
}

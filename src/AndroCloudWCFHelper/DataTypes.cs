using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AndroCloudHelper
{
    public class DataTypes
    {
        public DataTypeEnum SubmittedDataType { get; set; }
        public DataTypeEnum WantsDataType { get; set; }

        public DataTypes()
        {
            this.SubmittedDataType = DataTypeEnum.Unknown;
            this.WantsDataType = DataTypeEnum.Unknown;
        }

        public DataTypes(DataTypeEnum submittedDataType, DataTypeEnum wantsDataType)
        {
            this.SubmittedDataType = submittedDataType;
            this.WantsDataType = wantsDataType;
        }

        public static string DataTypeToText(DataTypeEnum dataTypeEnum)
        {
            if (dataTypeEnum == DataTypeEnum.JSON)
            {
                return "application/json";
            }
            else
            {
                return "application/xml";
            }
        }
    }
}

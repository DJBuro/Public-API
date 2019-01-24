using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using Newtonsoft.Json;

namespace AndroCloudHelper
{
    public class SerializeHelper
    {
        public static string Serialize<T>(object objectToSerialize, DataTypeEnum dataType)
        {
            string output = "";

            if (dataType == DataTypeEnum.XML)
            {
                XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces(); xmlSerializerNamespaces.Add("", "");
                using (StringWriter stringWriter = new StringWriter()) 
                {
                    using (XmlWriter writer = XmlWriter.Create(stringWriter, new XmlWriterSettings { OmitXmlDeclaration = true }))
                    {
                        new XmlSerializer(typeof(T)).Serialize(writer, objectToSerialize, xmlSerializerNamespaces);

                        output = stringWriter.ToString();
                    }
                }
            }
            else if (dataType == DataTypeEnum.JSON)
            {
                output = JsonConvert.SerializeObject(objectToSerialize);
            }

            return output;
        }

        public static string Deserialize<T>(string jsonOrXmlToDeserialize, DataTypeEnum dataType, out T outputObject)
        {
            outputObject = default(T);

            if (dataType == DataTypeEnum.XML)
            {
                try
                {
                    XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces(); xmlSerializerNamespaces.Add("", "");
                    using (StringWriter stringWriter = new StringWriter())
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(T));

                        using (StringReader stringReader = new StringReader(jsonOrXmlToDeserialize))
                        {
                            outputObject = (T)serializer.Deserialize(stringReader);
                        }
                    }
                }
                catch (InvalidOperationException exception)
                {
                    // There was a problem deserializing the object.  Return the error message to the caller
                    if (exception.InnerException != null)
                    {
                        return exception.InnerException.Message;
                    }
                    else
                    {
                        return exception.Message;
                    }
                }
            }
            else if (dataType == DataTypeEnum.JSON)
            {
                try
                {
                    outputObject = JsonConvert.DeserializeObject<T>(jsonOrXmlToDeserialize);
                }
                catch (Newtonsoft.Json.JsonReaderException exception)
                {
                    // There was a problem deserializing the object.  Return the error message to the caller
                    return exception.Message;
                }
            }

            return "";
        }
    }
}

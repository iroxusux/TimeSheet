using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace TimeSheet.Services
{
    public static class XmlSerialization
    {
        public static void WriteToXmlFile<T>(string sFile, T oData)
        {
            TextWriter oWriter = null;
            try
            {
                var oSerializer = new XmlSerializer(typeof(T));
                oWriter = new StreamWriter(sFile);
                oSerializer.Serialize(oWriter, oData);
            }
            catch (Exception)
            {
                Debug.WriteLine("Could Not Write To Xml File.");
            }
            finally
            {
                if(oWriter != null)
                {
                    oWriter.Close();
                }
            }
        }
        public static T ReadFromXmlFile<T>(string sFile) where T : new()
        {
            TextReader oReader = null;
            try
            {
                var oSerializer = new XmlSerializer(typeof(T));
                oReader = new StreamReader(sFile);
                return (T)oSerializer.Deserialize(oReader);
            }
            catch (Exception)
            {
                Debug.WriteLine("Could Not Read From Xml File.");
                return default;
            }
            finally
            {
                if (oReader != null)
                {
                    oReader.Close();
                }
            }
        }
        private static void SerializerUnkownNode(object oSender, XmlNodeEventArgs oArgs)
        {
            Debug.WriteLine(oArgs);
        }
        private static void SerializerUnkownAttribute(object oSender, XmlAttributeEventArgs oArgs)
        {
            Debug.WriteLine(oArgs);
        }
    }
}

﻿using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Nop.Integration.WebJob
{
    static class HelperFunctions
    {
        #region NotesOnUsage
        ////Make XML
        //var settings = new ObjectCustomerSettings();
        //var xmlString = settings.XmlSerializeToString();

        ////Make Object
        //var settings = xmlString.XmlDeserializeFromString<ObjectCustomerSettings>();
        #endregion

        public static string XmlSerializeToString(this object objectInstance)
        {
            var serializer = new XmlSerializer(objectInstance.GetType());
            var sb = new StringBuilder();

            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, objectInstance);
            }

            return sb.ToString();
        }

        public static T XmlDeserializeFromString<T>(this string objectData)
        {
            return (T)XmlDeserializeFromString(objectData, typeof(T));
        }

        public static object XmlDeserializeFromString(this string objectData, Type type)
        {
            var serializer = new XmlSerializer(type);
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }



    }
}

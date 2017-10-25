using CandyFramework.Common.Encryption;
using CandyFramework.Common.Extentions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace CandyFramework.Common.Converter
{
    public class Serialize
    {
        #region - XML -

        #region - Serialize -
        public static void XmlSerialize<T>(T obj, string path) where T : class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            XmlWriter writer = XmlWriter.Create(path);

            serializer.Serialize(writer, obj);

            writer.Close();
            ((IDisposable)writer).Dispose();
        }

        public static string XmlStringSerialize<T>(T obj) where T : class
        {
            XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, obj);
                return textWriter.ToString();
            }
        }
        #endregion

        #region - Desrializer -
        public static T XmlDesrializer<T>(string path) where T : class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            XmlReader reader = XmlReader.Create(path);

            object obj = serializer.Deserialize(reader);

            reader.Close();
            ((IDisposable)reader).Dispose();

            return (T)obj;
        }

        public static T XmlStringDesrializer<T>(string @this) where T : class
        {
            var reader = XmlReader.Create(@this.Trim().ToStream(), new XmlReaderSettings()
            {
                ConformanceLevel = ConformanceLevel.Document
            });
            return new XmlSerializer(typeof(T)).Deserialize(reader) as T;
        }
        #endregion

        #endregion

        #region - JSON -

        #region - Serialize -
        public static string JSONSerialize<T>(T serializeObject) where T : class
        {
            return JSONSerialize(serializeObject, false);
        }
        public static string JSONSerialize<T>(T serializeObject, string encrytPass) where T : class
        {
            JsonSerializerSettings jsSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            string result = JsonConvert.SerializeObject(serializeObject, jsSettings);
            if (!string.IsNullOrEmpty(encrytPass))
            {
                result = Encrypter.Encrypt(result, encrytPass);
            }

            return result;
        }
        #endregion

        #region - Desrializer -
        public static T JSONDeserialize<T>(string deserializeString)
        {
            return JSONDeserialize<T>(deserializeString, false);
        }
        public static T JSONDeserialize<T>(string deserializeString, string encrytPass)
        {
            if (!string.IsNullOrEmpty(encrytPass))
            {
                deserializeString = Encrypter.Decrypt(deserializeString, encrytPass);

            }

            var jsSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            return JsonConvert.DeserializeObject<T>(deserializeString, jsSettings);
        }
        #endregion

        #endregion
    }
}

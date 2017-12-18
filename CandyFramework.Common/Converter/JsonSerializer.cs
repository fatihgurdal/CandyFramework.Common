using CandyFramework.Common.Encryption;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandyFramework.Common.Converter
{
    public class JsonSerializer
    {
        #region - Serialize -
        public static string JSONSerialize<T>(T serializeObject) where T : class
        {
            return JSONSerialize(serializeObject, string.Empty);
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
            return JSONDeserialize<T>(deserializeString, string.Empty);
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
    }
}

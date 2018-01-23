using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCommon
{
    public class MessageSerializerService
    {
        public object SerializeObjectOfType<T>(T objectToSerialize) where T : class
        {
            object returnValue = null;
#if DEBUG
            returnValue = SerializeJson<T>(objectToSerialize);
#else
            returnValue = SerializeBson<T>(objectToSerialize);
#endif
            return returnValue;
        }

        public T DeserializeObjectOfType<T>(object objectToDeserialize) where T : class
        {
            T returnValue = null;
#if DEBUG
            returnValue = DeserializeJson<T>(objectToDeserialize);
#else
            returnValue = DeserializeBson<T>(objectToDeserialize);
#endif
            return returnValue;
        }

        #region Json
        public object SerializeJson<T>(T objectToSerialize) where T : class
        {
            return JsonConvert.SerializeObject(objectToSerialize);
        }

        public T DeserializeJson<T>(object objectToDeserialize) where T : class
        {
            return JsonConvert.DeserializeObject<T>((string)objectToDeserialize);
        }
        #endregion

        #region Bson
        public object SerializeBson<T>(T objectToSerialize) where T : class
        {
            MemoryStream ms = new MemoryStream();
            using (BsonDataWriter writer = new BsonDataWriter(ms))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, objectToSerialize);
            }

            return Convert.ToBase64String(ms.ToArray());
        }

        public T DeserializeBson<T>(object objectToDeserialize) where T : class
        {
            byte[] data = Convert.FromBase64String((string)objectToDeserialize);

            MemoryStream ms = new MemoryStream(data);
            using (BsonDataReader reader = new BsonDataReader(ms))
            {
                JsonSerializer serializer = new JsonSerializer();
                return serializer.Deserialize<T>(reader);
            }
        }
        #endregion
    }
}

using ServiceStack;
using ServiceStack.Text;

namespace EasyDDD.Infrastructure.Crosscutting.Json.ServiceStackText
{
    public class ServiceStackTextJson : IJson
    {
        public ServiceStackTextJson()
        {
            JsConfig.ThrowOnDeserializationError = true;
        }

        public string SerializeObject(object obj)
        {
            return obj.ToJson();
        }

        public T DeserializeObject<T>(string value)
        {
            return value.FromJson<T>();
        }

        public object DeserializeFromStream(System.Type type, System.IO.Stream stream)
        {
            return JsonSerializer.DeserializeFromStream(type, stream);
        }

        public void SerializeToStream(object value, System.Type type, System.IO.Stream writeStream)
        {
            JsonSerializer.SerializeToStream(value, type, writeStream);
        }
    }
}

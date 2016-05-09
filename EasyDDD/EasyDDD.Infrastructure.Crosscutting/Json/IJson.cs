using System;
using System.IO;

namespace EasyDDD.Infrastructure.Crosscutting.Json
{
    public interface IJson
    {
        string SerializeObject(object obj);
        T DeserializeObject<T>(string value);

        object DeserializeFromStream(Type type, Stream stream);

        void SerializeToStream(object value, Type type, Stream stream);
    }
}

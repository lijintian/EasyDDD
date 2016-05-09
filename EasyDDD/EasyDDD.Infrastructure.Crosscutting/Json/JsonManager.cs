using System;
using System.Diagnostics;
using System.IO;
using EasyDDD.Infrastructure.Crosscutting.InversionOfControl;

namespace EasyDDD.Infrastructure.Crosscutting.Json
{
    public static class JsonManager
    {
        #region 私有成员

        private static IJson InternalJson
        {
            [DebuggerStepThrough]
            get
            {
                return IoC.Resolve<IJson>();
            }
        }

        #endregion

        public static string SerializeObject(object obj)
        {
            return InternalJson.SerializeObject(obj);
        }

        public static T DeserializeObject<T>(string value)
        {
            return InternalJson.DeserializeObject<T>(value);
        }

        public static object DeserializeFromStream(Type type, Stream stream)
        {
            return InternalJson.DeserializeFromStream(type, stream);
        }

        public static void SerializeToStream(object value, System.Type type, System.IO.Stream writeStream)
        {
            InternalJson.SerializeToStream(value, type, writeStream);
        }
    }
}

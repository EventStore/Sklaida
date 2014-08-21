using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace BackendServices
{
    public class Json
    {
        public static Formatting Formatting = Formatting.Indented;

        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Include,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            TypeNameHandling = TypeNameHandling.None,
            Converters = new JsonConverter[] { new StringEnumConverter() }
        };

        public T From<T>(string text)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(text, Settings);
            }
            catch (Exception e)
            {
                Log.Exception(String.Format("'{0}' is not a valid serialized {1}", text, typeof(T).FullName), e);
                return default(T);
            }
        }

        public string To<T>(T value)
        {
            if (value == null) return "";
            try
            {
                return JsonConvert.SerializeObject(value, Formatting, Settings);
            }
            catch (Exception ex)
            {
                Log.Exception(string.Format("Error serializing object {0}", value), ex);
                return null;
            }
        }
 
    }
}
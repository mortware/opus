using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Opus.Portal.Converters
{
    public class ObjectFromJsonFieldConverter<TObject> : JsonConverter<TObject>
    {
        private readonly string _fieldName;
        private readonly Dictionary<string, Type> _objectTypes = new Dictionary<string, Type>();

        public ObjectFromJsonFieldConverter(string fieldName)
        {
            _fieldName = fieldName;
        }

        public ObjectFromJsonFieldConverter<TObject> AddType<T>(string fieldValue)
        {
            _objectTypes.TryAdd(fieldValue.ToLower(), typeof(T));
            return this;
        }

        public override void WriteJson(JsonWriter writer, TObject value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override TObject ReadJson(JsonReader reader, Type objectType, TObject existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return default(TObject);

            try
            {
                var json = JObject.Load(reader);
                var fieldValue = GetFieldValue(json);

                if (!_objectTypes.TryGetValue(fieldValue, out var type))
                    throw new Exception($"Unable to deserialize object from property name: '{fieldValue}'");

                var obj = (TObject)json.ToObject(type);
                 
                return obj;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to read Json", e);
            }
        }

        private string GetFieldValue(JObject obj)
        {
            if (!obj.ContainsKey(_fieldName))
                throw new JsonSerializationException($"Json does not contain the required property '{_fieldName}'");

            var fieldValue = obj[_fieldName].Value<string>();
            return fieldValue.ToLower();
        }
    }
}

using Newtonsoft.Json;
using UnityEngine;
using System;
public class Vector3Converter : JsonConverter<Vector3>
{
    public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(value.x);
        writer.WritePropertyName("y");
        writer.WriteValue(value.y);
        writer.WritePropertyName("z");
        writer.WriteValue(value.z);
        writer.WriteEndObject();
    }

    public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        float x = 0, y = 0, z = 0;

        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.PropertyName)
            {
                string propName = reader.Value.ToString();
                reader.Read();

                switch (propName)
                {
                    case "x": x = (float)(double)reader.Value; break;
                    case "y": y = (float)(double)reader.Value; break;
                    case "z": z = (float)(double)reader.Value; break;
                }
            }
            else if (reader.TokenType == JsonToken.EndObject)
            {
                break;
            }
        }

        return new Vector3(x, y, z);
    }

    public override bool CanRead => true;
}

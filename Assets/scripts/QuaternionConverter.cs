using Newtonsoft.Json;
using UnityEngine;
using System;
public class QuaternionConverter : JsonConverter<Quaternion>
{
    public override void WriteJson(JsonWriter writer, Quaternion value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(value.x);
        writer.WritePropertyName("y");
        writer.WriteValue(value.y);
        writer.WritePropertyName("z");
        writer.WriteValue(value.z);
        writer.WritePropertyName("w");
        writer.WriteValue(value.w);
        writer.WriteEndObject();
    }

    public override Quaternion ReadJson(JsonReader reader, Type objectType, Quaternion existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        float x = 0, y = 0, z = 0, w = 0;

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
                    case "w": w = (float)(double)reader.Value; break;
                }
            }
            else if (reader.TokenType == JsonToken.EndObject)
            {
                break;
            }
        }

        return new Quaternion(x, y, z, w);
    }

    public override bool CanRead => true;
}
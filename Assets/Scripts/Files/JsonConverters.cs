using UnityEngine;
using Newtonsoft.Json;
using System;

public class Vector3Converter : JsonConverter<Vector3>
{
    public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        Vector3 vector3 = Vector3.zero;
        Newtonsoft.Json.Linq.JObject jObject = Newtonsoft.Json.Linq.JObject.Load(reader);
        vector3.x = (float)jObject["X"];
        vector3.y = (float)jObject["Y"];
        vector3.z = (float)jObject["Z"];
        return vector3;
    }

    public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("X");
        writer.WriteValue(value.x);
        writer.WritePropertyName("Y");
        writer.WriteValue(value.y);
        writer.WritePropertyName("Z");
        writer.WriteValue(value.z);

        writer.WriteEndObject();
    }
}


public class QuaternionConverter : JsonConverter<Quaternion>
{
    public override Quaternion ReadJson(JsonReader reader, Type objectType, Quaternion existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        Quaternion quaternion = Quaternion.identity;
        Newtonsoft.Json.Linq.JObject jObject = Newtonsoft.Json.Linq.JObject.Load(reader);
        quaternion.x = (float)jObject["X"];
        quaternion.y = (float)jObject["Y"];
        quaternion.z = (float)jObject["Z"];
        quaternion.w = (float)jObject["W"];
        return quaternion;
    }

    public override void WriteJson(JsonWriter writer, Quaternion value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("X");
        writer.WriteValue(value.x);
        writer.WritePropertyName("Y");
        writer.WriteValue(value.y);
        writer.WritePropertyName("Z");
        writer.WriteValue(value.z);
        writer.WritePropertyName("W");
        writer.WriteValue(value.w);
        writer.WriteEndObject();
    }
}

public class ColorConverter : JsonConverter<Color>
{
    public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        Color color = new Color();
        Newtonsoft.Json.Linq.JObject jObject = Newtonsoft.Json.Linq.JObject.Load(reader);
        color.r = (float)jObject["R"];
        color.g = (float)jObject["G"];
        color.b = (float)jObject["B"];
        color.a = (float)jObject["A"];
        return color;
    }

    public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("R");
        writer.WriteValue(value.r);
        writer.WritePropertyName("G");
        writer.WriteValue(value.g);
        writer.WritePropertyName("B");
        writer.WriteValue(value.b);
        writer.WritePropertyName("A");
        writer.WriteValue(value.a);
        writer.WriteEndObject();
    }
}
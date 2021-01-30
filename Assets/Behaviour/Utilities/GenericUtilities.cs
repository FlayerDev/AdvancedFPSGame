using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class GenericUtilities
{
    public static void ChangeLayer(GameObject gameObject, int layer, bool changeChildren = true)
    {
        gameObject.layer = layer;
        if (!changeChildren) return;
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.layer = layer;
        }
    }
    public static T DeepClone<T>(this T a)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, a);
            stream.Position = 0;
            return (T)formatter.Deserialize(stream);
        }
    }
    public static float ToPercent01(float min, float max, float value)
    {
        float range = max - min;
        float output = (value - min) / range;
        return output;
    }
}

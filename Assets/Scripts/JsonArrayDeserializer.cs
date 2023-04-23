using System;
using UnityEngine;

public class JsonArrayDeserializer
{
    [Serializable]
    private class Wrapper<T>
    {
        public T[] Array;
    }

    public static T[] FromJson<T>(string jsonContent)
    {
        string newJson = "{ \"Array\": " + jsonContent + "}";
        Wrapper<T> wrapper;
        try
        {
            wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            return null;
        }

        return wrapper.Array;
    }
}
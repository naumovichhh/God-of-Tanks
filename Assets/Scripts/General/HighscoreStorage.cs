using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HighscoreStorage
{
    private static readonly string path = Application.persistentDataPath + "/highscores.json";

    public static void Append(Tuple<string, TimeSpan> highscore)
    {
        Wrapper wrapper = LoadWrapper();
        wrapper.data.Add(new SerializableTuple(highscore));
        string json = JsonUtility.ToJson(wrapper);
        File.WriteAllText(path, json);
    }

    private static Wrapper LoadWrapper()
    {
        Wrapper wrapper = null;

        if (!File.Exists(path))
        {
            File.Create(path).Dispose();
            Initialize();
        }

        string content = File.ReadAllText(path);
        wrapper = JsonUtility.FromJson<Wrapper>(content);

        if (wrapper == null)
        {
            wrapper = new Wrapper();
        }
        if (wrapper.data == null)
        {
            wrapper.data = new List<SerializableTuple>();
        }

        return wrapper;
    }

    private static void Initialize()
    {
        Wrapper wrapper = new Wrapper();
        List<SerializableTuple> list = new List<SerializableTuple>()
        {
            new SerializableTuple("Mike", 43),
            new SerializableTuple("Harry", 173),
            new SerializableTuple("BigJohn2", 113),
            new SerializableTuple("Mr_Alex_UA", 232),
            new SerializableTuple("Ultrazdravy", 210),
            new SerializableTuple("S-Y", 39),
            new SerializableTuple("Flashmob.", 48)
        };

        foreach (var item in list)
        {
            wrapper.data.Add(item);
        }

        string json = JsonUtility.ToJson(wrapper);
        File.WriteAllText(path, json);
    }

    public static List<Tuple<string, TimeSpan>> Load()
    {
        Wrapper wrapper = LoadWrapper();
        List<Tuple<string, TimeSpan>> result = new List<Tuple<string, TimeSpan>>();
        if (wrapper != null && wrapper.data != null)
        {
            foreach (var tuple in wrapper.data)
            {
                result.Add(tuple);
            }
        }

        return result;
    }

    public static void Clear()
    {
        if (File.Exists(path))
        {
            File.WriteAllText(path, String.Empty);
        }
    }
}

[Serializable]
public class Wrapper
{
    public List<SerializableTuple> data = new List<SerializableTuple>();
}

[Serializable]
public class SerializableTuple
{
    public SerializableTuple(Tuple<string, TimeSpan> tuple)
    {
        Item1 = tuple.Item1;
        Item2 = (uint)
        tuple.Item2.TotalSeconds;
    }

    public SerializableTuple(string Item1, uint Item2)
    {
        this.Item1 = Item1;
        this.Item2 = Item2;
    }

    public string Item1;
    public uint Item2;

    public static implicit operator Tuple<string, TimeSpan>(SerializableTuple serializableTuple)
    {
        Tuple<string, TimeSpan> result = new Tuple<string, TimeSpan>(
            serializableTuple.Item1,
            TimeSpan.FromSeconds(serializableTuple.Item2)
        );

        return result;
    }
}
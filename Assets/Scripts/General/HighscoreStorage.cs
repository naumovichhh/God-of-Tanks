using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class HighscoreStorage
{
    private static readonly string path = Application.persistentDataPath + "/records.dat";

    public static void Append(Tuple<string, TimeSpan> record)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        List<Tuple<string, TimeSpan>> data;
        data = Load();

        data.Add(record);
        using (var stream = File.Open(path, FileMode.Create))
        {
            formatter.Serialize(stream, data);
        }
    }

    public static List<Tuple<string, TimeSpan>> Load()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        List<Tuple<string, TimeSpan>> data = null;

        if (File.Exists(path))
        {
            using (var stream = File.Open(path, FileMode.Open))
            {
                data = formatter.Deserialize(stream) as List<Tuple<string, TimeSpan>>;
            }
        }
        else
        {
            File.Create(path).Dispose();
        }

        return data ?? new List<Tuple<string, TimeSpan>>();
    }

    public static void Clear()
    {
        if (File.Exists(path))
        {
            File.WriteAllText(path, String.Empty);
        }
    }
}
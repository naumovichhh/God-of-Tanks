using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class GridLayoutPopulate : MonoBehaviour
{
    public GameObject prefab;

    private void Awake()
    {
        Populate();
    }

    private void Populate()
    {
        var list = HighscoreStorage.Load();
        foreach (var item in list
            .OrderByDescending(i => i.Item2)
            .Select((e, i) => $"{i+1}. {e.Item1} {e.Item2.ToString(@"m\:ss")}"))
        {
            var instance = Instantiate(prefab, transform);
            instance.GetComponent<TextMeshProUGUI>().SetText(item);
        }
    }
}

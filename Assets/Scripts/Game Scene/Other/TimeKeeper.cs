using System.Collections;
using System.Diagnostics;
using UnityEngine;
using TMPro;

public class TimeKeeper : MonoBehaviour
{
    private TextMeshProUGUI text;
    private Stopwatch stopwatch;


    private void Awake()
    {
        stopwatch = new Stopwatch();
        stopwatch.Start();
        text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(ShowTime());
    }

    private IEnumerator ShowTime()
    {
        while (true)
        {
            var timeSpan = stopwatch.Elapsed;
            text.SetText(timeSpan.ToString(@"m\:ss"));
            yield return new WaitForSeconds(0.16f);
        }
    }
}

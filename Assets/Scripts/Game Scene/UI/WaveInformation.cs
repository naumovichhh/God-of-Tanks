using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

public class WaveInformation : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        NotifyWaveNumber(1);
    }

    public void NotifyWaveNumber(int number)
    {
        if (number < 1 || number > 10)
            throw new ArgumentOutOfRangeException();

        if (number == 1)
        {
            Show("1st wave is on the way");
        }
        else
        {
            string passedWave, upcomingWave;
            passedWave = $"{number-1}{GetSuffix(number-1)} wave is passed";
            upcomingWave = $"{number}{GetSuffix(number)} wave is on the way";
            Show($"{passedWave}\n{upcomingWave}");
        }
    }

    private void Show(string message)
    {
        text.SetText(message);
        text.canvasRenderer.SetAlpha(1);
        StartCoroutine(FadeText());
    }

    private IEnumerator FadeText()
    {
        yield return new WaitForSeconds(2);
        text.CrossFadeAlpha(0, 3, false);
    }

    private string GetSuffix(int number)
    {
        switch (number)
        {
            case 1:
                return "st";
            case 2:
                return "nd";
            case 3:
                return "rd";
            default:
                return "th";
        }
    }
}

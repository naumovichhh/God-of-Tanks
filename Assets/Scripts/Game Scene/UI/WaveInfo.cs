﻿using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class WaveInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI notification;
    [SerializeField] private TextMeshProUGUI display;
    [SerializeField] private int wavesToWin;

    private void Awake()
    {
        WaveStarted(1);
    }

    private void Start()
    {
        GameManager.Instance.Won = false;
    }

    public void WaveStarted(int number)
    {
        if (number < 1)
            throw new ArgumentOutOfRangeException(nameof(number));
        
        if (number == wavesToWin + 1)
        {
            GameManager.Instance.Won = true;
            Notify(number, true);
        }
        else
        {
            Notify(number);
        }

        ChangeDisplay(number);
    }

    private void ChangeDisplay(int number)
    {
        display.SetText($"{number}{GetSuffix(number)} wave");
    }

    private void Notify(int number, bool won = false)
    {
        if (number == 1)
        {
            Show("1st wave is on the way");
        }
        else
        {
            string passedWave, upcomingWave;
            passedWave = $"{number-1}{GetSuffix(number-1)} wave is passed";
            upcomingWave = $"{number}{GetSuffix(number)} wave is on the way";
            string eventualText = $"{passedWave}\n{upcomingWave}";
            if (won)
            {
                eventualText = "You have won!\n" + eventualText;
            }

            Show(eventualText);
        }
    }

    private void Show(string message)
    {
        notification.SetText(message);
        notification.canvasRenderer.SetAlpha(1);
        StartCoroutine(FadeText());
    }

    private IEnumerator FadeText()
    {
        yield return new WaitForSeconds(3);
        notification.CrossFadeAlpha(0, 3, false);
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

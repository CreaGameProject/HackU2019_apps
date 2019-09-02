using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SpanSetting : MonoBehaviour
{
    [SerializeField] private AlarmManager manager;
    [SerializeField] private Text sleep;
    [SerializeField] private Text rem;
    [SerializeField] private InputField input;
    public String raspberryAddress;

    private void Awake()
    {
        // sleep.text = manager.sleepTime.TotalMinutes.ToString();
        // rem.text = manager.REMtime.TotalMinutes.ToString();
        input.text = PlayerPrefs.GetString("raspberryAddress");
        manager.raspberryAddress = input.text;
    }

    public void Sleep()
    {
        PlayerPrefs.SetString("raspberryAddress", input.text);
        PlayerPrefs.Save();

        manager.raspberryAddress = input.text;
        Debug.Log(manager.raspberryAddress);
    }

    public void REM()
    {
        manager.REMtime = TimeSpan.FromMinutes(double.Parse(rem.text));
    }
}

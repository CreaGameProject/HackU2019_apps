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

    private void Start()
    {
        sleep.text = manager.sleepTime.TotalMinutes.ToString();
        rem.text = manager.REMtime.TotalMinutes.ToString();
    }
    public void Sleep()
    {
        manager.sleepTime=TimeSpan.FromMinutes(double.Parse(sleep.text));
    }

    public void REM()
    {
        manager.REMtime = TimeSpan.FromMinutes(double.Parse(rem.text));
    }
}

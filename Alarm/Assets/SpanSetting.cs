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
    public String raspberryAddress;

    private void Start()
    {
        sleep.text = manager.sleepTime.TotalMinutes.ToString();
        rem.text = manager.REMtime.TotalMinutes.ToString();
    }
    public void Sleep()
    {
        GameObject managerObj = GameObject.FindGameObjectWithTag("Manager");
        AlarmManager manager = managerObj.GetComponent<AlarmManager>();
        manager.raspberryAddress = sleep.text;
        Debug.Log(manager.raspberryAddress);
    }

    public void REM()
    {
        manager.REMtime = TimeSpan.FromMinutes(double.Parse(rem.text));
    }
}

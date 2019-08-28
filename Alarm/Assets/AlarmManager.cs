using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class AlarmManager : MonoBehaviour
{
    [Serializable]
    private class SettingClock
    {
        public GameObject clock;
        public Text text;
        public GameObject hand;
    }
    [SerializeField] private SettingClock ampm;
    [SerializeField] private SettingClock hour;
    [SerializeField] private SettingClock minute;
    [SerializeField] private GameObject setSwitch;
    [SerializeField] private GameObject reminder;
    [SerializeField] private Button[] importance;
    [SerializeField] private AudioClip[] clips;

    private SleepMesurement mesurement;
    private DeviceControler controler;
    private GameObject displayClock;
    public DateTime Alarm;
    public TimeSpan sleepTime;
    public TimeSpan REMtime;
    private int h;
    private int m;
    private int imp;
    private bool onoff;
    private bool moment;

    private void Start()
    {
        mesurement = GetComponent<SleepMesurement>();
        controler = GetComponent<DeviceControler>();
        displayClock = reminder;
        StopAlerm();
        reminder.GetComponentInChildren<Text>().text = "SET" + Environment.NewLine + "ALARM";

        sleepTime = TimeSpan.FromMinutes(360);
        REMtime = TimeSpan.FromMinutes(60);
        imp = 1;
        importance[imp].image.color = Color.red + Color.gray;
    }

    private void FixedUpdate()
    {
        if (onoff)
        {
            if (Alarm - REMtime < DateTime.Now && (!moment && mesurement.REM))
            {
                GetUpAlarm();
                moment = true;
            }
        }
        else
        {
            if (Alarm - sleepTime < DateTime.Now)
            {
                reminder.GetComponentInChildren<Text>().text = "SLEEP!!";
                ReplaceClock(reminder);
            }
        }
    }
    public void DisplayClock(int number)
    {
        onoff = false;
        Alarm = DateTime.MaxValue;
        setSwitch.GetComponent<Button>().image.color = Color.gray + Color.green;
        setSwitch.GetComponentInChildren<Text>().text = "OFF";
        setSwitch.GetComponentsInChildren<RectTransform>()[1].anchoredPosition = Vector2.left * 80f;
        switch (number)
        {
            case 0:
                ReplaceClock(ampm.clock);
                break;
            case 1:
                ReplaceClock(hour.clock);
                break;
            case 2:
                ReplaceClock(minute.clock);
                break;
            default:break;
        }
    }

    public void SetAMPM(int input)
    {
        h = h % 12 + 12 * input;
        switch (input)
        {
            case 0:
                ampm.text.text = "AM";
                ampm.hand.GetComponent<Image>().color = Color.grey / 2 + Color.black / 2;
                ampm.hand.GetComponent<RectTransform>().anchoredPosition = Vector2.up * 30;
                break;
            case 1:
                ampm.text.text = "PM";
                ampm.hand.GetComponent<Image>().color = Color.white;
                ampm.hand.GetComponent<RectTransform>().anchoredPosition = Vector2.down * 30;
                break;
            default:break;
        }
    }

    public void SetHour(int input)
    {
        h = h < 12 ? input : input + 12;
        hour.text.text = input.ToString("00");
        hour.hand.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, -30f * input);
    }

    public void SetMin(int input)
    {
        m = input;
        minute.text.text = input.ToString("00");
        minute.hand.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, -6f * input);
    }

    public void SetImportance(int input)
    {
        importance[imp].image.color = Color.grey / 2 + Color.black / 2;
        imp = input;
        importance[imp].image.color = Color.grey + Color.red;
    }

    public void AlarmSet()
    {
        Alarm = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
            h, m, 0);
        if (Alarm < DateTime.Now) {
            Alarm=Alarm.AddDays(1f);
        }
        if (!onoff && Alarm - sleepTime < DateTime.Now)
        {
            reminder.GetComponentInChildren<Text>().text = "SLEEP!!";
            ReplaceClock(reminder);
        }
    }

    public void Switch()
    {
        onoff = !onoff;
        if (onoff)
        {
            setSwitch.GetComponent<Button>().image.color = Color.gray + Color.red;
            setSwitch.GetComponentInChildren<Text>().text = "ON";
            setSwitch.GetComponentsInChildren<RectTransform>()[1].anchoredPosition = Vector2.right * 80f;
            SleepAlarm();
        }
        else
        {
            setSwitch.GetComponent<Button>().image.color = Color.gray + Color.green;
            setSwitch.GetComponentInChildren<Text>().text = "OFF";
            setSwitch.GetComponentsInChildren<RectTransform>()[1].anchoredPosition = Vector2.left * 80f;
            StopAlerm();
        }
    }

    private void ReplaceClock(GameObject display)
    {
        displayClock.SetActive(false);
        display.SetActive(true);
        displayClock = display;
    }

    //睡眠時のアラーム
    private void SleepAlarm()
    {
        reminder.GetComponentInChildren<Text>().text = "GOOD" + Environment.NewLine + "NIGHT";
        ReplaceClock(reminder);
        GetComponent<AudioSource>().clip = clips[3];
        GetComponent<AudioSource>().Play();
    }

    //起床時のアラーム
    private void GetUpAlarm()
    {
        reminder.GetComponentInChildren<Text>().text = "GET UP!!";
        ReplaceClock(reminder);
        GetComponent<AudioSource>().clip = clips[imp];
        GetComponent<AudioSource>().Play();
        controler.AlarmOn();
    }

    //起床時のアラーム停止
    private void StopAlerm()
    {
        reminder.GetComponentInChildren<Text>().text = "GOOD" + Environment.NewLine + "MONING";
        ReplaceClock(reminder);
        GetComponent<AudioSource>().Stop();
        Alarm = DateTime.MaxValue;
        controler.AlarmOff();
        moment = false;
    }
}

[Serializable]
public class Data
{
    public string answer;
    public bool forced;
    public string image;
}



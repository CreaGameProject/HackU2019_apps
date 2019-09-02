using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;
using Unity.Notifications.Android;

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

    private SleepMesurement mesurement;
    private DeviceControler controler;
    private GameObject displayClock;

    public DateTime Alarm;
    public TimeSpan sleepTime;
    public TimeSpan REMtime;
    public int imp;
    
    private int h;
    private int m;
    private bool onoff;
    private bool isRinging;
    private bool isReminding;

    public String raspberryAddress;

    private void Awake()
    {
        var c = new AndroidNotificationChannel
        {
            Id = "hacku2019app",
            Name = "HackU2019App",
            Importance = Importance.High,
            Description = "HackU2019の通知",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(c);

        raspberryAddress = PlayerPrefs.GetString("raspberryAddress");
    }

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
        SetImportance(imp);

        StartCoroutine(AlarmInitialze());
    }

    private void FixedUpdate()
    {
        if (onoff)
        {
            if(!isRinging && Alarm < DateTime.Now)
            {
                GetUpAlarm();
                isRinging = true;
            }
            else if (Alarm - REMtime < DateTime.Now && (!isRinging && mesurement.REM()))
            {
                GetUpAlarm();
                isRinging = true;
            }
        }
        else
        {
            if (!isReminding && Alarm - sleepTime < DateTime.Now)
            {
                reminder.GetComponentInChildren<Text>().text = "SLEEP!!";
                controler.SleepRemind();
                var n = new AndroidNotification
                {
                    Title = "睡眠リマインド",
                    Text = "そろそろ寝ましょう",
                };
                AndroidNotificationCenter.SendNotification(n, "hacku2019app");
                isReminding = true;
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
        importance[imp].image.color = Color.grey + Color.blue;
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
            var n = new AndroidNotification
            {
                Title = "睡眠リマインド",
                Text = "そろそろ寝ましょう",
            };
            AndroidNotificationCenter.SendNotification(n, "hacku2019app");
            ReplaceClock(reminder);
            isReminding = true;
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
            controler.SleepRemind();
            SleepAlarm();
        }
        else
        {
            setSwitch.GetComponent<Button>().image.color = Color.gray + Color.green;
            setSwitch.GetComponentInChildren<Text>().text = "OFF";
            setSwitch.GetComponentsInChildren<RectTransform>()[1].anchoredPosition = Vector2.left * 80f;
            controler.SleepRemindFinish();
            StopAlerm();
        }
    }

    private void ReplaceClock(GameObject display)
    {
        isReminding = false;
        displayClock.SetActive(false);
        display.SetActive(true);
        displayClock = display;
    }

    //睡眠時のアラーム
    private void SleepAlarm()
    {
        reminder.GetComponentInChildren<Text>().text = "GOOD" + Environment.NewLine + "NIGHT";
        ReplaceClock(reminder);
    }

    //起床時のアラーム
    private void GetUpAlarm()
    {
        reminder.GetComponentInChildren<Text>().text = "GET UP!!";
        ReplaceClock(reminder);
        StartCoroutine(LEDStart());
        controler.AlarmOn();
    }

    //起床時のアラーム停止
    private void StopAlerm()
    {
        reminder.GetComponentInChildren<Text>().text = "GOOD" + Environment.NewLine + "MONING";
        ReplaceClock(reminder);
        Alarm = DateTime.MaxValue;
        isRinging = false;
        StartCoroutine(LEDStop());
        controler.AlarmOff();
    }

    IEnumerator LEDStart()
    {
        Debug.Log(raspberryAddress);
        UnityWebRequest request = UnityWebRequest.Get(raspberryAddress);
        yield return request.SendWebRequest();
        if (request.isHttpError || request.isNetworkError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Response response = JsonUtility.FromJson<Response>(request.downloadHandler.text);
            Debug.Log(response.message);
        }
    }

    IEnumerator LEDStop()
    {
        UnityWebRequest request = UnityWebRequest.Get(raspberryAddress + "/stop");
        yield return request.SendWebRequest();
        if (request.isHttpError || request.isNetworkError)
        {
            Debug.Log(request.error);
        }
    }

    IEnumerator AlarmInitialze()
    {
        UnityWebRequest request = UnityWebRequest.Get("https://cgp-hacku2019.tech/api/tasks");

        yield return request.SendWebRequest();
        
        if (request.isHttpError || request.isNetworkError)
        {
            Debug.Log(request.error);
        }
        else
        {
            String jsonString = "{\"items\":" + request.downloadHandler.text + "}";
            AlarmTaskResponse response = JsonUtility.FromJson<AlarmTaskResponse>(jsonString);

            if (response.items.Count > 0)
            {
                AlarmTask task = response.items[response.items.Count - 1];

                SetHour(task.soundsAt.Hour % 12);
                SetMin(task.soundsAt.Minute);
                SetAMPM(task.soundsAt.Hour > 12 ? 1 : 0);

                AlarmSet();
            }
        }
    }
}

[System.Serializable]
public class Response
{
    public String message;
    public Boolean success;
}

[System.Serializable]
public class AlarmTaskResponse
{
    public List<AlarmTask> items;
}

[System.Serializable]
public class AlarmTask
{
    public string sounds_at;

    public DateTime soundsAt
    {
         get
        {
            return DateTime.Parse(
                sounds_at, null, System.Globalization.DateTimeStyles.RoundtripKind
            );
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// 出力デバイスへの出力クラス
/// </summary>
public class DeviceControler : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioSource source;
    [SerializeField] private AlarmManager manager;

    string url = "https://yesno.wtf/api";
    //以下の3つをこのクラスにおけるメインメソッドとします。

    public void Sleep()
    {
        source.clip = clips[3];
        source.Play();
    }
    public void AlarmOn()
    {
        source.clip = clips[manager.imp];
        source.Play();
        //StartCoroutine(HttpPost());
        //LEDon()
    }

    public void AlarmOff()
    {
        source.Stop();
        //LEDoff()
    }


    //実験
    private IEnumerator HttpPost()
    {
        Data data = new Data();
        data.answer = "yes";
        data.forced = false;
        data.image = "image";
        var json = JsonUtility.ToJson(data);
        var jsonBinary = System.Text.Encoding.UTF8.GetBytes(json);

        DownloadHandlerBuffer downloadHandlerBuffer = new DownloadHandlerBuffer();

        UploadHandlerRaw uploadHandlerRaw = new UploadHandlerRaw(jsonBinary);
        uploadHandlerRaw.contentType = "application/json";

        UnityWebRequest www = new UnityWebRequest(url, "POST", downloadHandlerBuffer, uploadHandlerRaw);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }
}



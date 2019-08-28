using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

/// <summary>
/// 出力デバイスへの出力クラス
/// </summary>
public class DeviceControler : MonoBehaviour
{
    string url = "https://yesno.wtf/api";
    //以下の二つをこのクラスにおけるメインメソッドとします。
    public void AlarmOn()
    {
        StartCoroutine(HttpPost());
    }

    public void AlarmOff()
    {

    }

    private IEnumerator HttpPost()
    {
        Data data=new Data();
        data.answer = "yes";
        data.forced = false;
        data.image = "image";
        var json = JsonUtility.ToJson(data);
        var jsonBinary = System.Text.Encoding.UTF8.GetBytes(json);

        DownloadHandlerBuffer downloadHandlerBuffer = new DownloadHandlerBuffer();

        UploadHandlerRaw uploadHandlerRaw = new UploadHandlerRaw(jsonBinary);
        uploadHandlerRaw.contentType = "application/json";

        UnityWebRequest www =new UnityWebRequest(url, "POST", downloadHandlerBuffer, uploadHandlerRaw);
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

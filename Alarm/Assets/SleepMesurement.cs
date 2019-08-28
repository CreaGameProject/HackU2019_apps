using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

/// <summary>
/// 入力デバイスからの入力クラス
/// </summary>
public class SleepMesurement : MonoBehaviour
{
    string url = "https://yesno.wtf/api";
    //レム睡眠かどうか判別し、このデータをManagerで取得する。
    public bool REM;

    private void FixedUpdate()
    {
        StartCoroutine(HttpGet());
    }

    IEnumerator HttpGet()
    {
        Data data = new Data();
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        JsonUtility.FromJsonOverwrite(www.downloadHandler.text,data);

        if(www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(data.answer);
            Debug.Log(data.forced);
            Debug.Log(data.image);
        }
    }

}

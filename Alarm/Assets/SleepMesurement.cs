using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// 入力デバイスからの入力クラス
/// </summary>
public class SleepMesurement : MonoBehaviour
{
    string url = "https://yesno.wtf/api";
    //レム睡眠かどうか判別し、このデータをManagerで取得する。

    private float[] heartPulseSpan;
    private int index;
    private int count;
    private Vector3[] move;

    private void Awake()
    {
        heartPulseSpan = new float[100];
        index = 0;
        count = 0;
        move = new Vector3[2];
    }

    public bool REM()
    {
        return count > 10 && move[0].magnitude > 1f;
    }

    private void HeartAwake(float pluseSpan)
    {
        //適当
        //鼓動間が短くなる→脈が速くなる
        //速くなればcount+1,遅くなればcount-1
        //countが規定値(仮で10)に達したらREM判定
        count = heartPulseSpan[index] > pluseSpan ? count++ :
            count > 0 ? count-- : 0;
        heartPulseSpan[index] = pluseSpan;
        index = (index + 1) % heartPulseSpan.Length;
    }

    private void MoveAwake(Vector3 vector)
    {
        //適当
        //[0] -> 加速度の差(微分値)
        //[1] -> 一つ前の加速度
        move[0] = move[1] - vector;
        move[1] = vector;
    }

    private void FixedUpdate()
    {
        //StartCoroutine(HttpGet());
    }

    //実験
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

[System.Serializable]
public class Data
{
    public string answer;
    public bool forced;
    public string image;
}
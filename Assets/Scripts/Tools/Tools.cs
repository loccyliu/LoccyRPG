/*******************************************************************
* Summary: 
* Author : 
* Date   : 
*******************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class Tools
{
    static public T FindInParents<T>(GameObject go) where T : Component
    {
        if (go == null) return null;
        var comp = go.GetComponent<T>();

        if (comp != null)
            return comp;

        Transform t = go.transform.parent;
        while (t != null && comp == null)
        {
            comp = t.gameObject.GetComponent<T>();
            t = t.parent;
        }
        return comp;
    }

    static public T FindInParents<T>(Transform go) where T : Component
    {
        if (go == null) return null;
        var comp = go.GetComponent<T>();

        if (comp != null)
            return comp;

        Transform t = go.parent;
        while (t != null && comp == null)
        {
            comp = t.gameObject.GetComponent<T>();
            t = t.parent;
        }
        return comp;
    }

    public static List<T> AddList<T>(this List<T> list, List<T> list2)
    {
        int c = list2.Count;
        for (int i = 0; i < c; ++i)
        {
            list.Add(list2[i]);
        }
        return list;
    }

    /// <summary>
    /// 科学计数法
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string ToLabelData(this object obj)
    {
        string result = "0";
        long num;
        if (!long.TryParse(obj.ToString(), out num))
        {
            Debug.LogWarning("The Num maybe error.");
            num = 0;
            return obj.ToString();
        }

        if (num > 1000000000)//G
        {
            result = (num / 1000000000.0F).ToString("0.00") + "B";
        }
        else if (num > 1000000)//M
        {
            result = (num / 1000000.0F).ToString("0.00") + "M";
        }
        else if (num > 1000)//K
        {
            result = (num / 1000.0F).ToString("0.00") + "K";
        }
        else
        {
            result = num + "";
        }
        return result;
    }

    /// <summary>
    /// 剩余时间
    /// </summary>
    /// <param name="obj"></param>
    /// <returns>00d00h00m00s</returns>
    public static string ToLeftTime(this object obj)
    {
        string result = "";
        int time = 0;
        if (!int.TryParse(obj.ToString(), out time))
        {
            Debug.LogWarning("The Num maybe error.");
            time = 0;
            return obj.ToString();
        }

        System.TimeSpan ts = new System.TimeSpan(0, 0, time);//Debug.Log("00-" + time);


        if (ts.Days > 0)
        {
            result = result + ts.Days + "d";
        }

        if (ts.Hours > 0)
        {
            result = result + ts.Hours + "h";
        }
        if (ts.Minutes > 0)
        {
            result = result + ts.Minutes + "m";
        }

        if (ts.Seconds > 0)
        {
            result = result + ts.Seconds + "s";
        }
        if (result == "")
        {
            result = "0";
        }
        return result;
    }

    /// <summary>
    /// 剩余时间2
    /// </summary>
    /// <param name="obj"></param>
    /// <returns>00:00:00</returns>
    public static string ToLeftTime2(this object obj)
    {
        string result = "";
        int time = 0;
        if (!int.TryParse(obj.ToString(), out time))
        {
            Debug.LogWarning("The Num maybe error.");
            time = 0;
            return obj.ToString();
        }
        int hour = 0;
        int min = 0;
        int sec = 0;
        if (time >= 3600)//超过一小时
        {
            hour = time / 3600;
            time = time % 3600;
        }

        if (time >= 60)
        {
            min = time / 60;
            time = time % 60;
        }
        if (time > 0)
        {
            sec = time;
        }

        result = hour.ToString("00") + ":" + min.ToString("00") + ":" + sec.ToString("00");
        return result;
    }

    /// <summary>
    /// 转换成当地时间
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static System.DateTime ToLocalTime(this object obj)
    {
        long time = (long)obj;
        //Debug.LogError(time);
        System.DateTime dt = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dt = dt.AddMilliseconds((double)time).ToLocalTime();
        return dt;
    }

    /// <summary>
    /// 返回字典的键列表LIST
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <param name="dic"></param>
    /// <returns></returns>
    public static List<T> DicKeysToList<T, V>(this Dictionary<T, V> dic)
    {
        List<T> tempList = new List<T>();

        foreach (T t in dic.Keys)
        {
            tempList.Add(t);
        }
        return tempList;
    }

    /// <summary>
    /// 返回字典的值列表LIST
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <param name="dic"></param>
    /// <returns></returns>
    public static List<V> DicValuesToList<T, V>(this Dictionary<T, V> dic)
    {
        List<V> tempList = new List<V>();

        foreach (V v in dic.Values)
        {
            tempList.Add(v);
        }
        return tempList;
    }

    /// <summary>
    /// 序列化深度拷贝
    /// </summary>
    public static T DepthClone<T>(T obj) where T : class
    {
        MemoryStream stream = new MemoryStream();
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, obj);
        stream.Position = 0;
        return formatter.Deserialize(stream) as T;
    }

    public static int[] GetRandInt(int len)
    {
        int[] sequence = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        int[] output = new int[len];

        int end = sequence.Length - 1;

        for (int i = 0; i < len; i++)
        {
            int num = Random.Range(0, end + 1);
            output[i] = sequence[num];
            sequence[num] = sequence[end];
            end--;
        }

        return output;
    }
}

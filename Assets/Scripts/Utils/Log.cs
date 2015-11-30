/*
 * Log.cs
 * RpgFramework
 * Created by com.loccy on 10/28/2015 14:23:45.
 */

using UnityEngine;
using System.Collections;

public class Log
{
	public static bool showLog = true;

#if UNITY_EDITOR
	public static void i(object msg)
	{
		if (showLog)
			Debug.Log (msg);
		if (Const.logType != LogType.None)
		{
			string str = string.Format ("{0}:[LOG]{1}", System.DateTime.Now.ToShortTimeString (), msg);
			EventSystem.Instance.FireEvent (EventCode.AddLog, str);
		}
	}

	public static void e(object err)
	{
		if (showLog)
			Debug.LogError (err);
		if (Const.logType != LogType.None)
		{
			string str = string.Format ("{0}:[ERROR]{1}", System.DateTime.Now.ToShortTimeString (), err);
			EventSystem.Instance.FireEvent (EventCode.AddLog, str);
		}
	}

	public static void w(object  warn)
	{
		if (showLog)
			Debug.LogWarning (warn);
		if (Const.logType != LogType.None)
		{
			string str = string.Format ("{0}:[WARN]{1}", System.DateTime.Now.ToShortTimeString (), warn);
			EventSystem.Instance.FireEvent (EventCode.AddLog, str);
		}
	}

#else
	public static void i(object msg)
	{
		if (Const.logType != LogType.None)
		{
			string str = string.Format ("{0}:[LOG]{1}", System.DateTime.Now.ToShortTimeString (), msg);
			EventSystem.Instance.FireEvent (EventCode.AddLog, str);
		}
	}

	public static void e(object err)
	{
		if (Const.logType != LogType.None)
		{
			string str = string.Format ("{0}:[ERROR]{1}", System.DateTime.Now.ToShortTimeString (), err);
			EventSystem.Instance.FireEvent (EventCode.AddLog, str);
		}
	}

	public static void w(object  warn)
	{
		if (Const.logType != LogType.None)
		{
			string str = string.Format ("{0}:[WARN]{1}", System.DateTime.Now.ToShortTimeString (), warn);
			EventSystem.Instance.FireEvent (EventCode.AddLog, str);
		}
	}
#endif
}

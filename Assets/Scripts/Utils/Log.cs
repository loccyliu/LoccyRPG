/*
 * Log.cs
 * RpgFramework
 * Created by com.loccy on 10/28/2015 14:23:45.
 */

using UnityEngine;
using System.Collections;

public class Log 
{
	public static bool showLog  = true;

#if UNITY_EDITOR
	public static void i(object msg)
	{
		if (showLog)
			Debug.Log(msg);
	}

	public static void e(object err)
	{
		if (showLog)
			Debug.LogError(err);
	}

	public static void w(object  warn)
	{
		if (showLog)
			Debug.LogWarning(warn);
	}
#else
	public static void i(object msg)
	{
	}

	public static void e(object err)
	{
	}

	public static void w(object  warn)
	{
	}
#endif
}

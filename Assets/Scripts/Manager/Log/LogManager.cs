/*
 * LogManager.cs
 * RpgFramework
 * Created by com.loccy on 11/30/2015 15:45:46.
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System.IO;

public class LogManager : MonoBehaviour 
{
	string fileName = "log.txt";
	StringBuilder msg = new StringBuilder();

	Vector2 scrollview = new Vector2(960,100);

	void Start () 
	{
		RegisterHandler ();

		msg.AppendLine ("Log Start>>");

		if (Const.logType == LogType.LogFile)
		{
			fileName = Application.persistentDataPath + "/" + fileName;
			Log.i (fileName);

			if (!File.Exists (fileName))
			{
				try
				{
					File.Create (fileName);
				}
				catch (System.Exception e)
				{
					throw(e);
				}
			}
		}
	}

	void OnDistroy()
	{
		UnregisterHandler ();
	}

	void onMessage(object para)
	{
		msg.AppendLine (para.ToString ());
		if (Const.logType == LogType.LogFile)
		{
			File.AppendAllText(fileName,para.ToString()+"\n");
		}
	}

	void OnGUI()
	{
		if (Const.logType == LogType.LogScreen)
		{
			GUI.backgroundColor = Color.gray;
			GUI.color = Color.white;

			scrollview = GUILayout.BeginScrollView (scrollview);
			GUILayout.Label (msg.ToString (),GUILayout.Width(300));
			GUILayout.EndScrollView ();
		}
	}

	#region Handler 事件注册
	protected virtual void RegisterHandler()
	{
		EventSystem.Instance.RegistEvent(EventCode.AddLog, onMessage);
	}

	protected virtual void UnregisterHandler()
	{
		EventSystem.Instance.UnregistEvent(EventCode.AddLog, onMessage);
	}
	#endregion
}

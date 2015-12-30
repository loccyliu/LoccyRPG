/*
 * AppStartUp.cs
 * RpgFramework
 * Created by com.loccy on 10/28/2015 15:08:46.
 */

using UnityEngine;
using System.Collections;

public class GameStartUp : MonoBehaviour
{
	void Start()
	{
		InitApplication ();
	}

	void InitApplication()
	{
		if (Const.logType == LogType.LogScreen)
		{
			if(GameObject.Find("LogObj") != null)
				return;
			GameObject logGo = new GameObject ("LogObj");
			Util.Add<LogManager> (logGo);
			DontDestroyOnLoad(logGo);
		}

		if (GameManager.instance == null)
		{
			GameObject game = new GameObject ("GameManager");
			game.tag = "GameManager";
			game.AddComponent<GameManager> ();
		}
	}
}

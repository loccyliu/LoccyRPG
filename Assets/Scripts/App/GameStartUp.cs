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
		if (GameManager.instance == null)
		{
			GameObject game = new GameObject ("GameManager");
			game.tag = "GameManager";
			game.AddComponent<GameManager> ();
		}
	}
}

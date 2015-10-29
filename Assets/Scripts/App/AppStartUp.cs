/*
 * AppStartUp.cs
 * RpgDemo
 * Created by com.loccy on 10/28/2015 15:08:46.
 */

using UnityEngine;
using System.Collections;

public class AppStartUp : MonoBehaviour 
{
	void Start () 
	{
		InitApplication();
	}

	void InitApplication () 
	{
		if (AppManager.instance == null)
		{
			GameObject app = new GameObject("AppManager");
			app.AddComponent<DontDestroy>();

			app.AddComponent<ResourceManager>();
			app.AddComponent<StateManager>();
		}
	}
}

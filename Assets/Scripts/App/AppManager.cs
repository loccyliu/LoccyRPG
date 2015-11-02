/*
 * AppManager.cs
 * RpgFramework
 * Created by com.loccy on 10/28/2015 15:14:14.
 */

using UnityEngine;
using System.Collections;

public class AppManager : MonoBehaviour 
{
	public static AppManager instance;

	void Awake () 
	{
		instance = this;
	}
}

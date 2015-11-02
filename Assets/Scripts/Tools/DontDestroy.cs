/*
 * DontDestroy.cs
 * RpgFramework
 * Created by com.loccy on 10/28/2015 14:57:39.
 */

using UnityEngine;
using System.Collections;

public class DontDestroy : MonoBehaviour 
{
	void Start () 
	{
		DontDestroyOnLoad(gameObject);
	}
}

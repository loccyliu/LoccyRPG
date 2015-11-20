/*
 * LoadingView.cs
 * RpgFramework
 * Created by com.loccy on 11/03/2015 14:22:08.
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingView : MonoBehaviour 
{
	[SerializeField]
	Slider slider;

	AsyncOperation async = null;

	void Start () 
	{
		async = Application.LoadLevelAsync (Const.NextLevel);
	}

	void Update () 
	{
		if (slider != null && async != null) 
		{
			slider.value = async.progress;
		}
	}

}

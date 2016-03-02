/*
 * Loading.cs
 * Fast3
 * Created by com.sinodata on 01/11/2016 17:02:48.
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Loading : MonoBehaviour 
{
	[SerializeField]
	Slider loading_sli;
	AsyncOperation async;

	void Start () 
	{
		StartCoroutine (LoadLevel ());
	}
	
	IEnumerator LoadLevel () {
#if UNITY_5_3
		async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (GlobaData.nextLevel);
#else
		async = Application.LoadLevelAsync(GlobaData.nextLevel);
#endif
		yield return async;
	}

	void Update()
	{
		if (async.progress <= 1 && loading_sli) {
			loading_sli.value = async.progress;
		}
	}
}

/*
 * SoundManager.cs
 * RpgFramework
 * Created by com.loccy on 11/19/2015 14:41:46.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour 
{
	string[] res = {"Sound/button_tap","Sound/win","Sound/stop","Sound/spin","Sound/winall","Sound/cash"};
	Hashtable audiosource  = new Hashtable();
	
	void Awake () 
	{
		GameObject smgo = new GameObject ("SoundManager");
		DontDestroyOnLoad (smgo);
		RegisterHandler ();	
		AudioClip ac;
		for (int i = 0; i < res.Length; i++) {
			string name = res [i].Substring(6);
			ac = Resources.Load (res [i], typeof(AudioClip)) as AudioClip;
			GameObject go = new GameObject (name);
			go.transform.SetParent (smgo.transform);
			AudioSource a = Util.Add<AudioSource> (go);
			a.clip = ac;
			if (name == "spin")
				a.loop = true;
			audiosource.Add(name, a);
		}
	}

	void OnDestroy()
	{
		UnregisterHandler ();
	}

	#region Public Operation
	public void PlayBtnTap()
	{
		PlayClip("button_tap");
		if (audiosource.ContainsKey ("button_tap")) {
			AudioSource a = (AudioSource)audiosource ["button_tap"];
			a.Play ();
		}
	}

	public void PlayClip(string name)
	{
		if (audiosource.ContainsKey (name)) {
			AudioSource a = (AudioSource)audiosource [name];
			if (!a.isPlaying)
				a.Play ();
		}
	}

	public void StopClip(string name)
	{
		if (audiosource.ContainsKey (name)) {
			AudioSource a = (AudioSource)audiosource [name];
			a.Stop ();
		}
	}
	#endregion

	#region Handlers
	void onMusicVolume(object para)
	{
		if (audiosource.ContainsKey ("spin")) {
			AudioSource a = (AudioSource)audiosource ["spin"];
			a.volume = (float)para;
		}
	}

	void onSoundVolume(object para)
	{
		foreach (string key in audiosource.Keys) {
			AudioSource a = (AudioSource)audiosource [key];
			if (key != "spin")
				a.volume = (float)para;
		}
	}
	#endregion

	#region Handler 事件注册
	protected virtual void RegisterHandler ()
	{
		EventSystem.Instance.RegistEvent (EventCode.MusicVolumeChange, onMusicVolume);
		EventSystem.Instance.RegistEvent (EventCode.SoundVolumeChange, onSoundVolume);
	}

	protected virtual void UnregisterHandler ()
	{
		EventSystem.Instance.UnregistEvent (EventCode.MusicVolumeChange, onMusicVolume);
		EventSystem.Instance.UnregistEvent (EventCode.SoundVolumeChange, onSoundVolume);
	}
	#endregion
}

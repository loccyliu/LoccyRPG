/*
 * SoundManager.cs
 * RpgFramework
 * Created by com.loccy on 11/19/2015 14:41:46.
 */

using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour 
{
	private Hashtable audios = new Hashtable();
	
	void Start () 
	{
		RegisterHandler ();
	}

	void OnDestroy()
	{
		UnregisterHandler ();
	}

	#region Public Operation
	public void PlayBtnTap()
	{
	}

	public void PlayClip(string name)
	{
		
	}
	#endregion

	#region Handlers
	void onMusicVolume(object para)
	{
	}

	void onSoundVolume(object para)
	{
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

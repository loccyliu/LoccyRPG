/*
 * EventCode
 * 20150925 15:20:35
 * Loccy
 */

public class UIEventParams
{
	public UIWindowID windowID;
	public object args;

	public UIEventParams(UIWindowID id,object obj)
	{
		this.windowID = id;
		this.args = obj;
	}
}

public enum EventCode
{
	None = 0,
	AddLog,

	//==========Other===========
	ChangeStateMachine = 10,

	//===========UI=============
	EnableUIWindow = 20,
	UpdateUIWindow,
	DisableUIWindow,

	//=========Common===========
	MusicVolumeChange = 80,
	SoundVolumeChange,



	//==========GAME============
	GameEvent1 = 100,
	GameStart,

}




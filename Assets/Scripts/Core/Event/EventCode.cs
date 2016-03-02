/*
 * EventCode
 * 20150925 15:20:35
 * Loccy
 */

public class UIViewParams
{
	public UIWindowID windowID;
	public object args;

	public UIViewParams(UIWindowID id,object obj)
	{
		this.windowID = id;
		this.args = obj;
	}
}

public class UIDialogParams
{
	public UIDialogID dialogID;
	public System.Action<object> callback;
	public object args;

	public UIDialogParams(UIDialogID id,System.Action<object> cb,object obj)
	{
		this.dialogID = id;
		this.callback = cb;
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
	EnableDialog,
	DisableDialog,

	//=========Common===========
	MusicVolumeChange = 80,
	SoundVolumeChange,



	//==========GAME============
	GameEvent1 = 100,
	GameStart,

}




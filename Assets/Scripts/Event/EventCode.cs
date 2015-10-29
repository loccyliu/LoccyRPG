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

	//==========Other===========
	ChangeStateMachine = 1,

	//===========UI=============
	EnableUIWindow = 10,
	UpdateUIWindow,
	DisableUIWindow,



	//==========GAME============
	GameEvent1 = 100,

}




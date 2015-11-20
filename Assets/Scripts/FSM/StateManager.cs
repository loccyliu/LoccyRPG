using UnityEngine;
using System.Collections;

public class StateManager : MonoBehaviour 
{
	BaseState curState = null;


	void OnEnable () 
	{
		RegisterHandler();
	}

	void OnDisable()
	{
		curState.onExit();
		UnregisterHandler();
	}

	void Update () 
	{
		curState.onUpdate();
	}

	void onChangeState(object para)
	{
		BaseState st = (BaseState)para;
		if (st != null)
		{
			curState.onExit();
			curState = st;
			curState.onEnter();
		}
		else
		{
			Log.e("para is null");
		}
	}

	#region Handler 事件注册
	protected virtual void RegisterHandler()
	{
		EventSystem.Instance.RegistEvent(EventCode.ChangeStateMachine, onChangeState);
	}

	protected virtual void UnregisterHandler()
	{
		EventSystem.Instance.UnregistEvent(EventCode.ChangeStateMachine, onChangeState);
	}

	#endregion
}

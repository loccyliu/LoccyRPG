using UnityEngine;
using System.Collections.Generic;

public enum StateEnum
{
	Null,
	Loading,
	Login,
	Gaming,
	Quit,
}

public class StateManager : MonoBehaviour 
{
	StateEnum curState = StateEnum.Null;
	Dictionary<StateEnum,BaseState> stateMap = new Dictionary<StateEnum, BaseState>();

	public StateEnum CurState{
		get{ 
			return curState;
		}
	}

	void Awake()
	{
		InitStates ();
	}
	/// <summary>
	/// Inits the states.
	/// </summary>
	void InitStates()
	{
		stateMap [StateEnum.Loading] = new LoadingState ();
		stateMap [StateEnum.Login] = new LoginState ();
		stateMap [StateEnum.Gaming] = new GamingState ();
		stateMap [StateEnum.Quit] = new QuitState ();
	}

	void OnEnable () 
	{
		RegisterHandler();
	}

	void OnDisable()
	{
		stateMap[curState].onExit();
		UnregisterHandler();
	}

	void Update () 
	{
		stateMap[curState].onUpdate();
	}
	/// <summary>
	/// Ons the state of the change.
	/// </summary>
	/// <param name="para">Para.</param>
	void onChangeState(object para)
	{
		StateEnum se = (StateEnum)para;
		if (se != StateEnum.Null)
		{
			stateMap[curState].onExit();
			curState = se;
			stateMap[curState].onEnter();
		}
		else
		{
			Log.i("state is null");
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

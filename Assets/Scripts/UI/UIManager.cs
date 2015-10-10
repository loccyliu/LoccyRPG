/*
 * RpgDemo
 * UIManager.cs
 * Created by com.loccy on 10/09/2015 16:10:10.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 页面管理类
/// </summary>
public class UIManager : MonoBehaviour
{
	//-----------------------------UI页面类声明-----------------------//
	PlayerListClass plc;
	TestClass tc;


	//-----------------------------UI页面类声明-----------------------//

	private Stack<UIWindowID> viewStack = new Stack<UIWindowID>();


	void OnEnable()
	{
		RegisterHandler();
	}

	void OnDisable()
	{
		UnregisterHandler();
	}

	void Start()
	{
		InitUIClass();
	}

#region UI Method 【页面管理】

	/// <summary>
	/// 初始化UI类
	/// </summary>
	void InitUIClass()
	{
		plc = new PlayerListClass("UI/Player/PlayerListPop");
		tc = new TestClass("UI/Player/TestView");
	}

	void PushView(UIWindowID vid)
	{
		viewStack.Push(vid);
	}

	UIWindowID PopView()
	{
		if (viewStack.Count > 0)
		{
			return viewStack.Pop();
		}
		return UIWindowID.None;
	}

	UIWindowID PeekView()
	{
		if (viewStack.Count > 0)
		{
			return viewStack.Peek();
		}
		return UIWindowID.None;
	}

	/// <summary>
	/// Enables the view.
	/// </summary>
	/// <param name="para">Para.</param>
	void EnableView(object para)
	{
		UIEventParams up = (UIEventParams)para;
		if (up.windowID != UIWindowID.None)
		{
			UIWindowID preWin = PeekView();
			if (preWin != up.windowID)//打开的不是同一个窗口
			{
				if (preWin != UIWindowID.None)//关闭旧页面，关闭后不能打开上一层页面
				{
					EventSystem.Instance.FireEvent(EventCode.DisableUIWindow, new UIEventParams(preWin, true));
				}
				PushView(up.windowID);
			}
		}
//		print("Enable->");
//		PrintStack();

		//----------------------【处理打开UI页面】-----------------------//
		switch (up.windowID)
		{
		case UIWindowID.None:
			break;
		case UIWindowID.PlayerListPop:
			if (plc != null)
			{
				plc.Show();
			}
			break;
		case UIWindowID.TestView:
			if (tc != null)
			{
				tc.Show();
			}
			break;
		default:
			break;
		}
		//----------------------------------------------------//
	}

	/// <summary>
	/// Disables the view.
	/// </summary>
	/// <param name="para">Para.</param>
	void DisableView(object para)
	{
		UIEventParams up = (UIEventParams)para;

		bool isnew = false;
		bool.TryParse(up.args.ToString(), out isnew);

		//----------------------【处理关闭UI页面】-----------------------//
		switch (up.windowID)
		{
		case UIWindowID.None:
			break;
		case UIWindowID.PlayerListPop:
			if (plc != null)
				plc.Close();
			break;
		case UIWindowID.TestView:
			if (tc != null)
			{
				tc.Close();
			}
			break;
		default:
			break;
		}
		//---------------------------------------------------------//

		//如果是打开新页面而触发的关闭则不打开上一层页面，否则打开上一级页面
		if (isnew)
		{
			return;
		}
		if (up.windowID != UIWindowID.None)
		{
			if (PeekView() == up.windowID)//只能关闭栈顶窗口
			{
				PopView();//释放栈顶
				UIWindowID nextWin = PeekView();//打开下一个
				if (nextWin != UIWindowID.None)//打开新页面
				{
					EventSystem.Instance.FireEvent(EventCode.EnableUIWindow, new UIEventParams(nextWin, null));
				}
			}
		}

//		print("Disable->");
//		PrintStack();
	}

	void PrintStack()
	{
		foreach (UIWindowID id in viewStack)
		{
			print("id:" + id);
		}
		print("==========");
	}

#endregion

#region Handler 事件注册
	protected virtual void RegisterHandler()
	{
		EventSystem.Instance.RegistEvent(EventCode.EnableUIWindow, EnableView);
		EventSystem.Instance.RegistEvent(EventCode.DisableUIWindow, DisableView);
	}

	protected virtual void UnregisterHandler()
	{
		EventSystem.Instance.UnregistEvent(EventCode.EnableUIWindow, EnableView);
		EventSystem.Instance.UnregistEvent(EventCode.DisableUIWindow, DisableView);
	}

#endregion
}

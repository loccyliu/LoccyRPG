/*
 * UIManager.cs
 * RpgFramework
 * Created by com.loccy on 10/09/2015 16:10:10.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 页面管理类
/// </summary>
public class UIManager : MonoBehaviour
{
	/// <summary>
	/// The main ui root.
	/// </summary>
	private Transform root;

	Transform Root {
		get {
			if (root == null)
				root = PopRoot.parent;
			return root;
		}
	}

	/// <summary>
	/// The popview ui root.
	/// </summary>
	private Transform popRoot;

	Transform PopRoot {
		get {
			if (popRoot == null)
				popRoot = GameObject.FindObjectOfType<PopViewRoot> ().transform;
			return popRoot;
		}
	}

	//-----------------------------UI页面类声明-----------------------//
	MainUIClass mainui;




	//-----------------------------UI页面类声明-----------------------//
	/// <summary>
	/// The view stack.【页面管理栈】
	/// </summary>
	private Stack<UIWindowID> viewStack = new Stack<UIWindowID> ();
	/// <summary>
	/// The view dic.【页面存储字典】key为id
	/// </summary>
	private Dictionary<UIWindowID,UIClass> viewDic = new Dictionary<UIWindowID, UIClass> ();
	/// <summary>
	/// The classes.【页面存储表】key为名字
	/// </summary>
	private Dictionary<string, UIClass> classes = new Dictionary<string, UIClass> ();

	void OnEnable()
	{
		RegisterHandler ();
	}

	void OnDisable()
	{
		UnregisterHandler ();
	}

	void Start()
	{
		InitUIClass ();
	}


	#region UI Method 【页面管理】

	/// <summary>
	/// 初始化UI类
	/// </summary>
	void InitUIClass()
	{
		mainui = new MainUIClass ();
		viewDic.Add (UIWindowID.MainUI, mainui);
	}

	void PushView(UIWindowID vid)
	{
		//if(viewStack.Contains(vid))

		viewStack.Push (vid);
	}

	UIWindowID PopView()
	{
		if (viewStack.Count > 0)
		{
			return viewStack.Pop ();
		}
		return UIWindowID.None;
	}

	UIWindowID PeekView()
	{
		if (viewStack.Count > 0)
		{
			return viewStack.Peek ();
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
			UIWindowID preWin = PeekView ();
			//打开的不是同一个窗口
			if (preWin != up.windowID)
			{
				if (preWin != UIWindowID.None)
				{
					//关闭旧页面，关闭后不能打开上一层页面
					EventSystem.Instance.FireEvent (EventCode.DisableUIWindow, new UIEventParams (preWin, true));
				}
				PushView (up.windowID);
			}
		}

		//----------------------【处理打开UI页面】-----------------------//
		if (viewDic.ContainsKey (up.windowID))
		{
			viewDic [up.windowID].Show ();
		}
	}

	/// <summary>
	/// Disables the view.
	/// </summary>
	/// <param name="para">Para.</param>
	void DisableView(object para)
	{
		UIEventParams up = (UIEventParams)para;

		bool isnew = false;
		bool.TryParse (up.args.ToString (), out isnew);

		//----------------------【处理关闭UI页面】-----------------------//
		if (viewDic.ContainsKey (up.windowID))
		{
			viewDic [up.windowID].Close ();
		}

		//如果是打开新页面而触发的关闭则不打开上一层页面，否则打开上一级页面
		if (isnew)
		{
			return;
		}
		if (up.windowID != UIWindowID.None)
		{
			if (PeekView () == up.windowID)
			{
				//只能关闭栈顶窗口
				PopView ();//释放栈顶
				UIWindowID nextWin = PeekView ();//打开下一个
				if (nextWin != UIWindowID.None)
				{
					//打开新页面
					EventSystem.Instance.FireEvent (EventCode.EnableUIWindow, new UIEventParams (nextWin, null));
				}
			}
		}
	}

	/// <summary>
	/// Prints the stack.[Debug]
	/// </summary>
	void PrintStack()
	{
		foreach (UIWindowID id in viewStack)
		{
			Log.i ("id:" + id);
		}
		Log.i ("==========");
	}

	#endregion

	#region CreateView

	public void CreateView(string name, Action<GameObject> func)
	{
		StartCoroutine (OnCreateView (name, true, func));
	}

	public void CreateMainUI(string name, Action<GameObject> func)
	{
		StartCoroutine (OnCreateView (name, false, func));
	}

	IEnumerator OnCreateView(string name, bool ispop, Action<GameObject> func)
	{
		yield return StartCoroutine (Initialize ());

		string assetName = name + "View";
		// Load asset from assetBundle.
		string abName = name.ToLower () + ".unity3d";
		AssetBundleAssetOperation request = ResourceManager.LoadAssetAsync (abName, assetName, typeof(GameObject));
		if (request == null)
			yield break;
		yield return StartCoroutine (request);

		// Get the asset.
		GameObject prefab = request.GetAsset<GameObject> ();

		if (prefab == null)
		{
			Log.e ("prefab is null");
			yield break;
		}
		if (!ispop && Root.FindChild (name) != null || ispop && PopRoot.FindChild (name) != null)
			yield break;
		GameObject go = Instantiate (prefab) as GameObject;
		go.name = assetName;
		go.layer = LayerMask.NameToLayer ("UI");
		go.transform.SetParent (ispop ? PopRoot : Root);
		if (ispop)
			go.transform.SetAsLastSibling ();
		else
			go.transform.SetAsFirstSibling ();
		go.transform.localScale = Vector3.one;
		go.transform.localPosition = Vector3.zero;
		RectTransform rt = (RectTransform)(go.transform);
		rt.sizeDelta = new Vector2 (10, 10);

		if (func != null)
		{
			func (go);
		}
		Log.i (string.Format ("CreatePanel::>> {0} withh prefab ->【{1}】", name, prefab));
	}

	IEnumerator Initialize()
	{
		ResourceManager.BaseDownloadingURL = Util.GetRelativePath ();

		var request = ResourceManager.Initialize (Const.AssetDirname);
		if (request != null)
			yield return StartCoroutine (request);
	}

	public void CreateViewFromRes(string name,Action<GameObject> func)
	{
		string prefabName = name + "View";
		UnityEngine.Object prefab = Resources.Load (prefabName, typeof(GameObject));

		if (prefab == null)
		{
			Log.e ("prefab is null");
			return;
		}
		if (PopRoot.FindChild (prefabName) != null)
			return;
		GameObject go = Instantiate (prefab) as GameObject;
		go.name = prefabName;
		go.layer = LayerMask.NameToLayer ("UI");
		go.transform.SetParent (PopRoot);
		go.transform.SetAsLastSibling ();
		go.transform.localScale = Vector3.one;
		go.transform.localPosition = Vector3.zero;
		RectTransform rt = (RectTransform)(go.transform);
		rt.sizeDelta = new Vector2 (10, 10);

		if (func != null)
		{
			func (go);
		}
		Log.i (string.Format ("CreatePanel::>> {0} withh prefab ->【{1}】", prefabName, prefab));
	}

	/// <summary>
	/// Gets the uiclass.
	/// </summary>
	/// <returns>The uiclass.</returns>
	/// <param name="className">Class name.</param>
	public UIClass GetUIClass(string className)
	{
		UIClass uc = null;
		if (classes == null || !classes.ContainsKey (className))
			return null;
		classes.TryGetValue (className, out uc);
		return uc;
	}

	#endregion

	#region Handler 事件注册
	protected virtual void RegisterHandler()
	{
		EventSystem.Instance.RegistEvent (EventCode.EnableUIWindow, EnableView);
		EventSystem.Instance.RegistEvent (EventCode.DisableUIWindow, DisableView);
	}

	protected virtual void UnregisterHandler()
	{
		EventSystem.Instance.UnregistEvent (EventCode.EnableUIWindow, EnableView);
		EventSystem.Instance.UnregistEvent (EventCode.DisableUIWindow, DisableView);
	}
	#endregion
}

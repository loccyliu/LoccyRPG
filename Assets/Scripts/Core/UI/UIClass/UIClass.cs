/*
 * UIClass
 * 20151008 11:35:23
 * Loccy
 */
using UnityEngine;
using System;

public class UIClass
{
	public string resName;

	protected GameObject viewObj;
	protected UIView view;

	public UIClass()
	{
		RegisterHandler ();
	}

	public UIClass(string name)
	{
		resName = name;
		RegisterHandler ();
	}

	~UIClass ()
	{
		UnregisterHandler ();
	}

	public virtual void Show()
	{
		if (view == null)
		{
			if (viewObj == null)
			{
				ioo.uiManager.CreateView (resName, (go) => {
					viewObj = go;
					view = Util.Add<UIView> (viewObj);
					view.Show ();
				});
				return;
			}
			else
				view = viewObj.GetComponent<UIView> ();
		}
		view.Show ();
	}

	public virtual void UpdateData(object data)
	{
		if (view != null)
			view.UpdateUI (data);
	}

	public virtual void OnMessge(int protocolId, ByteBuffer buff)
	{
	}

	public virtual void Close()
	{
		if (view != null)
		{
			view.Close ();
		}
	}


	#region Handler

	protected virtual void RegisterHandler()
	{
		EventSystem.Instance.RegistEvent (EventCode.UpdateUIWindow, UpdateData);
	}

	protected virtual void UnregisterHandler()
	{
		EventSystem.Instance.UnregistEvent (EventCode.UpdateUIWindow, UpdateData);
	}

	#endregion
}
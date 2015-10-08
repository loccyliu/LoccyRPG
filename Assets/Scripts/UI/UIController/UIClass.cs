/*
 * UIClass
 * 20151008 11:35:23
 * Loccy
 */
using UnityEngine;
using System;

public class UIClass
{
	protected GameObject prefab;
	protected GameObject viewObj;
	protected UIView view;
	protected object data;

	public UIClass()
	{
		
	}

	public UIClass(string resource)
	{
		prefab = Resources.Load(resource) as GameObject;
	}

	public UIClass(string resource, string dt)
	{
		prefab = Resources.Load(resource) as GameObject;
		data = dt;
	}

	public virtual void Show()
	{
		RegisterHandler();

		if (view == null)
		{
			if (prefab == null)
				return;
			PopViewRoot pv = GameObject.FindObjectOfType<PopViewRoot>();//PopViewRoot 管理PopView等 
			if (pv == null)
				return;
			viewObj = MonoBehaviour.Instantiate(prefab) as GameObject;
			viewObj.name = prefab.name;
			viewObj.transform.SetParent(pv.transform);
			viewObj.transform.localPosition = Vector3.zero;
			viewObj.transform.localScale = Vector3.one;

			view = viewObj.GetComponent<UIView>();//AddCompenent


		}
		view.Show();
	}

	public virtual void UpdateData(object data)
	{
		if (view != null)
			view.UpdateUI(data);
	}

	public virtual void Close()
	{
		UnregisterHandler();

		if (view != null)
		{
			view.Close();
		}
	}

	void EnableView(object para)
	{
		UpdateData(para);
		Show();
	}

	void DisableView(object para)
	{
		Close();
	}

#region Handler

	protected virtual void RegisterHandler()
	{
		EventSystem.Instance.RegistEvent(EventCode.EnableUIWindow, EnableView);
		EventSystem.Instance.RegistEvent(EventCode.UpdateUIWindow, UpdateData);
		EventSystem.Instance.RegistEvent(EventCode.DisableUIWindow, DisableView);
	}

	protected virtual void UnregisterHandler()
	{
		EventSystem.Instance.UnregistEvent(EventCode.EnableUIWindow, EnableView);
		EventSystem.Instance.UnregistEvent(EventCode.UpdateUIWindow, UpdateData);
		EventSystem.Instance.UnregistEvent(EventCode.DisableUIWindow, DisableView);
	}

#endregion
}
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

	public UIClass()
	{
		
	}

	public UIClass(string resource)
	{
		RegisterHandler();
		prefab = Resources.Load(resource) as GameObject;
	}

	~UIClass()
	{
		UnregisterHandler();
	}

	public virtual void Show()
	{
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
		if (view != null)
		{
			view.Close();
		}
	}


#region Handler

	protected virtual void RegisterHandler()
	{
		EventSystem.Instance.RegistEvent(EventCode.UpdateUIWindow, UpdateData);
	}

	protected virtual void UnregisterHandler()
	{
		EventSystem.Instance.UnregistEvent(EventCode.UpdateUIWindow, UpdateData);
	}

#endregion
}
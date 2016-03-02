/*
 * UIDialogClass.cs
 * Rpg
 * Created by com.sinodata on 01/06/2016 10:10:33.
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public abstract class UIDialogClass 
{
	public string resName;

	protected GameObject viewObj;
	protected UIDialogView dialog;

	public UIDialogClass()
	{
		RegisterHandler ();
	}

	public UIDialogClass(string name)
	{
		resName = name;
		RegisterHandler ();
	}

	~UIDialogClass ()
	{
		UnregisterHandler ();
	}

	public virtual void Show(UIDialogParams para)
	{
		if (dialog == null)
		{
			if (viewObj == null)
			{
				ioo.uiManager.CreateView (resName, (go) => {
					viewObj = go;
					dialog = Util.Add<UIDialogView> (viewObj);
					dialog.Show (para);
				});
				return;
			}
			else
				dialog = viewObj.GetComponent<UIDialogView> ();
		}
		dialog.Show (para);
	}

	public virtual void Close(UIDialogParams para)
	{
		if (dialog != null)
		{
			dialog.Close (para);
		}
	}

	public virtual void UpdateData(object data)
	{
		if (dialog != null)
			dialog.UpdateUI (data);
	}

	public virtual void OnMessge(int protocolId, ByteBuffer buff)
	{
	}

	public virtual void Close()
	{
		if (dialog != null)
		{
			dialog.Close (null);
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

/*
 * UIDialogView.cs
 * Fast3
 * Created by com.sinodata on 01/06/2016 10:10:49.
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public abstract class UIDialogView : MonoBehaviour 
{

	/// <summary>
	/// The type of the dispear.
	/// </summary>
	public ViewDispearType dispearType = ViewDispearType.None;
	protected Action<object> callback;

	bool isShow = false;

	public virtual void Show(UIDialogParams para, ViewDispearType ty = ViewDispearType.None)
	{
		if (isShow)
			return;
		isShow = true;
		this.callback = para.callback;
		ViewDispear.AnimationShow(gameObject, dispearType);
		gameObject.SetActive(true);
	}

	public virtual void UpdateUI(object data)
	{

	}

	public virtual void Close(UIDialogParams para,ViewDispearType ty = ViewDispearType.None)
	{
		isShow = false;
		callback = para.callback;
		ViewDispear.AnimationClose(gameObject, dispearType);
	}

}


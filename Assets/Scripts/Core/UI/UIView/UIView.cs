/*
 * RpgFramework
 * UIView.cs
 * Created by com.loccy on 10/08/2015 15:15:49.
 */

using UnityEngine;
using System.Collections;
using DG.Tweening;

public class UIView : MonoBehaviour
{
	/// <summary>
	/// The type of the dispear.
	/// </summary>
	public ViewDispearType dispearType = ViewDispearType.None;

	bool isShow = false;

	public virtual void UpdateUI(object data)
	{

	}

	public virtual void Show(ViewDispearType ty = ViewDispearType.None)
	{
		if (isShow)
			return;
		isShow = true;
		ViewDispear.AnimationShow(gameObject, dispearType);
		gameObject.SetActive(true);
	}

	public virtual void Close(ViewDispearType ty = ViewDispearType.None)
	{
		ioo.soundManager.PlayBtnTap ();
		isShow = false;
		ViewDispear.AnimationClose(gameObject, dispearType);
	}
}

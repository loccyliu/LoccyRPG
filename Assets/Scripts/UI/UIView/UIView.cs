/*
 * RpgDemo
 * UIView.cs
 * Created by com.loccy on 10/08/2015 15:15:49.
 */

using UnityEngine;
using System.Collections;
using DG.Tweening;

public class UIView : MonoBehaviour
{

	ViewDispearType dispearType = ViewDispearType.Left2Right;

	bool isShow = false;

	public virtual void UpdateUI(object data)
	{
	
	}

	public virtual void Show()
	{
		if (isShow)
			return;
		isShow = true;
		ViewDispear.AnimationShow(gameObject, dispearType);
		gameObject.SetActive(true);
	}

	public virtual void Close()
	{
		isShow = false;

		ViewDispear.AnimationClose(gameObject, dispearType);
		//gameObject.SetActive(false);
	}
}

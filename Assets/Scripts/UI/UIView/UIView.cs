/*
 * RpgDemo
 * UIView.cs
 * Created by com.loccy on 10/08/2015 15:15:49.
 */

using UnityEngine;
using System.Collections;

public class UIView : MonoBehaviour
{
	void Start()
	{
	
	}

	public virtual void UpdateUI(object data)
	{
	
	}

	public virtual void Show()
	{
		gameObject.SetActive(true);
	}

	public virtual void Close()
	{
		gameObject.SetActive(false);
	}
}

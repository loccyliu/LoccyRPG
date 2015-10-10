/*
 * RpgDemo
 * TestView.cs
 * Created by com.loccy on 10/08/2015 16:26:27.
 */

using UnityEngine;
using System.Collections;

public class TestView : UIView
{
	void Start()
	{
	
	}

	public override void Show()
	{
		base.Show();
		//获取数据源，刷新数据

	}

	public override void UpdateUI(object data)
	{
		base.UpdateUI(data);
		//获取数据源，刷新数据
	}

	public override void Close()
	{
		base.Close();
	}
}

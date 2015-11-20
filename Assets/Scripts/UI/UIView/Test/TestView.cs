/*
 * RpgFramework
 * TestView.cs
 * Created by com.loccy on 10/08/2015 16:26:27.
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TestView : UIView
{
	public Button close_btn;

	void Awake () 
	{
		dispearType = ViewDispearType.Left2RightLinear;
		close_btn = Util.Get<Button> (transform, "Close_Button");
		close_btn.onClick.AddListener (onClose);
	}

	void onClose()
	{
		EventSystem.Instance.FireEvent (EventCode.DisableUIWindow, new UIEventParams (UIWindowID.TestView, "TestView"));
	}

	public override void Show(ViewDispearType ty = ViewDispearType.None)
	{
		base.Show(dispearType);
		//获取数据源，刷新数据

	}

	public override void UpdateUI(object data)
	{
		base.UpdateUI(data);
		//获取数据源，刷新数据
	}

	public override void Close(ViewDispearType ty = ViewDispearType.None)
	{
		base.Close(dispearType);
	}
}

/*
 * RpgFramework
 * PlayerListView.cs
 * Created by com.loccy on 10/09/2015 16:24:31.
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerListView : UIView 
{
	public Button close_btn;
	
	void Awake () 
	{
		dispearType = ViewDispearType.Right2LeftBounce;
		close_btn = Util.Get<Button> (transform, "Close_Button");
		close_btn.onClick.AddListener (onClose);
	}

	void onClose()
	{
		EventSystem.Instance.FireEvent (EventCode.DisableUIWindow, new UIEventParams (UIWindowID.PlayerListPop, "PlayerList"));
	}

	void Update () 
	{
	
	}

	public override void Show (ViewDispearType ty = ViewDispearType.None)
	{
		base.Show (dispearType);
		//获取数据源
	}

	public override void UpdateUI (object data)
	{
		base.UpdateUI (data);
	}

	public override void Close (ViewDispearType ty = ViewDispearType.None)
	{
		base.Close (dispearType);
	}
}

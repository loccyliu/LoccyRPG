/*
 * MainUIView.cs
 * RpgFramework
 * Created by com.loccy on 11/20/2015 09:42:20.
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainUIView : UIView 
{
	public Button top1_tbn,top2_tbn,top3_tbn,bot1_tbn,bot2_tbn,bot3_tbn;

	void Start () 
	{
		dispearType = ViewDispearType.None;
		top1_tbn = Util.Get<Button> (transform, "Button1");
		top2_tbn = Util.Get<Button> (transform, "Button2");
		top3_tbn = Util.Get<Button> (transform, "Button3");
		bot1_tbn = Util.Get<Button> (transform, "Button4");
		bot2_tbn = Util.Get<Button> (transform, "Button5");
		bot3_tbn = Util.Get<Button> (transform, "Button6");
		Util.Get<Text>(bot3_tbn.gameObject,"Text").text = "TestView";
		Util.Get<Text>(bot2_tbn.gameObject,"Text").text = "PlayerListView";

		top1_tbn.onClick.AddListener (onTop1);
		top2_tbn.onClick.AddListener (onTop2);
		top3_tbn.onClick.AddListener (onTop3);
		bot1_tbn.onClick.AddListener (onBot1);
		bot2_tbn.onClick.AddListener (onBot2);
		bot3_tbn.onClick.AddListener (onBot3);
	}

	void Update () 
	{

	}

	void onTop1()
	{
		Log.i(">>top left");
	}

	void onTop2()
	{
		Log.i(">>top center");
	}

	void onTop3()
	{
		Log.i(">>top right");
	}

	void onBot1()
	{
		Log.i(">>bot left");
	}

	void onBot2()
	{
		Log.i(">>bot center");
		EventSystem.Instance.FireEvent (EventCode.EnableUIWindow, new UIEventParams (UIWindowID.PlayerListPop, "PlayerList"));
	}

	void onBot3()
	{
		Log.i(">>bot right");
		EventSystem.Instance.FireEvent (EventCode.EnableUIWindow, new UIEventParams (UIWindowID.TestView, "TestView"));
	}

	public override void UpdateUI(object data)
	{
		base.UpdateUI (data);
	}

	public override void Show(ViewDispearType ty = ViewDispearType.None)
	{
		base.Show (dispearType);
	}

	public override void Close(ViewDispearType ty = ViewDispearType.None)
	{
		base.Close (dispearType);
	}

}

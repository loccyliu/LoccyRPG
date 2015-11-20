/*
 * MainUIClass.cs
 * RpgFramework
 * Created by com.loccy on 11/20/2015 09:42:06.
 */

using UnityEngine;
using System.Collections;

public class MainUIClass : UIClass
{
	public MainUIClass()
	{
		resName = UIClassNames.MainUI;
	}

	public override void Show()
	{
		if (view == null)
		{
			if (viewObj == null)
			{
				ioo.uiManager.CreateView (resName, (go) => {
					viewObj = go;
					view = Util.Add<MainUIView> (viewObj);
					view.Show ();
				});
				return;
			}
			else
				view = viewObj.GetComponent<MainUIView> ();
		}
		view.Show ();
	}

	public override void UpdateData(object data)
	{
		base.UpdateData (data);
	}

	public override void OnMessge(int protocolId, ByteBuffer buff)
	{
		base.OnMessge (protocolId, buff);
	}

	public override void Close()
	{
		base.Close ();
	}

	protected override void RegisterHandler()
	{
		base.RegisterHandler ();
	}

	protected override void UnregisterHandler()
	{
		base.UnregisterHandler ();
	}
}
/*
 * RpgFramework
 * PlayerListClass.cs
 * Created by com.loccy on 10/09/2015 16:24:18.
 */

using UnityEngine;
using System.Collections;

public class PlayerListClass : UIClass
{
	public PlayerListClass(string name)
	{
		resName = name;
	}

	public override void Show()
	{
		if (view == null)
		{
			if (viewObj == null)
			{
				ioo.uiManager.CreatePopView (resName, (go) => {
					if (go != null)
					{
						viewObj = go;
						view = Util.Add<PlayerListView> (viewObj);
						view.Show ();
					}
				});
				return;
			}
			else
				view = viewObj.GetComponent<PlayerListView> ();
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

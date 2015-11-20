/*
 * RpgFramework
 * TestClass.cs
 * Created by com.loccy on 10/08/2015 16:32:54.
 */

using UnityEngine;
using System.Collections;

public class TestClass : UIClass
{
	public TestClass()
	{
		resName = UIClassNames.Test;
	}

	public override void Show()
	{
		if (view == null)
		{
			if (viewObj == null)
			{
				ioo.uiManager.CreateView (resName, (go) => {
					viewObj = go;
					view = Util.Add<TestView> (viewObj);
					view.Show ();
				});
				return;
			}
			else
				view = viewObj.GetComponent<TestView> ();
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

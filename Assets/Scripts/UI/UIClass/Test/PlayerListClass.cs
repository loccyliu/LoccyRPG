/*
 * RpgDemo
 * PlayerListClass.cs
 * Created by com.loccy on 10/09/2015 16:24:18.
 */

using UnityEngine;
using System.Collections;

public class PlayerListClass : UIClass 
{
	public PlayerListClass(string resource)
	{
		prefab = Resources.Load(resource) as GameObject;
	}

	public override void Show ()
	{
		if (view == null)
		{
			if (prefab == null)
				return;
			PopViewRoot pv = GameObject.FindObjectOfType<PopViewRoot>();//PopViewRoot 管理PopView等 
			if (pv == null)
				return;
			viewObj = MonoBehaviour.Instantiate(prefab) as GameObject;
			viewObj.name = prefab.name;
			viewObj.transform.SetParent(pv.transform);
			viewObj.transform.localPosition = Vector3.zero;
			viewObj.transform.localScale = Vector3.one;

			view = viewObj.AddComponent<PlayerListView>();//AddCompenent
		}
		view.Show();
	}

	public override void UpdateData (object data)
	{
		base.UpdateData(data);
	}

	public override void Close ()
	{
		base.Close ();
	}

	protected override void RegisterHandler ()
	{
		base.RegisterHandler ();
	}

	protected override void UnregisterHandler ()
	{
		base.UnregisterHandler ();
	}
}

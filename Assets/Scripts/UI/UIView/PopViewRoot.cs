/*
 * RpgFramework
 * PopViewRoot.cs
 * Created by com.loccy on 10/08/2015 16:02:29.
 */

using UnityEngine;
using System.Collections.Generic;

public class PopViewRoot : MonoBehaviour
{
	/*
	private uint _vid = 0;
	private Stack<uint> viewStack = new Stack<uint>();
	private Dictionary<uint,UIView> views = new Dictionary<uint, UIView>();

	public void PushView(UIView view)
	{
		_vid++;
		if (viewStack.Contains(_vid))
		{
			viewStack.Pop();
		}
		if (views.ContainsKey(_vid))
		{
			views.Remove(_vid);
		}
		viewStack.Push(_vid);
		views.Add(_vid, view);
	}

	public UIView PopView()
	{
		if (viewStack.Count > 0)
		{
			uint vid = viewStack.Pop();
			return views[vid];
		}
		return null;
	}*/
}
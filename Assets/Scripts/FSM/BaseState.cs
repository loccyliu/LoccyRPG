using UnityEngine;
using System.Collections;

public abstract class BaseState : MonoBehaviour 
{
	public virtual void onEnter()
	{
		Resources.UnloadUnusedAssets();
		System.GC.Collect();
	}

	public virtual void onUpdate()
	{	
	}

	public virtual void onExit()
	{
		Resources.UnloadUnusedAssets();
		System.GC.Collect();
	}
}

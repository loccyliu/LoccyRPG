/*
 * ViewDispear.cs
 * RpgFramework
 * Created by com.loccy on 10/28/2015 17:17:28.
 */

using UnityEngine;
using System.Collections;
using DG.Tweening;

public enum ViewDispearType
{
	None,
	Up2Down,
	Down2Up,
	Left2Right,
	Right2Left,
	Fade,
}

public class ViewDispear 
{
	private static Tweener tw = null;

	public static void AnimationShow(GameObject go, ViewDispearType ty)
	{
		if (go == null)
			return;

		if (tw != null && !tw.IsComplete ()) 
		{
			tw.Complete ();
		}

		switch (ty)
		{
		case ViewDispearType.Down2Up:
			{
				go.transform.localPosition = new Vector3(0, -1000, 0);
				tw = go.transform.DOLocalMoveY(0, 0.5f);
			}
			break;
		case ViewDispearType.Up2Down:
			{
				go.transform.localPosition = new Vector3(0, 1000, 0);
				tw = go.transform.DOLocalMoveY(0, 0.5f);
			}
			break;
		case ViewDispearType.Left2Right:
			{
				go.transform.localPosition = new Vector3(-1000, 0, 0);
				tw = go.transform.DOLocalMoveX(0, 0.5f);
			}
			break;
		case ViewDispearType.Right2Left:
			{
				go.transform.localPosition = new Vector3(1000, 0, 0);
				tw = go.transform.DOLocalMoveX(0, 0.5f);
			}
			break;
		}
	}

	public static void AnimationClose(GameObject go, ViewDispearType ty)
	{
		if (go == null)
			return;

		if (tw != null && !tw.IsComplete ()) 
		{
			tw.Complete ();
		}

		switch (ty)
		{
		case ViewDispearType.Down2Up:
			{
				tw = go.transform.DOLocalMoveY(-2000, 0.5f).OnComplete(()=>{go.SetActive(false);});
			}
			break;
		case ViewDispearType.Up2Down:
			{
				tw = go.transform.DOLocalMoveY(2000, 0.5f).OnComplete(()=>{go.SetActive(false);});
			}
			break;
		case ViewDispearType.Left2Right:
			{
				tw = go.transform.DOLocalMoveX(-2000, 0.5f).OnComplete(()=>{go.SetActive(false);});
			}
			break;
		case ViewDispearType.Right2Left:
			{
				tw = go.transform.DOLocalMoveX(2000, 0.5f).OnComplete(()=>{go.SetActive(false);});
			}
			break;


		default:
			break;
		}


	}

}
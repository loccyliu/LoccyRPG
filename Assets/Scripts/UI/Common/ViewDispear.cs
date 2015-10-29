/*
 * ViewDispear.cs
 * RpgDemo
 * Created by com.loccy on 10/28/2015 17:17:28.
 */

using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ViewDispear 
{
	public static void AnimationShow(GameObject go, ViewDispearType ty)
	{
		if (go == null)
			return;

		switch (ty)
		{
		case ViewDispearType.Down2Up:
			{
				go.transform.localPosition = new Vector3(0, -1000, 0);
				go.transform.DOLocalMoveY(0, 0.5f);
			}
			break;
		case ViewDispearType.Up2Down:
			{
				go.transform.localPosition = new Vector3(0, 1000, 0);
				go.transform.DOLocalMoveY(0, 0.5f);
			}
			break;
		case ViewDispearType.Left2Right:
			{
				go.transform.localPosition = new Vector3(-1000, 0, 0);
				go.transform.DOLocalMoveX(0, 0.5f);
			}
			break;
		case ViewDispearType.Right2Left:
			{
				go.transform.localPosition = new Vector3(1000, 0, 0);
				go.transform.DOLocalMoveX(0, 0.5f);
			}
			break;
		}
	}

	public static void AnimationClose(GameObject go, ViewDispearType ty)
	{
		if (go == null)
			return;

		Tweener tw = null;

		switch (ty)
		{
		case ViewDispearType.Down2Up:
			{
				go.transform.DOLocalMoveY(-2000, 0.5f).OnComplete(()=>{go.SetActive(false);});
			}
			break;
		case ViewDispearType.Up2Down:
			{
				go.transform.DOLocalMoveY(2000, 0.5f).OnComplete(()=>{go.SetActive(false);});
			}
			break;
		case ViewDispearType.Left2Right:
			{
				go.transform.DOLocalMoveX(-2000, 0.5f).OnComplete(()=>{go.SetActive(false);});
			}
			break;
		case ViewDispearType.Right2Left:
			{
				go.transform.DOLocalMoveX(2000, 0.5f).OnComplete(()=>{go.SetActive(false);});
			}
			break;



		default:
			break;
		}


	}

}
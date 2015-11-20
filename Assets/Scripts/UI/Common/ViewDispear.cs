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
	Up2DownLinear,
	Down2UpLinear,
	Left2RightLinear,
	Right2LeftLinear,
	Up2DownBounce,
	Down2UpBounce,
	Left2RightBounce,
	Right2LeftBounce,
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
		case ViewDispearType.Down2UpLinear:
			{
				go.transform.localPosition = new Vector3(0, -1000, 0);
				tw = go.transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.Linear);
			}
			break;
		case ViewDispearType.Up2DownLinear:
			{
				go.transform.localPosition = new Vector3(0, 1000, 0);
				tw = go.transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.Linear);
			}
			break;
		case ViewDispearType.Left2RightLinear:
			{
				go.transform.localPosition = new Vector3(-1000, 0, 0);
				tw = go.transform.DOLocalMoveX(0, 0.5f).SetEase(Ease.OutBounce);
			}
			break;
		case ViewDispearType.Right2LeftLinear:
			{
				go.transform.localPosition = new Vector3(1000, 0, 0);
				tw = go.transform.DOLocalMoveX(0, 0.5f).SetEase(Ease.OutBounce);
			}
			break;
		case ViewDispearType.Down2UpBounce:
			{
				go.transform.localPosition = new Vector3(0, -1000, 0);
				tw = go.transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.OutBounce);
			}
			break;
		case ViewDispearType.Up2DownBounce:
			{
				go.transform.localPosition = new Vector3(0, 1000, 0);
				tw = go.transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.OutBounce);
			}
			break;
		case ViewDispearType.Left2RightBounce:
			{
				go.transform.localPosition = new Vector3(-1000, 0, 0);
				tw = go.transform.DOLocalMoveX(0, 0.5f).SetEase(Ease.OutBounce);
			}
			break;
		case ViewDispearType.Right2LeftBounce:
			{
				go.transform.localPosition = new Vector3(1000, 0, 0);
				tw = go.transform.DOLocalMoveX(0, 0.5f).SetEase(Ease.OutBounce);
			}
			break;
		case ViewDispearType.Fade:
			{
				//go.GetComponent<UnityEngine.UI.Image> ().DOFade (1, 1);
				//go.GetComponent<UnityEngine.UI.Image> ().DOFade (1, 0.5f).SetEase (Ease.Linear);
			}
			break;

		default:
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
		case ViewDispearType.Down2UpLinear:
			{
				tw = go.transform.DOLocalMoveY(-2000, 0.5f).SetEase(Ease.Linear).OnComplete(()=>{go.SetActive(false);});
			}
			break;
		case ViewDispearType.Up2DownLinear:
			{
				tw = go.transform.DOLocalMoveY(2000, 0.5f).SetEase(Ease.Linear).OnComplete(()=>{go.SetActive(false);});
			}
			break;
		case ViewDispearType.Left2RightLinear:
			{
				tw = go.transform.DOLocalMoveX(-2000, 0.5f).SetEase(Ease.Linear).OnComplete(()=>{go.SetActive(false);});
			}
			break;
		case ViewDispearType.Right2LeftLinear:
			{
				tw = go.transform.DOLocalMoveX(2000, 0.5f).SetEase(Ease.Linear).OnComplete(()=>{go.SetActive(false);});
			}
			break;
		case ViewDispearType.Down2UpBounce:
			{
				tw = go.transform.DOLocalMoveY(-2000, 0.5f).SetEase(Ease.InBounce).OnComplete(()=>{go.SetActive(false);});
			}
			break;
		case ViewDispearType.Up2DownBounce:
			{
				tw = go.transform.DOLocalMoveY(2000, 0.5f).SetEase(Ease.InBounce).OnComplete(()=>{go.SetActive(false);});
			}
			break;
		case ViewDispearType.Left2RightBounce:
			{
				tw = go.transform.DOLocalMoveX(-2000, 0.5f).SetEase(Ease.InBounce).OnComplete(()=>{go.SetActive(false);});
			}
			break;
		case ViewDispearType.Right2LeftBounce:
			{
				tw = go.transform.DOLocalMoveX(2000, 0.5f).SetEase(Ease.InBounce).OnComplete(()=>{go.SetActive(false);});
			}
			break;
		case ViewDispearType.Fade:
			{
				//go.GetComponent<UnityEngine.UI.Image> ().DOFade (0, 0.5f).SetEase (Ease.Linear).OnComplete(()=>{go.SetActive(false);});
			}
			break;

		default:
			{
				go.SetActive (false);
			}
			break;
		}


	}

}
/*******************************************************************
* Summary:
* Author :
* Date   : 20150714 16:05
*******************************************************************/
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TweenRotation : Tween 
{
    public Vector3 from = new Vector3(0, 0, 0);
    public Vector3 to = new Vector3(0, 0, 0);
    public bool isRelative = false;
    public override void Start()
    {
        base.Start();
        if (isRelative)
        {
            transform.localRotation = Quaternion.Euler(from);
            tw = transform.DOLocalRotate(to, duration);
        }
        else
        {
            transform.rotation = Quaternion.Euler(from);
            tw = transform.DORotate(to, duration);
        }

        if (useCurve)
            tw.SetEase(animationCurve);
        else
            tw.SetEase(easeType);

        tw.SetSpeedBased(speedBase);
        tw.SetUpdate(ignoreTimeScale);
        tw.SetRelative(isRelative);
        tw.SetLoops(loopTimes, loopType);
        tw.SetDelay(delay);
        tw.OnComplete(Execute);
    }
	
	
}

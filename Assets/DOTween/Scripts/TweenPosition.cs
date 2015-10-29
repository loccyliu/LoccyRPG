/*******************************************************************
* Summary: 
* Author : 
* Date   : 20150714 16:05
*******************************************************************/
using UnityEngine;
using System.Collections.Generic;
//using Holoville.HOTween;

using DG.Tweening;

[AddComponentMenu("Tween/TweenPosition")]
public class TweenPosition : Tween 
{
    public Vector3 from = new Vector3(0, 0, 0);
    public Vector3 to = new Vector3(1, 0, 0);
    public bool isRelative = false;

    public override void Start()
    {
        base.Start();
        if (isRelative)
        {
            transform.localPosition = from;
            tw = transform.DOLocalMove(to, duration);
        }
        else
        {
            transform.position = from;
            tw = transform.DOMove(to, duration);
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

/*******************************************************************
* Summary: TweenScale
* Author : 
* Date   : 20150714 16:05
*******************************************************************/
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TweenScale : Tween
{
    public Vector3 from = new Vector3(1, 1, 1);
    public Vector3 to = new Vector3(1, 1, 1);
    public bool isRelative = false;

    public override void Start()
    {
        base.Start();
        transform.localScale = from;

        tw = transform.DOScale(to, duration);
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
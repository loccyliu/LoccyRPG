/*******************************************************************
* Summary: 
* Author : 
* Date   : 
*******************************************************************/
using UnityEngine;
using System.Collections;
using DG.Tweening;

[AddComponentMenu("Tween/TweenAlpha")]
public class TweenAlpha : Tween 
{
    public enum Target
    {
        Camera,
        Material,
        Light
    }
    [Range(0,1)]
    public float from = 1.0f;
    [Range(0, 1)]
    public float to = 1.0f;
    public Target target = Target.Material;

    public override void Start()
    {
        base.Start();

        Material mat = null;
        //mat.DOFade
        if (target== Target.Material)
        {
           //gameObject.renderer.material
        }
        else if (target == Target.Light)
        {
        }
        else
        { 
        }


        tw = transform.DOScale(to, duration);
        if (useCurve)
            tw.SetEase(animationCurve);
        else
            tw.SetEase(easeType);

        tw.SetSpeedBased(speedBase);
        tw.SetUpdate(ignoreTimeScale);
        tw.SetLoops(loopTimes, loopType);
        tw.SetDelay(delay);
        tw.OnComplete(Execute);
    }
}

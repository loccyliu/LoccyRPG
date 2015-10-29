/*******************************************************************
* Summary: Tween
* Author : 
* Date   : 20150714 16:05
*******************************************************************/
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using DG.Tweening;

public class Tween : MonoBehaviour 
{
    [HideInInspector]
    public LoopType loopType = LoopType.Restart;
    [HideInInspector]
    public int loopTimes = 1;
    [HideInInspector]
    public UpdateType updateType = UpdateType.Normal;
    [HideInInspector]
    public AnimationCurve animationCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));
    [HideInInspector]
    public Ease easeType = Ease.Linear;
    [HideInInspector]
    public bool speedBase = false;
    [HideInInspector]
    public float duration = 1;
    [HideInInspector]
    public float delay = 0;
    [HideInInspector]
    public int tweenGroup = 0;
    [HideInInspector]
    public bool ignoreTimeScale = false;
    
	//[HideInInspector]
    //public List<EventDelegate> onFinish;


    [HideInInspector]
    public bool useCurve = true;
    protected Tweener tw;

	
	public UnityEvent onFinish = new UnityEvent();

    public bool isPlaying
    {
        get { return tw == null ? true : tw.IsPlaying(); }
    }

    public bool isComplete
    {
        get { return tw == null ? true : tw.IsComplete(); }
    }    

	// Use this for initialization
	public virtual void Start () {
        
	}

    public void Execute()
    {
//        foreach (EventDelegate del in onFinish)
//        {
//            del.Execute();
//        }
		onFinish.Invoke();

    }

    public void ReStart()
    {
        if (tw != null)
            tw.Restart();
    }

    public void PlayForward()
    {
        if (tw != null)
            tw.PlayForward();
    }

    public void PlayBackwards()
    {
        if (tw != null)
            tw.PlayBackwards();
    }

    public void Pause()
    {
        if (tw != null)
            tw.Pause();
    }

    public void Kill()
    {
        if (tw != null)
            tw.Kill();
    }
}

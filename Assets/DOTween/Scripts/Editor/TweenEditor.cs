
/*******************************************************************
* Summary: 
* Author : 
* Date   : 
*******************************************************************/
using UnityEngine;
using System.Collections;
using UnityEditor;
using DG.Tweening;

[CustomEditor (typeof(Tween), true)]
public class TweenEditor : Editor
{
	public override void OnInspectorGUI ()
	{
		GUILayout.Space (6f);
		Tools.SetLabelWidth (110f);
		base.OnInspectorGUI ();
		DrawCommonProperties ();
	}

	protected void DrawCommonProperties ()
	{
		Tween tw = target as Tween;
        
		if (Tools.DrawHeader ("Tweener")) {
			Tools.BeginContents (false);
			Tools.SetLabelWidth (110f);

			GUI.changed = false;

			LoopType type = (LoopType)EditorGUILayout.EnumPopup ("Loop Type", tw.loopType);
			UpdateType ut = (UpdateType)EditorGUILayout.EnumPopup ("Update Type", tw.updateType);
			GUILayout.BeginHorizontal ();
			int lpt = EditorGUILayout.IntField ("Loop Times", tw.loopTimes, GUILayout.Width (170f));
			GUILayout.Label ("times");
			GUILayout.EndHorizontal ();

			tw.useCurve = EditorGUILayout.Toggle ("Use Curve?", tw.useCurve, GUILayout.Width (170f));
			AnimationCurve curve;
			Ease et = Ease.Linear;
			if (tw.useCurve) {               
				curve = EditorGUILayout.CurveField ("Animation Curve", tw.animationCurve, GUILayout.Width (170f), GUILayout.Height (62f));
			} else {
				curve = tw.animationCurve;
				et = (Ease)EditorGUILayout.EnumPopup ("Ease Type", tw.easeType);
			}

			GUILayout.BeginHorizontal ();
			bool sb = EditorGUILayout.Toggle ("Speed Base", tw.speedBase);
			GUILayout.Label ("if true,duration val will ref to speed");
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			float dur = EditorGUILayout.FloatField ("Duration", tw.duration, GUILayout.Width (170f));
			GUILayout.Label ("seconds");
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			float del = EditorGUILayout.FloatField ("Start Delay", tw.delay, GUILayout.Width (170f));
			GUILayout.Label ("seconds");
			GUILayout.EndHorizontal ();

			int tg = EditorGUILayout.IntField ("Tween Group", tw.tweenGroup, GUILayout.Width (170f));
			bool it = EditorGUILayout.Toggle ("Ignore TimeScale", tw.ignoreTimeScale);

			if (GUI.changed) {
				Tools.RegisterUndo ("Tween Change", tw);
				tw.loopType = type;
				tw.loopTimes = lpt;
				tw.updateType = ut;
				tw.animationCurve = curve;
				tw.easeType = et;
				tw.ignoreTimeScale = it;
				tw.tweenGroup = tg;
				tw.duration = dur;
				tw.speedBase = sb;
				tw.delay = del;
				//Tools.SetDirty(tw);
			}
			Tools.EndContents ();
		}
		Tools.SetLabelWidth (80f);
//        Tools.DrawEvents("On Complete", tw, tw.onFinish, "");
	}
}

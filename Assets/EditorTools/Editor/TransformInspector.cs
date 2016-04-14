// Author:  Matthias Viranyi <matthias@viranyi.com>
// Project: 
// Date:    

using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Transform))]
public class TransformInspector : Editor
{
	#region Members

	private float _resetBtnWidth = 25;

	#endregion

	#region Unity Funcs

	public override void OnInspectorGUI()
	{
		Transform t = (Transform)target;

		// Replicate the standard transform inspector gui
		EditorGUIUtility.LookLikeControls();
		EditorGUI.indentLevel = 0;

		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("0",GUILayout.Width(_resetBtnWidth))) {
			t.localPosition = Vector3.zero;
		}
		Vector3 position = EditorGUILayout.Vector3Field("Position", t.localPosition);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("0",GUILayout.Width(_resetBtnWidth))) {
			t.localEulerAngles = Vector3.zero;
		}
		Vector3 eulerAngles = EditorGUILayout.Vector3Field("Rotation", t.localEulerAngles);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("1",GUILayout.Width(_resetBtnWidth))) {
			t.localScale = Vector3.one;
		}
		Vector3 scale = EditorGUILayout.Vector3Field("Scale", t.localScale);
		EditorGUILayout.EndHorizontal();
		EditorGUIUtility.LookLikeInspector();

		if (t.parent != null && GUILayout.Button("DeParent()")) {
			t.parent = null;
		}

		if (GUI.changed)
		{
			Undo.RegisterUndo(t, "Transform Change");

			t.localPosition = FixIfNaN(position);
			t.localEulerAngles = FixIfNaN(eulerAngles);
			t.localScale = FixIfNaN(scale);
		}
	}

	#endregion

	private Vector3 FixIfNaN(Vector3 v)
	{
		if (float.IsNaN(v.x))
		{
			v.x = 0;
		}
		if (float.IsNaN(v.y))
		{
			v.y = 0;
		}
		if (float.IsNaN(v.z))
		{
			v.z = 0;
		}
		return v;
	}
}
/*
 * ToolsMenu.cs
 * #PROJECTNAME#
 * Created by #SMARTDEVELOPERS# on #CREATEONDATE#.
 */

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using UnityEngine.UI;
using hpjFonts;

public class ToolsMenu : Editor
{
//	[MenuItem( "Window/EditorTools/Font Editor" )]
//	static void FontMaker () {
//		FontEditor window = (FontEditor) EditorWindow.GetWindow( typeof( FontEditor ) );
//		window.Show();
//		window.position = new Rect( 20, 80, 770, 550 );
//		window.titleContent = new GUIContent( "Font Editor" );
//	}
//



	[MenuItem("GameObject/RenameWith...",false,13)]
	static void RenameWith()
	{
		if (Selection.activeTransform == null)
			return;

		Debug.Log (Selection.activeTransform.name);
		//Rename (Selection.activeTransform, "Image", "JetPos");
	}

	static void Rename(Transform t,string oldName,string newName)
	{
		if (t.name == oldName)
			t.name = newName;
		if (t.childCount == 0)
			return;
		for (int i = 0; i < t.childCount; i++) {
			Transform tc = t.GetChild (i);
			Rename (tc, oldName, newName);
		}
	}
}
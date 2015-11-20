/*
 * GenUIEditor.cs
 * 赛马2
 * Created by sinodata on 10/19/2015 09:54:13.
 */

using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

//[CustomEditor(typeof(RectTransform))]
public class GenUIEditor : Editor 
{
	[SerializeField]
	RectTransform mTrans;

	string filepath = "";
	StringBuilder content = new StringBuilder();

	void OnSceneGUI()
	{
		mTrans = target as RectTransform;

		Handles.BeginGUI();
		if(mTrans.name == "Canvas")
		{
			if(GUILayout.Button("CheckNames", GUILayout.Width(150)))
			{
				CheckNames(mTrans);
				Debug.Log("Check Ok!");
			}
			if(GUILayout.Button("GenUI", GUILayout.Width(150)))
			{
				nameList = new List<string>();
				//Debug.Log(Selection.activeTransform);
				string sn = EditorApplication.currentScene;
				int index = sn.LastIndexOf('/');

				sn = sn.Replace(".unity","");
				filepath = Application.dataPath + "/UIConfigs/" +sn.Substring(index+1)+"_"+ mTrans.name + ".txt";

				GenUIRes(mTrans);
				Debug.Log("content=" + content);

				FileStream fs = new FileStream(filepath, FileMode.Create);
				StreamWriter sw = new StreamWriter(fs);
				sw.Write(content);
				sw.Flush();
				sw.Close();
				fs.Close();
				Debug.Log(mTrans.name + " Generate Ok! @"+filepath);
			}
		}
		Handles.EndGUI();
		if (GUI.changed)
			EditorUtility.SetDirty(target);
	}

	List<string> nameList = new List<string>();

	private void CheckNames(Transform t)
	{
		if (t.childCount == 0)
		{
			return;
		}
		for (int i = 0; i < t.childCount; i++)
		{
			Transform tc = t.GetChild(i);
			if(nameList.Contains(tc.name))
			{
				tc.name = t.name + "_" + tc.name + "_" +i.ToString("000");
			}
			nameList.Add(tc.name);
			CheckNames(tc);
		}
	}

	private void GenUIRes(Transform t)
	{
		if (t.childCount == 0)
		{
			return;
		}

		for (int i = 0; i < t.childCount; i++)
		{
			Transform tc = t.GetChild(i);
			Image im = tc.GetComponent<Image>();
			if(im != null)
			{
				if(im.sprite != null)
					content.Append(tc.name + "|" + im.sprite.name + "\n");
			}
			GenUIRes(tc);
		}
	}
}

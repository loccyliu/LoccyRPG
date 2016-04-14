/*
 * AutoCreate.cs
 * Rpg
 * Created by com.sinodata on 01/05/2016 13:49:07.
 */
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class AutoCreate : Editor {

	[MenuItem("Assets/Create/UIView")]
	public static void CreateUIView()
	{
		ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<EndNameEditOperation>(),
			GetCurPath() + "/NewUIView.cs",
			null,
			"Assets/EditorTools/Templates/New UIView Template.txt");
		Debug.Log ("gen ok! in >> " +GetCurPath());
	}

	[MenuItem("Assets/Create/UIClass")]
	public static void CreateUIClass()
	{
		ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<EndNameEditOperation>(),
			GetCurPath() + "/NewUIClass.cs",
			null,
			"Assets/EditorTools/Templates/New UIClass Template.txt");
		Debug.Log ("gen ok! in >> " +GetCurPath());
	}

	[MenuItem("Assets/Create/UIDialogView")]
	public static void CreateUIDialogView()
	{
		ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<EndNameEditOperation>(),
			GetCurPath() + "/NewUIDialogView.cs",
			null,
			"Assets/EditorTools/Templates/New UIDialogView Template.txt");
		Debug.Log ("gen ok! in >> " +GetCurPath());
	}

	[MenuItem("Assets/Create/UIDialogClass")]
	public static void CreateUIDialogClass()
	{
		ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<EndNameEditOperation>(),
			GetCurPath() + "/NewUIDialogClass.cs",
			null,
			"Assets/EditorTools/Templates/New UIDialogClass Template.txt");
		Debug.Log ("gen ok! in >> " +GetCurPath());
	}

	static string GetCurPath()
	{
		string path = "Aseets";
		foreach (Object obj in Selection.GetFiltered(typeof(Object),SelectionMode.Assets)) {
			path = AssetDatabase.GetAssetPath(obj);
			if (!string.IsNullOrEmpty(path) && File.Exists(path)) {
				path = Path.GetDirectoryName(path);
				break;
			}
		}
		return path;
	}
}
class EndNameEditOperation : UnityEditor.ProjectWindowCallback.EndNameEditAction
{
	public override void Action(int instanceId, string pathName, string resourceFile)
	{
		Object obj = CreateFromTemplate(pathName, resourceFile);
		ProjectWindowUtil.ShowCreatedAsset(obj);
	}

	internal static Object CreateFromTemplate(string pathName,string sourceName){
		string dPath = Path.GetFullPath(pathName);
		StreamReader sr = new StreamReader(sourceName);
		string scripts = sr.ReadToEnd();
		sr.Close();
		string fileName = Path.GetFileNameWithoutExtension(pathName);
		string fileExtension = Path.GetExtension(pathName);
		scripts = System.Text.RegularExpressions.Regex.Replace(scripts, "#SCRIPTNAME#", fileName);
		scripts = System.Text.RegularExpressions.Regex.Replace(scripts,"#CREATEONDATE#", System.DateTime.Now.ToString("d")+" "+System.DateTime.Now.ToString("HH:mm:ss"));
		scripts = System.Text.RegularExpressions.Regex.Replace(scripts,"#PROJECTNAME#", PlayerSettings.productName);
		scripts = System.Text.RegularExpressions.Regex.Replace(scripts,"#SMARTDEVELOPERS#", PlayerSettings.companyName);
		scripts = System.Text.RegularExpressions.Regex.Replace(scripts,"#FILEEXTENSION#", fileExtension);

		System.Text.UTF8Encoding encode = new System.Text.UTF8Encoding(true, false);
		StreamWriter sw = new StreamWriter(dPath, false, encode);
		sw.Write(scripts);
		sw.Close();
		AssetDatabase.ImportAsset(pathName);
		return AssetDatabase.LoadAssetAtPath(pathName, typeof(Object));
	}
}
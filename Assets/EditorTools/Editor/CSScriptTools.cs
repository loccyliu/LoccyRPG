
using UnityEditor;
using System.Collections;
using UnityEngine;

public class CSScriptTools : UnityEditor.AssetModificationProcessor 
{
	public static void OnWillCreateAsset ( string path ) 
	{
		path = path.Replace(".meta", "");

		int index = path.LastIndexOf(".");
		if (index <= 0)
			return;
		string file = path.Substring(index);
		if (file != ".cs" && file != ".js" && file != ".boo") 
			return;
		string fileExtension = file;
		
		index = Application.dataPath.LastIndexOf("Assets");
		path = Application.dataPath.Substring(0, index) + path;
		file = System.IO.File.ReadAllText(path);
		
		file = file.Replace("#CREATEIONDATE#", System.DateTime.Now.ToString("d")+" "+System.DateTime.Now.ToString("HH:mm:ss"));
		file = file.Replace("#PROJECTNAME#", PlayerSettings.productName);
		file = file.Replace("#SMARTDEVELOPERS#", PlayerSettings.companyName);
		file = file.Replace("#FILEEXTENSION#", fileExtension);
		
		System.IO.File.WriteAllText(path, file);
		AssetDatabase.Refresh();
	}
}

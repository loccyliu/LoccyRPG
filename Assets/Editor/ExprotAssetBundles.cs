using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Text;

public class ExprotAssetBundles :Editor
{
	static string outputPath = "Assets/StreamingAssets";

	[MenuItem("Assets/BuildAsb")]
	public static void Build()
	{
		BuildPipeline.BuildAssetBundles(outputPath);//
		AssetDatabase.Refresh();
		Log.i ("Build ok! >> "  + outputPath);
	}

	[MenuItem("Assets/GenFilesTxt")]
	public static void GenFilesTxt()
	{
		string[] asbs = AssetDatabase.GetAllAssetBundleNames();

		string lineText = "StreamingAssets\nStreamingAssets.manifest\n";
		for (int i = 0; i < asbs.Length; ++i)
		{
			if(asbs[i].EndsWith(".unity3d"))
				lineText += asbs[i] + ".manifest";
			else
				lineText += asbs[i] + ".unity3d";
			lineText += "\n";
		}

		string path = Application.dataPath + "/StreamingAssets/files.txt";

		FileStream fs = new FileStream(path, FileMode.Create);
		StreamWriter sw = new StreamWriter(fs);
		sw.Write(lineText);
		sw.Flush();
		sw.Close();
		fs.Close();
		AssetDatabase.Refresh();
		Log.i ("gen ok! >> " + path);
	}

	[MenuItem("Assets/GenVersionTxt")]
	public static void GenVersionTxt()
	{
		string[] asbs = AssetDatabase.GetAllAssetBundleNames();

		string verText = "";
		for (int i = 0; i < asbs.Length; ++i)
		{
			string id = AssetDatabase.AssetPathToGUID(outputPath +"/" + asbs[i]);
			Debug.Log(asbs[i] + "==" + id);
			verText += asbs[i] + "|" + id;
			verText += "\n";
		}

		string path = Application.dataPath + "/StreamingAssets/version.txt";

		FileStream fs = new FileStream(path, FileMode.Create);
		StreamWriter sw = new StreamWriter(fs);
		sw.Write(verText);
		sw.Flush();
		sw.Close();
		fs.Close();
		AssetDatabase.Refresh();
		Log.i ("gen ok! >> " + path);
	}
}

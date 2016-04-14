using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Text;

public class ExprotAssetBundles :Editor
{
	static string outputPath = "Assets/StreamingAssets";

	[MenuItem("Assets/BuildAsb/Android")]
	public static void BuildAndroid()
	{
		CheckStreamingAssetsFold ();
		BuildPipeline.BuildAssetBundles(outputPath,BuildAssetBundleOptions.ForceRebuildAssetBundle,BuildTarget.Android);//
		AssetDatabase.Refresh();
		Debug.Log ("Build Android asb ok! >> "  + outputPath);
	}

	[MenuItem("Assets/BuildAsb/IOS")]
	public static void BuildIOS()
	{
		CheckStreamingAssetsFold ();
		BuildPipeline.BuildAssetBundles(outputPath,BuildAssetBundleOptions.ForceRebuildAssetBundle,BuildTarget.iOS);//
		AssetDatabase.Refresh();
		Debug.Log ("Build iOS asb ok! >> "  + outputPath);
	}

	[MenuItem("Assets/GenFilesTxt")]
	public static void GenFilesTxt()
	{
		CheckStreamingAssetsFold ();
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
		File.WriteAllText (path, lineText);


		AssetDatabase.Refresh();
		Debug.Log ("gen ok! >> " + path);
	}

	[MenuItem("Assets/GenVersionTxt")]
	public static void GenVersionTxt()
	{
		CheckStreamingAssetsFold ();
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
		File.WriteAllText (path, verText);
		AssetDatabase.Refresh();
		Debug.Log ("gen ok! >> " + path);
	}

	[MenuItem("Assets/BuildApk")]
	public static void BuildApk()
	{
		EditorBuildSettingsScene[] levels = EditorBuildSettings.scenes;
		string[] ls = new string[levels.Length];
		for (int i = 0; i < levels.Length; ++i) {
			ls [i] = levels [i].path;
		}
		BuildPipeline.BuildPlayer (ls, Application.dataPath + "/../" + Application.productName + ".apk", BuildTarget.Android, BuildOptions.None);
		System.Diagnostics.Process.Start(Application.dataPath + "/../");
	}

	[MenuItem("Assets/BuildXcode")]
	public static void BuildXcode()
	{
		EditorBuildSettingsScene[] levels = EditorBuildSettings.scenes;
		string[] ls = new string[levels.Length];
		for (int i = 0; i < levels.Length; ++i) {
			ls [i] = levels [i].path;
		}
		BuildPipeline.BuildPlayer (ls, Application.dataPath + "/../"+ Application.productName + "_iOS", BuildTarget.iOS, BuildOptions.None);
		System.Diagnostics.Process.Start(Application.dataPath + "/../");
	}

	static void CheckStreamingAssetsFold()
	{
		string p = Application.dataPath + "/StreamingAssets";
		if (!Directory.Exists (p)) {
			Directory.CreateDirectory (p);
		}
	}
}

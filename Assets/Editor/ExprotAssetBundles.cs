/*
 * ExprotAssetBundles.cs
 * RpgFramework
 * Created by com.loccy on 10/28/2015 15:08:46.
 */
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
		BuildPipeline.BuildAssetBundles(outputPath,BuildAssetBundleOptions.ForceRebuildAssetBundle,BuildTarget.Android);//
		AssetDatabase.Refresh();
		Log.i ("Build Android ok! >> "  + outputPath);
	}

	[MenuItem("Assets/BuildAsb/IOS")]
	public static void BuildIOS()
	{
		BuildPipeline.BuildAssetBundles(outputPath,BuildAssetBundleOptions.ForceRebuildAssetBundle,BuildTarget.iOS);//
		AssetDatabase.Refresh();
		Log.i ("Build iOS ok! >> "  + outputPath);
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
		File.WriteAllText (path, lineText);


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
		File.WriteAllText (path, verText);
		AssetDatabase.Refresh();
		Log.i ("gen ok! >> " + path);
	}

	[MenuItem("Assets/BuildApk")]
	public static void BuildApk()
	{
		EditorBuildSettingsScene[] levels = EditorBuildSettings.scenes;
		string[] ls = new string[levels.Length];
		for (int i = 0; i < levels.Length; ++i) {

			ls [i] = levels [i].path;
		}
		BuildPipeline.BuildPlayer (ls, Application.dataPath + "/../game.apk", BuildTarget.Android, BuildOptions.None);
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
		BuildPipeline.BuildPlayer (ls, Application.dataPath + "/../game_iOS", BuildTarget.iOS, BuildOptions.None);
		System.Diagnostics.Process.Start(Application.dataPath + "/../");
	}
}

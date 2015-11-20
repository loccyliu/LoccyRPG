/*
 * RescourceManager.cs
 * RpgFramework
 * Created by Loocy on 10/16/2015 09:58:41.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void LoadAssetCallback(object para);

public class ResourceManager_WWW : MonoBehaviour
{
	string CDN = "";

	string assetBundlesPath = "AssetBundle";
	string asb_ex = ".unity3d";
	int ver = 0;

	WWW manifest_www = null;
	AssetBundle manifestAsb = null;

	WWW cur_www = null;

	Dictionary<string,AssetBundle> allBundles = new Dictionary<string, AssetBundle>();
	Dictionary<string,List<AssetBundle>> bundleDependences = new Dictionary<string, List<AssetBundle>>();

	public UnityEngine.UI.Slider slider;
	public static ResourceManager_WWW instance;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		//LoadAll();
	}

	/*======================================================================================*/

	IEnumerator LoadAssetBundleManifest()
	{
		CDN =  "file://" + Application.dataPath + "/AssetBundle/";
		Log.i("CDN=" + CDN);
		if(manifestAsb == null)
		{
			string url = CDN + assetBundlesPath;
			manifest_www = WWW.LoadFromCacheOrDownload(url, ver);
			cur_www = manifest_www;

			yield return manifest_www;

			if(!string.IsNullOrEmpty(manifest_www.error))
			{
				Log.e("error:" + assetBundlesPath + " -- " + manifest_www.error);
			}
			else
			{
				manifestAsb = manifest_www.assetBundle;
			}
		}
		yield return null;
	}

	bool IsLoadingManifest()
	{
		if(manifest_www != null && !manifest_www.isDone)
		{
			return true;
		}
		return false;
	}

	/// <summary>
	/// Loads all object.
	/// manifest记录了所有被打包的文件以及其所依赖的
	/// </summary>
	/// <returns>The all object.</returns>
	IEnumerator LoadAllObjAsync()
	{
		manifestAsb = null;
		yield return StartCoroutine(LoadAssetBundleManifest());

		if(manifestAsb == null)
		{
			Log.e("error:manifestAsb == null");
			yield return 0;
		}
		else
		{
			Object maniObj = manifestAsb.LoadAsset("AssetBundleManifest");
			AssetBundleManifest asbManifest = (AssetBundleManifest)maniObj;
			Object[] ss = manifestAsb.LoadAllAssets();
			//Log.i("===================>" + ss[0]);

			string[] childs = asbManifest.GetAllAssetBundles();
			for (int j = 0; j < childs.Length; ++j)
			{
				Log.i(">>>" + childs[j]);
				string[] dps = asbManifest.GetAllDependencies(childs[j]);
				bundleDependences[childs[j]] = new List<AssetBundle>();

				for (int i = 0; i < dps.Length; ++i)
				{
					Log.i("dps:" + dps[i]);
					WWW dpswww = WWW.LoadFromCacheOrDownload(CDN + dps[i], ver);
					cur_www = dpswww;

					yield return dpswww;

					if(!string.IsNullOrEmpty(dpswww.error))
					{
						Log.e("dps error:" + dps[i] + " -- " + dpswww.error);
					}
					else
					{
						AssetBundle dpsAsb = dpswww.assetBundle;

						bundleDependences[childs[j]].Add(dpsAsb); 
					}
				}

				WWW resourceWww = WWW.LoadFromCacheOrDownload(CDN + childs[j], ver);
				cur_www = resourceWww;

				yield return resourceWww;

				if(!string.IsNullOrEmpty(resourceWww.error))
				{
					Log.e("resource error:" + childs[j] + " -- " + resourceWww.error);
				}
				else
				{
					AssetBundle resourceAsb = resourceWww.assetBundle;
					allBundles[childs[j]] = resourceAsb;
				}
			}

			yield return new WaitForEndOfFrame();
			LoadGameScene();
		}
	}

	/// <summary>
	/// Loads the object.
	/// </summary>
	/// <returns>The object.</returns>
	/// <param name="resourceName">Resource name with extention name</param>
	IEnumerator LoadObjAsync(string asbName, string resourceName, LoadAssetCallback callback)
	{
		if(!asbName.EndsWith(asb_ex))
			asbName += asb_ex;
		if(allBundles.ContainsKey(asbName))
		{
			Object obj = allBundles[asbName].LoadAsset(resourceName);
			if(callback != null)
				callback(obj);
			yield return 0;
		}

		yield return StartCoroutine(LoadAssetBundleManifest());

		CDN =  "file://" + Application.dataPath + "/AssetBundles/";
		if(manifestAsb == null)
		{
			Log.e("asb is null");
			yield return null;
		}
		Object maniObj = manifestAsb.LoadAsset("assetbundlemanifest");
		AssetBundleManifest asbManifest = (AssetBundleManifest)maniObj;

		//获取目标的依赖信息
		string[] dps = asbManifest.GetAllDependencies(asbName + asb_ex);
		Log.i("len=" + dps.Length);

		//加载依赖资源
		for (int i = 0; i < dps.Length; ++i)
		{
			Log.i(string.Format("dps[{0}]={1}", i, dps[i]));
			WWW dpswww = WWW.LoadFromCacheOrDownload(CDN + dps[i], ver);

			yield return dpswww;

			if(!string.IsNullOrEmpty(dpswww.error))
			{
				Log.e("dps error:" + dps[i] + " -- " + dpswww.error);
			}
			else
			{
				AssetBundle dpsAsb = dpswww.assetBundle;
				Object[] dpsObj = dpsAsb.LoadAllAssets();

				dpsAsb.Unload(false);
			}
		}

		//加载目标资源
		WWW resourceWww = WWW.LoadFromCacheOrDownload(CDN + asbName + asb_ex, ver);
		yield return resourceWww;

		if(!string.IsNullOrEmpty(resourceWww.error))
		{
			Log.e("resource error:" + resourceName + " -- " + resourceWww.error);
		}
		else
		{
			AssetBundle resourceAsb = resourceWww.assetBundle;
			Object resourceObj = resourceAsb.LoadAsset(resourceName);
			resourceAsb.Unload(false);
			Log.i("Load..." + resourceObj.name);

			if(callback != null)
				callback(resourceObj);
			//AssetBundle.Instantiate(resourceObj);
		}
	}


	public void LoadAll()
	{
		StartCoroutine(LoadAllObjAsync());
	}

	public void LoadObj(string asbname, string resourceName, LoadAssetCallback callback)
	{
		StartCoroutine(LoadObjAsync(asbname, resourceName, callback));
	}

	/********************************需要先加载所有资源****************************************/
	public T LoadAsset<T>(string asbname,string objname) where T : Object
	{
		if(!asbname.EndsWith(asb_ex))
			asbname += asb_ex;
		Log.i("===>"+objname);
		if(allBundles.ContainsKey(asbname))
		{
			
			T temp = allBundles[asbname].LoadAsset(objname, typeof(T)) as T;
			Log.i("exist "+temp.name);
			return temp;
		}
		return null;
	}

	public T LoadAsset<T>(string asbname,string objname, LoadAssetCallback callback) where T : Object
	{
		if(!asbname.EndsWith(asb_ex))
			asbname += asb_ex;
		if(allBundles.ContainsKey(asbname))
		{
			T temp = allBundles[asbname].LoadAsset(objname) as T;
			return temp;
		}
		return null;
	}
	/*========================================================================================*/

	void LoadGameScene()
	{
		Application.LoadLevel("TestUI");
	}

	void Update()
	{
		if(cur_www != null && slider != null)
		{
			slider.value = cur_www.progress;
		}
	}
}
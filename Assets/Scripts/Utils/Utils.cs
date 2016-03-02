/*
 * Utils.cs
 * RpgFramework
 * Created by com.loccy on 11/19/2015 14:34:34.
 */

using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using ICSharpCode.SharpZipLib.GZip;
using System.Runtime.Serialization.Formatters.Binary;

public class Util : MonoBehaviour
{
	public static int Int (object o)
	{
		return Convert.ToInt32 (o);
	}

	public static float Float (object o)
	{
		return (float)Math.Round (Convert.ToSingle (o), 2);
	}

	public static long Long (object o)
	{
		return Convert.ToInt64 (o);
	}

	public static int Random (int min, int max)
	{
		return UnityEngine.Random.Range (min, max);
	}

	public static float Random (float min, float max)
	{
		return UnityEngine.Random.Range (min, max);
	}

	public static string Uid (string uid)
	{
		int position = uid.LastIndexOf ('_');
		return uid.Remove (0, position + 1);
	}

	public static long GetTime ()
	{
		TimeSpan ts = new TimeSpan (DateTime.UtcNow.Ticks - new DateTime (1970, 1, 1, 0, 0, 0).Ticks);
		return (long)ts.TotalMilliseconds;
	}

	/// <summary>
	/// 搜索子物体组件-GameObject版
	/// </summary>
	public static T Get<T> (GameObject go, string subnode) where T : Component
	{
		if (go != null) {
			return Get<T>(go.transform, subnode);
		}
		return null;
	}

	/// <summary>
	/// 搜索子物体组件-Transform版
	/// </summary>
	public static T Get<T> (Transform go, string subnode) where T : Component
	{
		if (go != null) {
			Transform sub = go.FindChild (subnode);
			if (sub != null)
				return sub.GetComponent<T> ();
		}
		return null;
	}

	/// <summary>
	/// 搜索子物体组件-Component版
	/// </summary>
	public static T Get<T> (Component go, string subnode) where T : Component
	{
		return Get<T>(go.transform, subnode);
	}

	/// <summary>
	/// 添加组件
	/// </summary>
	public static T Add<T> (GameObject go) where T : Component
	{
		if (go != null) {
			T[] ts = go.GetComponents<T> ();
			for (int i = 0; i < ts.Length; i++) {
				if (ts [i] != null)
					Destroy (ts [i]);
			}
			return go.AddComponent<T> ();
		}
		return null;
	}

	/// <summary>
	/// 添加组件
	/// </summary>
	public static T Add<T> (Transform go) where T : Component
	{
		return Add<T> (go.gameObject);
	}

	/// <summary>
	/// 查找子对象
	/// </summary>
	public static GameObject Child (GameObject go, string subnode)
	{
		return Child (go.transform, subnode);
	}

	/// <summary>
	/// 查找子对象
	/// </summary>
	public static GameObject Child (Transform go, string subnode)
	{
		Transform tran = go.FindChild (subnode);
		if (tran == null)
			return null;
		return tran.gameObject;
	}

	/// <summary>
	/// 取平级对象
	/// </summary>
	public static GameObject Peer (GameObject go, string subnode)
	{
		return Peer (go.transform, subnode);
	}

	/// <summary>
	/// 取平级对象
	/// </summary>
	public static GameObject Peer (Transform go, string subnode)
	{
		Transform tran = go.parent.FindChild (subnode);
		if (tran == null)
			return null;
		return tran.gameObject;
	}
	/// <summary>
	/// Sets the active.
	/// </summary>
	/// <param name="go">Go.</param>
	/// <param name="st">If set to <c>true</c> st.</param>
	public static void SetActive(GameObject go,bool st)
	{
		if (go == null)
			return;
		go.SetActive (st);
	}

	/// <summary>
	/// Sets the active.
	/// </summary>
	/// <param name="tr">Tr.</param>
	/// <param name="st">If set to <c>true</c> st.</param>
	public static void SetActive(Transform tr,bool st)
	{
		if (tr == null || tr.gameObject == null)
			return;
		tr.gameObject.SetActive (st);
	}
	/// <summary>
	/// Sets the active.
	/// </summary>
	/// <param name="co">Co.</param>
	/// <param name="st">If set to <c>true</c> st.</param>
	public static void SetActive(Component co,bool st)
	{
		if (co == null || co.gameObject == null)
			return;
		co.gameObject.SetActive (st);
	}

	public static void SetActive(GameObject go,string sub,bool st)
	{
		if (go == null)
			return;
		Child(go, sub).SetActive(st);
	}

	static public T FindInParents<T>(GameObject go) where T : Component
	{
		return FindInParents<T>(go.transform);
	}

	static public T FindInParents<T>(Transform go) where T : Component
	{
		if (go == null) return null;
		var comp = go.GetComponent<T>();

		if (comp != null)
			return comp;

		Transform t = go.parent;
		while (t != null && comp == null)
		{
			comp = t.gameObject.GetComponent<T>();
			t = t.parent;
		}
		return comp;
	}

	public static bool DelChild(Transform t,string name)
	{
		GameObject go = t.FindChild (name).gameObject;
		if (go != null) {
			Destroy (go);
			return true;
		}
		return false;
	}

	/// <summary>
	/// 手机震动
	/// </summary>
	public static void Vibrate ()
	{
		//int canVibrate = PlayerPrefs.GetInt(Const.AppPrefix + "Vibrate", 1);
		//if (canVibrate == 1) iPhoneUtils.Vibrate();
	}

	/// <summary>
	/// Base64编码
	/// </summary>
	public static string Encode (string message)
	{
		byte[] bytes = Encoding.GetEncoding ("utf-8").GetBytes (message);
		return Convert.ToBase64String (bytes);
	}

	/// <summary>
	/// Base64解码
	/// </summary>
	public static string Decode (string message)
	{
		byte[] bytes = Convert.FromBase64String (message);
		return Encoding.GetEncoding ("utf-8").GetString (bytes);
	}

	/// <summary>
	/// 判断数字
	/// </summary>
	public static bool IsNumeric (string str)
	{
		if (str == null || str.Length == 0)
			return false;
		for (int i = 0; i < str.Length; i++) {
			if (!Char.IsNumber (str [i])) {
				return false;
			}
		}
		return true;
	}

	/// <summary>
	/// HashToMD5Hex
	/// </summary>
	public static string HashToMD5Hex (string sourceStr)
	{
		byte[] Bytes = Encoding.UTF8.GetBytes (sourceStr);
		using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider ()) {
			byte[] result = md5.ComputeHash (Bytes);
			StringBuilder builder = new StringBuilder ();
			for (int i = 0; i < result.Length; i++)
				builder.Append (result [i].ToString ("x2"));
			return builder.ToString ();
		}
	}

	/// <summary>
	/// 计算字符串的MD5值
	/// </summary>
	public static string MD5 (string source)
	{
		MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider ();
		byte[] data = System.Text.Encoding.UTF8.GetBytes (source);
		byte[] md5Data = md5.ComputeHash (data, 0, data.Length);
		md5.Clear ();

		string destString = "";
		for (int i = 0; i < md5Data.Length; i++) {
			destString += System.Convert.ToString (md5Data [i], 16).PadLeft (2, '0');
		}
		destString = destString.PadLeft (32, '0');
		return destString;
	}

	/// <summary>
	/// 计算文件的MD5值
	/// </summary>
	public static string md5file (string file)
	{
		try {
			FileStream fs = new FileStream (file, FileMode.Open);
			System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider ();
			byte[] retVal = md5.ComputeHash (fs);
			fs.Close ();

			StringBuilder sb = new StringBuilder ();
			for (int i = 0; i < retVal.Length; i++) {
				sb.Append (retVal [i].ToString ("x2"));
			}
			return sb.ToString ();
		} catch (Exception ex) {
			throw new Exception ("md5file() fail, error:" + ex.Message);
		}
	}

	/// <summary>
	/// 功能：压缩字符串
	/// </summary>
	/// <param name="infile">被压缩的文件路径</param>
	/// <param name="outfile">生成压缩文件的路径</param>
	public static void CompressFile (string infile, string outfile)
	{
		Stream gs = new GZipOutputStream (File.Create (outfile));
		FileStream fs = File.OpenRead (infile);
		byte[] writeData = new byte[fs.Length];
		fs.Read (writeData, 0, (int)fs.Length);
		gs.Write (writeData, 0, writeData.Length);
		gs.Close ();
		fs.Close ();
	}

	/// <summary>
	/// 功能：输入文件路径，返回解压后的字符串
	/// </summary>
	public static string DecompressFile (string infile)
	{
		string result = string.Empty;
		Stream gs = new GZipInputStream (File.OpenRead (infile));
		MemoryStream ms = new MemoryStream ();
		int size = 2048;
		byte[] writeData = new byte[size];
		while (true) {
			size = gs.Read (writeData, 0, size);
			if (size > 0) {
				ms.Write (writeData, 0, size);
			} else {
				break;
			}
		}
		result = new UTF8Encoding ().GetString (ms.ToArray ());
		gs.Close ();
		ms.Close ();
		return result;
	}

	/// <summary>
	/// 压缩字符串
	/// </summary>
	public static string Compress (string source)
	{
		byte[] data = Encoding.UTF8.GetBytes (source);
		MemoryStream ms = null;
		using (ms = new MemoryStream ()) {
			using (Stream stream = new GZipOutputStream (ms)) {
				try {
					stream.Write (data, 0, data.Length);
				} finally {
					stream.Close ();
					ms.Close ();
				}
			}
		}
		return Convert.ToBase64String (ms.ToArray ());
	}

	/// <summary>
	/// 解压字符串
	/// </summary>
	public static string Decompress (string source)
	{
		string result = string.Empty;
		byte[] buffer = null;
		try {
			buffer = Convert.FromBase64String (source);
		} catch {
			Log.e("Decompress---->>>>" + source);
		}
		using (MemoryStream ms = new MemoryStream (buffer)) {
			using (Stream sm = new GZipInputStream (ms)) {
				StreamReader reader = new StreamReader (sm, Encoding.UTF8);
				try {
					result = reader.ReadToEnd ();
				} finally {
					sm.Close ();
					ms.Close ();
				}
			}
		}
		return result;
	}

	/// <summary>
	/// 清除所有子节点
	/// </summary>
	public static void ClearChild (Transform go)
	{
		if (go == null)
			return;
		for (int i = go.childCount - 1; i >= 0; i--) {
			Destroy (go.GetChild (i).gameObject);
		}
	}

	/// <summary>
	/// 生成一个Key名
	/// </summary>
	public static string GetKey (string key)
	{
		return Const.AppPrefix + Const.UserId + "_" + key;
	}

	/// <summary>
	/// 取得整型
	/// </summary>
	public static int GetInt (string key)
	{
		string name = GetKey (key);
		return PlayerPrefs.GetInt (name);
	}

	/// <summary>
	/// 有没有值
	/// </summary>
	public static bool HasKey (string key)
	{
		string name = GetKey (key);
		return PlayerPrefs.HasKey (name);
	}

	/// <summary>
	/// 保存整型
	/// </summary>
	public static void SetInt (string key, int value)
	{
		string name = GetKey (key);
		PlayerPrefs.DeleteKey (name);
		PlayerPrefs.SetInt (name, value);
	}

	/// <summary>
	/// 取得数据
	/// </summary>
	public static string GetString (string key)
	{
		string name = GetKey (key);
		return PlayerPrefs.GetString (name);
	}

	/// <summary>
	/// 保存数据
	/// </summary>
	public static void SetString (string key, string value)
	{
		string name = GetKey (key);
		PlayerPrefs.DeleteKey (name);
		PlayerPrefs.SetString (name, value);
	}

	/// <summary>
	/// 删除数据
	/// </summary>
	public static void RemoveData (string key)
	{
		string name = GetKey (key);
		PlayerPrefs.DeleteKey (name);
	}

	/// <summary>
	/// 清理内存
	/// </summary>
	public static void ClearMemory ()
	{
		GC.Collect ();
		Resources.UnloadUnusedAssets ();
	}

	/// <summary>
	/// 是否为数字
	/// </summary>
	public static bool IsNumber (string strNumber)
	{
		Regex regex = new Regex ("[^0-9]");
		return !regex.IsMatch (strNumber);
	}

	/// <summary>
	/// 取得数据存放目录
	/// </summary>
	public static string DataPath {
		get {
			string game = Const.AppName.ToLower ();
			if (Application.isMobilePlatform) {
				return Application.persistentDataPath + "/" + game + "/";
			}
			if (Const.DebugMode) {
				return Application.dataPath + "/" + Const.AssetDirname + "/";
			}
			return Application.dataPath + "/SourceFiles/";
		}
	}

	public static string GetRelativePath ()
	{
		if (Application.isEditor)
			return "file://" + System.Environment.CurrentDirectory.Replace ("\\", "/") + "/Assets/" + Const.AssetDirname + "/";
		else if (Application.isMobilePlatform || Application.isConsolePlatform)
			return "file:///" + DataPath;
		else // For standalone player.
			return "file://" + Application.streamingAssetsPath + "/";
	}

	/// <summary>
	/// 取得行文本
	/// </summary>
	public static string GetFileText (string path)
	{
		return File.ReadAllText (path);
	}

	/// <summary>
	/// 网络可用
	/// </summary>
	public static bool NetAvailable {
		get {
			return Application.internetReachability != NetworkReachability.NotReachable;
		}
	}

	/// <summary>
	/// 是否是无线
	/// </summary>
	public static bool IsWifi {
		get {
			return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
		}
	}

	/// <summary>
	/// 图片与byte[]互转
	/// </summary>
	/// <param name="pic">Pic.</param>
	public static Texture2D convertPNG(Texture2D pic)
	{
		byte[] data = pic.EncodeToPNG();
		//Debug.Log("data = " + data.Length + "|" + data[0]);
		Texture2D mConvertPNG = new Texture2D(200, 200);
		mConvertPNG.LoadImage(data);

		return mConvertPNG;
	}

	/// <summary>
	/// byte[]与base64互转
	/// </summary>
	/// <returns>The to pic.</returns>
	/// <param name="base64">Base64.</param>
	public static Texture2D ByteToPic(string base64)
	{ 
		Texture2D pic = new Texture2D(200, 200);
		//将base64转码为byte[] 
		byte[] data = System.Convert.FromBase64String(base64);
		//加载byte[]图片
		pic.LoadImage(data);

		string base64str = System.Convert.ToBase64String(data);

		return pic;
	}

	/// <summary>
	/// 应用程序内容路径
	/// </summary>
	public static string AppContentPath ()
	{
		string path = string.Empty;
		switch (Application.platform) {
		case RuntimePlatform.Android:
			path = "jar:file://" + Application.dataPath + "!/assets/";
			break;
		case RuntimePlatform.IPhonePlayer:
			path = Application.dataPath + "/Raw/";
			break;
		default:
			path = Application.dataPath + "/" + Const.AssetDirname + "/";
			break;
		}
		return path;
	}

	/// <summary>
	/// 是否是登录场景
	/// </summary>
	public static bool isLogin {
		get { return Application.loadedLevelName.ToLower().CompareTo ("login") == 0; }
	}

	/// <summary>
	/// 是否是城镇场景
	/// </summary>
	public static bool isMain {
		get { return Application.loadedLevelName.ToLower().CompareTo ("main") == 0; }
	}

	/// <summary>
	/// 判断是否是战斗场景
	/// </summary>
	public static bool isFight {
		get { return Application.loadedLevelName.ToLower().CompareTo ("fight") == 0; }
	}
}

public static class ExtendClass
{
	public static List<T> AddList<T>(this List<T> list, List<T> list2)
	{
		int c = list2.Count;
		for (int i = 0; i < c; ++i)
		{
			list.Add(list2[i]);
		}
		return list;
	}

	/// <summary>
	/// 剩余时间
	/// </summary>
	/// <param name="obj"></param>
	/// <returns>00d00h00m00s</returns>
	public static string ToLeftTime(this string obj)
	{
		string result = "";
		int time = 0;
		if (!int.TryParse(obj, out time))
		{
			Log.w("The Num maybe error.");
			time = 0;
			return obj.ToString();
		}

		System.TimeSpan ts = new System.TimeSpan(0, 0, time);//Log.i("00-" + time);


		if (ts.Days > 0)
		{
			result = result + ts.Days + "d";
		}

		if (ts.Hours > 0)
		{
			result = result + ts.Hours + "h";
		}
		if (ts.Minutes > 0)
		{
			result = result + ts.Minutes + "m";
		}

		if (ts.Seconds > 0)
		{
			result = result + ts.Seconds + "s";
		}
		if (result == "")
		{
			result = "0";
		}
		return result;
	}

	/// <summary>
	/// 转换成当地时间
	/// </summary>
	/// <param name="obj"></param>
	/// <returns></returns>
	public static System.DateTime ToLocalTime(this long obj)
	{
		long time = obj;
		//Log.iError(time);
		System.DateTime dt = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
		dt = dt.AddMilliseconds((double)time).ToLocalTime();
		return dt;
	}

	public static System.DateTime ToNormalTime(this string str)
	{
		DateTime dt = new DateTime ();
		string dateFormatStr = "yyyyMMddHHmmss";
		dt = DateTime.ParseExact (str, dateFormatStr, System.Globalization.CultureInfo.CurrentCulture);
		return dt;
	}

	/// <summary>
	/// 返回字典的键列表LIST
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="V"></typeparam>
	/// <param name="dic"></param>
	/// <returns></returns>
	public static List<T> DicKeysToList<T, V>(this Dictionary<T, V> dic)
	{
		List<T> tempList = new List<T>();

		foreach (T t in dic.Keys)
		{
			tempList.Add(t);
		}
		return tempList;
	}

	/// <summary>
	/// 返回字典的值列表LIST
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="V"></typeparam>
	/// <param name="dic"></param>
	/// <returns></returns>
	public static List<V> DicValuesToList<T, V>(this Dictionary<T, V> dic)
	{
		List<V> tempList = new List<V>();

		foreach (V v in dic.Values)
		{
			tempList.Add(v);
		}
		return tempList;
	}

	/// <summary>
	/// 序列化深度拷贝
	/// </summary>
	public static T DepthClone<T>(T obj) where T : class
	{
		MemoryStream stream = new MemoryStream();
		BinaryFormatter formatter = new BinaryFormatter();
		formatter.Serialize(stream, obj);
		stream.Position = 0;
		return formatter.Deserialize(stream) as T;
	}
}

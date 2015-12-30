/*
 * HttpClient.cs
 * Fast3
 * Created by com.sinodata on 12/28/2015 11:09:05.
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System;
using UnityEngine.EventSystems;

public enum RequestType
{
	POST = 0,
	GET = 1,
}

public class HttpClient : MonoBehaviour
{
	private float progress = 0;
	GameObject loadingObj;
	private Queue<KeyValuePair<RequestType, HttpRequest>> requestQueue = new Queue<KeyValuePair<RequestType, HttpRequest>>();

	#region
	/****************************************数据接口****************************************************/

	/// <summary>
	/// 初始化
	/// </summary>
	/// <param name="macid">Macid.终端系列号(本机维一码)</param>
	/// <param name="hallid">Hallid.大厅编号(从大厅认证取的厅码)</param>
	/// <param name="callback">Callback.</param>
	public void JInit(string macId, string hallId, Action<object> callback)
	{
		if(Const.UseMock) {
			string res = @"{'id=':'123'}";
				
			if(callback != null) {
				callback(res);
			
			} else {
				string url = Const.WebUrl + "/jinit";
				Dictionary<string,string> data = new Dictionary<string, string>() {
					{ "macId",macId },
					{ "hallId",hallId },
				};
				AddRequest(RequestType.POST, new HttpRequest(url, data, callback));
			}
		}
	}

	/// <summary>
	/// 注册大厅
	/// </summary>
	/// <param name="macId">Mac identifier.</param>
	/// <param name="hallId">Hall identifier.</param>
	/// <param name="terminalId">Terminal identifier.终端编号</param>
	/// <param name="terminalTag">Terminal tag.位置编号(特殊终端标签编码)</param>
	/// <param name="terminalType">Terminal type.终端机型号</param>
	/// <param name="callback">Callback.</param>
	public void JRegHall(string macId, string hallId, string terminalId, string terminalTag, string terminalType, Action<object> callback)
	{
		if(Const.UseMock) {
			string res = "{'backCode':'0','errorDesc':'成功' }";
			if(callback != null) {
				callback(res);
			}
		} else {
			string url = Const.WebUrl + "/jreghall";
			Dictionary<string,string> data = new Dictionary<string, string>() {
				{ "macId",macId },
				{ "hallId",hallId },
				{ "terminalId",terminalId },
				{ "terminalTag",terminalTag },
				{ "terminalType",terminalType },
			};
			AddRequest(RequestType.POST, new HttpRequest(url, data, callback));
		}
	}

	/// <summary>
	/// 基础数据
	/// </summary>
	/// <param name="callback">Callback.</param>
	public void JBaseData(Action<object> callback)
	{
		if(Const.UseMock) {
			string res = "";
			if(callback != null) {
				callback(res);
			}
		} else {
			string url = Const.WebUrl + "/jbasedata";
			Dictionary<string,string> data = new Dictionary<string, string>();
			AddRequest(RequestType.POST, new HttpRequest(url, data, callback));
		}
	}
		
	/// <summary>
	/// Login the specified cardno, pwd and callback.
	/// </summary>
	/// <param name="cardno">Cardno.</param>
	/// <param name="pwd">Pwd.</param>
	/// <param name="callback">Callback.</param>
	public void Login(string cardno, string pwd, Action<object> callback)
	{
		if(Const.UseMock) {
			string res = "{'cardno':'123','pwd':'123'}";
			if(callback != null) {
				callback(res);
			}
		} else {
			string url = Const.WebUrl + "/login";
			Dictionary<string,string> data = new Dictionary<string, string>() {
				{ "cardno",cardno },
				{ "pwd",pwd },
			};
			AddRequest(RequestType.POST, new HttpRequest(url, data, callback));
		}
	}

	public void GetUserInfo(string usrid, Action<object> callback)
	{
		if(Const.UseMock) {
			string res = "{'id':'123','money':'10000'}";
			if(callback != null) {
				callback(res);
			}
		} else {
			Dictionary<string,string> data = new Dictionary<string, string>(){ { "userid",usrid } };
			string url = Const.WebUrl + "/getuserinfo";
			AddRequest(RequestType.GET, new HttpRequest(url, data, callback));
		}
	}
	/****************************************数据接口****************************************************/
	#endregion

	/*------------------------------------------------------------------------------------------------*/
	void Start()
	{
		loadingObj = GameObject.Find("Loading");
		ShowLoading(false);
	}

	void Update()
	{
		if(requestQueue.Count > 0) {
			KeyValuePair<RequestType, HttpRequest> hr = requestQueue.Dequeue();
			//POST
			if(hr.Key == RequestType.POST) {
				StartCoroutine(POST(hr.Value.url, hr.Value.data, hr.Value.callback));
			} else if(hr.Key == RequestType.GET) {//GET
				StartCoroutine(GET(hr.Value.url, hr.Value.data, hr.Value.callback));
			}
		}
	}

	/// <summary>
	/// 加入请求队列
	/// </summary>
	/// <param name="type">Type.</param>
	/// <param name="hr">Hr.</param>
	void AddRequest(RequestType type, HttpRequest hr)
	{
		requestQueue.Enqueue(new KeyValuePair<RequestType, HttpRequest>(type, hr));
	}

	void ShowLoading(bool st)
	{
		if(loadingObj)
			loadingObj.SetActive(st);
	}

	public float Progress()
	{
		return progress;
	}

	IEnumerator POST(string url, Dictionary<string, string> post, Action<object> callback)
	{
		ShowLoading(true);
		WWWForm form = new WWWForm();
		foreach(KeyValuePair<string, string> post_arg in post) {
			form.AddField(post_arg.Key, post_arg.Value);
		}
		WWW www = new WWW(url, form);

		yield return www;
		progress = www.progress;
		string mContent = "";

		if(www.error != null) {
			mContent = "error :" + www.error;
		} else {
			mContent = www.text;
		}
		if(callback != null) {
			callback(mContent);
		}
		ShowLoading(false);
	}

	IEnumerator GET(string url, Dictionary<string, string> get, Action<object> callback)
	{
		ShowLoading(true);
		string Parameters;
		bool first;
		if(get.Count > 0) {
			first = true;
			Parameters = "?";
			foreach(KeyValuePair<string, string> post_arg in get) {
				if(first)
					first = false;
				else
					Parameters += "&";

				Parameters += post_arg.Key + "=" + post_arg.Value;
			}
		} else {
			Parameters = "";
		}

		WWW www = new WWW(url + Parameters);
		yield return www;
		progress = www.progress;
		string mContent = "";
		if(www.error != null) {
			mContent = "error :" + www.error;
		} else {
			mContent = www.text;
		}
		if(callback != null) {
			callback(mContent);
		}
		ShowLoading(false);
	}

	IEnumerator GETTexture(string picURL, Action<object> callback)
	{
		WWW wwwTexture = new WWW(picURL);

		yield return wwwTexture;

		Texture2D tex = null;
		if(wwwTexture.error != null) {
			Debug.Log("error :" + wwwTexture.error);
		} else {
			tex = wwwTexture.texture;
		}

		if(callback != null) {
			callback(tex);
		}
	}
}


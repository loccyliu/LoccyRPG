/*
 * HttpClient.cs
 * Rpg
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
	private Queue<KeyValuePair<RequestType, HttpRequest>> requestQueue = new Queue<KeyValuePair<RequestType, HttpRequest>>();

#region
	/****************************************数据接口****************************************************/
	/// <summary>
	/// 注册机器
	/// </summary>
	/// <param name="macId">Mac identifier.</param>
	/// <param name="terminalType">Terminal type.</param>
	/// <param name="callback">Callback.</param>
	public void JTMAutoReg(string macId, string terminalType, Action<object> callback)
	{
		if(Const.UseMock) {
			string res = "{'backCode':'0','errorDesc':'成功' }";
			if(callback != null) {
				callback(res);
			}
		} else {
			string url = Const.WebUrl + "/jtmautoreg";
			Dictionary<string,string> data = new Dictionary<string, string>() {
				{ "macId",macId },
				{ "terminalType",terminalType },
			};
			AddRequest(RequestType.POST, new HttpRequest(url, data, callback));
		}
	}

	/// <summary>
	/// 初始化检查
	/// </summary>
	/// <param name="macId">Mac identifier.</param>
	/// <param name="callback">Callback.</param>
	public void JInitCheck(string macId, Action<object> callback)
	{
		if(Const.UseMock) {
			string res = "{'backCode':'0','errorDesc':'校验成功' }";
			if(callback != null) {
				callback(res);
			}
		} else {
			string url = Const.WebUrl + "/jinitcheck";
			Dictionary<string,string> data = new Dictionary<string, string>() {
				{ "macId",macId },
			};
			AddRequest(RequestType.POST, new HttpRequest(url, data, callback));
		}
	}


	/// <summary>
	/// 初始化
	/// </summary>
	/// <param name="macid">Macid.终端系列号(本机维一码)</param>
	/// <param name="hallid">Hallid.大厅编号(从大厅认证取的厅码)</param>
	/// <param name="callback">Callback.</param>
	public void JInit(string macId, string hallId, Action<object> callback)
	{
		if (Const.UseMock) {
			string res = @"{
				'terminal': {
				'terminalId': '123456789012',
				'macId': null,
				'terminalType': '8400',
				'hallId': null,
				'hallName': null,
				'tagIds': '001,002',
				'tagNames': '001,002',
				'bingCard': null,
				'bingTime': null,
				'terminalTag': null,
				'terminalState': '0'
			},
			'errorDesc': '系统初始访问成功！',
			'sysparam': {
				'outTime': null,
				'initPlayId': null,
				'connTimeout': null,
				'analyseAppUrl': 'http://10.20.46.242:8080/LotteryGamesDeepData/k3ZZAnalyseChart?playId=4&selectDate=#1',
				'playinfo': null
			},
			'backCode': '0',
			'playinfo': {
				'playIds': '1,2,3,4,7,8,9',
				'playEnames': 'B001,S3,QL730,K3,wf7,K3,ABC',
				'playCnames': '双色球,数字3,七乐彩,快乐3,玩法7,快3-2,ABC',
				'playId': null,
				'playEname': null,
				'playCname': null,
				'getTermTime': null,
				'sellStopCount': null,
				'maxMultiple': null,
				'termLong': null,
				'dtMaxCodeNum': null,
				'fxMaxCodeNum': null,
				'wfxMaxCodeNum': null,
				'sellMinTimes': null,
				'maxCodeNum': null,
				'showTermNum': null}}";

			if (callback != null) {
				callback(res);
			}
		} else {
			string url = Const.WebUrl + "/jinit";
			Dictionary<string,string> data = new Dictionary<string, string>() {
				{ "macId", macId },
				{ "hallId", hallId },
			};
			AddRequest(RequestType.POST, new HttpRequest(url, data, callback));
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
			string res = "{\"errorDesc\":\"获取系统基础参数成功！\",\"backCode\":\"0\",\"sysparam\":{\"outTime\":\"25\",\"initPlayId\":\"5\",\"connTimeout\":\"10\",\"analyseAppUrl\":null,\"playinfo\":null},\"playinfo\":[{\"playIds\":null,\"playEnames\":null,\"playCnames\":null,\"playId\":\"1\",\"playEname\":\"B001\",\"playCname\":\"双色球\",\"getTermTime\":null,\"sellStopCount\":null,\"maxMultiple\":null,\"termLong\":null,\"dtMaxCodeNum\":null,\"fxMaxCodeNum\":null,\"wfxMaxCodeNum\":null,\"sellMinTimes\":null,\"maxCodeNum\":null,\"showTermNum\":null},{\"playIds\":null,\"playEnames\":null,\"playCnames\":null,\"playId\":\"2\",\"playEname\":\"S3\",\"playCname\":\"数字3\",\"getTermTime\":null,\"sellStopCount\":null,\"maxMultiple\":null,\"termLong\":null,\"dtMaxCodeNum\":null,\"fxMaxCodeNum\":null,\"wfxMaxCodeNum\":null,\"sellMinTimes\":null,\"maxCodeNum\":null,\"showTermNum\":null},{\"playIds\":null,\"playEnames\":null,\"playCnames\":null,\"playId\":\"3\",\"playEname\":\"QL730\",\"playCname\":\"七乐彩\",\"getTermTime\":null,\"sellStopCount\":null,\"maxMultiple\":null,\"termLong\":null,\"dtMaxCodeNum\":null,\"fxMaxCodeNum\":null,\"wfxMaxCodeNum\":null,\"sellMinTimes\":null,\"maxCodeNum\":null,\"showTermNum\":null},{\"playIds\":null,\"playEnames\":null,\"playCnames\":null,\"playId\":\"4\",\"playEname\":\"K3\",\"playCname\":\"快乐3\",\"getTermTime\":null,\"sellStopCount\":null,\"maxMultiple\":null,\"termLong\":null,\"dtMaxCodeNum\":null,\"fxMaxCodeNum\":null,\"wfxMaxCodeNum\":null,\"sellMinTimes\":null,\"maxCodeNum\":null,\"showTermNum\":null},{\"playIds\":null,\"playEnames\":null,\"playCnames\":null,\"playId\":\"5\",\"playEname\":\"K2\",\"playCname\":\"快2\",\"getTermTime\":\"30\",\"sellStopCount\":\"10\",\"maxMultiple\":\"400\",\"termLong\":\"300\",\"dtMaxCodeNum\":null,\"fxMaxCodeNum\":null,\"wfxMaxCodeNum\":null,\"sellMinTimes\":\"1\",\"maxCodeNum\":\"15\",\"showTermNum\":\"3\"}]}";
			if(callback != null) {
				callback(res);
			}
		} else {
			string url = Const.WebUrl + "/jbasedata";
			Dictionary<string,string> data = new Dictionary<string, string>();
			AddRequest(RequestType.POST, new HttpRequest(url, data, callback));
		}
	}

	public void JSyncTime(Action<object> callback)
	{
		if(Const.UseMock) {
			string res = "{\"backCode\":\"0\",\"errorDesc\":\"成功\",\"game\":[{\"serviceDate\":\"20140311232323\"}]}";
			if(callback != null) {
				callback(res);
			}
		} else {
			string url = Const.WebUrl + "/jsynctime";
			Dictionary<string,string> data = new Dictionary<string, string>();
			AddRequest(RequestType.GET, new HttpRequest(url, data, callback));
		}
	}


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
				{ "cardno", Util.MD5(cardno) },
				{ "pwd", Util.MD5(pwd) },
			};
			AddRequest(RequestType.POST, new HttpRequest(url, data, callback));
		}
	}

	public void ModifyPwd(string oldpwd, string newpwd, Action<object> callback)
	{
		if(Const.UseMock) {
			string res = "{'errorcode':'0','msg':'ok'}";
			if(callback != null) {
				callback(res);
			}
		} else {
			string url = Const.WebUrl + "/modifypwd";
			Dictionary<string,string> data = new Dictionary<string, string>() {
				{ "oldpwd", Util.MD5(oldpwd) },
				{ "newpwd", Util.MD5(newpwd) },
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
	private float progress = 0;

	void Start()
	{
		//requestQueue = new Queue<KeyValuePair<RequestType, HttpRequest>>();
	}

	void Update()
	{
		if(requestQueue.Count > 0) {
			KeyValuePair<RequestType, HttpRequest> hr = requestQueue.Dequeue();
			if(hr.Key == RequestType.POST) {
				//POST
				StartCoroutine(POST(hr.Value.url, hr.Value.data, hr.Value.callback));
			} else if(hr.Key == RequestType.GET) {
				//GET
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
		Debug.Log("add:>>" + hr.url);
		requestQueue.Enqueue(new KeyValuePair<RequestType, HttpRequest>(type, hr));
	}

	void ShowLoading(bool st)
	{
		if (st)
			EventSystem.Instance.FireEvent(EventCode.EnableDialog, new UIDialogParams(UIDialogID.Loading, null, null));
		else
			EventSystem.Instance.FireEvent(EventCode.DisableDialog, new UIDialogParams(UIDialogID.Loading, null, null));
	}

	public float Progress()
	{
		return progress;
	}

	private void Print(string url,Dictionary<string,string> p)
	{
		string info = "";
		info += "Send:>>" + url + "\n";
		foreach (string k in p.Keys) {
			info += string.Format("{0}={1}\n", k, p[k]);
		}
		Log.i(info);
	}

	IEnumerator POST(string url, Dictionary<string, string> post, Action<object> callback)
	{
		Print(url, post);
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
			//mContent = "error :" + www.error;
			//NetError网络错误等
			ShowLoading(false);
			yield return null;
		} else {
			mContent = www.text;
		}
		Log.i("Recive:<<" + mContent);
		if(callback != null) {
			callback(mContent);
		}
		ShowLoading(false);
	}

	IEnumerator GET(string url, Dictionary<string, string> get, Action<object> callback)
	{
		Print(url, get);
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
			//mContent = "error :" + www.error;

			//NetError网络错误等
			ShowLoading(false);
			yield return null;
		} else {
			mContent = www.text;
		}
		Log.i("Recive:<<" + mContent);
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

	IEnumerator GETTextureByte(string picURL, Action<object> callback)
	{
		WWW www = new WWW(picURL);
		yield return www;

		Texture2D tex = null;
		if(www.error != null) {
			Debug.Log("error :" + www.error);
		} else {
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(new StringReader(www.text));
			//通过索引查找子节点 
			string PicByte = xmlDoc.GetElementsByTagName("base64Binary").Item(0).InnerText;
			tex = Util.ByteToPic(PicByte);
		}

		if(callback != null) {
			callback(tex);
		}
	}
}


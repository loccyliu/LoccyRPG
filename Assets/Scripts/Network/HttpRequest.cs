/*
 * NewBehaviourScript.cs
 * Fast3
 * Created by com.sinodata on 12/28/2015 16:46:03.
 */

using UnityEngine;
using System.Collections.Generic;
using System;

public class HttpRequest
{
	public string url;
	public Dictionary<string,string> data;
	public Action<object> callback;

	public HttpRequest(string _url,Dictionary<string,string> _data,Action<object> _callback)
	{
		this.url = _url;
		this.data = _data;
		this.callback = _callback;
	}
}
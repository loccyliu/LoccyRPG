/*
 * NetworkManager.cs
 * RpgFramework
 * Created by com.loccy on 11/19/2015 14:44:13.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class NetworkManager : MonoBehaviour
{
	private int count;
	//private TimerInfo timer;
	private bool islogging = false;
	private static Queue<KeyValuePair<int, ByteBuffer>> sEvents = new Queue<KeyValuePair<int, ByteBuffer>> ();


	public static void AddEvent(int _event, ByteBuffer data)
	{
		sEvents.Enqueue (new KeyValuePair<int, ByteBuffer> (_event, data));
	}

	void Update()
	{
		if (sEvents.Count > 0)
		{
			while (sEvents.Count > 0)
			{
				KeyValuePair<int, ByteBuffer> _event = sEvents.Dequeue ();
				switch (_event.Key)
				{
				case Protocal.Connect:
					OnConnect ();
					break;
				case Protocal.Exception:
					OnException ();
					break;
				case Protocal.Disconnect:
					OnDisconnect ();
					break;
				default: 
					
					break;
				}
			}
		}
	}

	public void SendConnect()
	{
		Const.SocketPort = 2012;
		Const.SocketAddress = "127.0.0.1";
		SocketClient.SendConnect ();
	}

	public void SendMessage(ByteBuffer buffer)
	{
		SocketClient.SendMessage (buffer);
	}


	public void OnConnect()
	{
		Debug.LogWarning ("Game Server connected!!");
	}

	public void OnException()
	{
		islogging = false; 
		Debug.LogError ("OnException------->>>>");
	}


	public void OnDisconnect()
	{
		islogging = false;
		Debug.LogError ("OnDisconnect------->>>>");
	}


	void OnDestroy()
	{
		Debug.Log ("~NetworkManager was destroy");
	}
}
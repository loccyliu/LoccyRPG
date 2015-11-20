/*
 * TimeManager.cs
 * RpgFramework
 * Created by com.loccy on 11/19/2015 14:41:55.
 */

using UnityEngine;
using System.Collections.Generic;

public class TimeManager : MonoBehaviour 
{
	public Dictionary<ulong,CTimer> timerList = new Dictionary<ulong, CTimer>();
	ulong _tid = 0;

	/// <summary>
	/// Adds a timer.
	/// </summary>
	/// <returns>The timer.</returns>
	public ulong AddTimer()
	{
		_tid++;
		CTimer ct = new CTimer();
		timerList.Add(_tid,ct);

		return _tid;
	}
	/// <summary>
	/// Removes the timer.
	/// </summary>
	/// <param name="id">Identifier.</param>
	public void RemoveTimer(ulong id)
	{
		if(timerList.ContainsKey(id))
		{
			timerList[id].Stop();
			timerList.Remove(id);
		}
	}
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update()
	{
		foreach (ulong key in timerList.Keys)
		{
			if (timerList[key].Update(Time.deltaTime))
			{
				timerList.Remove(key);
			}
		}
	}
}

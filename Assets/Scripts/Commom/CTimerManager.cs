/*
 * CTimerManager
 * 20151008 11:12:10
 * Loccy
 */
using UnityEngine;
using System.Collections.Generic;

public class CTimerManager : MonoBehaviour
{
	public Dictionary<ulong,CTimer> timerList = new Dictionary<ulong, CTimer>();
	ulong _tid = 0;

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

	public ulong AddTimer()
	{
		_tid++;
		CTimer ct = new CTimer();
		timerList.Add(_tid,ct);

		return _tid;
	}

	public void RemoveTimer(ulong id)
	{
		if(timerList.ContainsKey(id))
		{
			timerList[id].Stop();
			timerList.Remove(id);
		}
	}
}

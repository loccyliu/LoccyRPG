/*
 * CTimer
 * 20151008 11:18:10
 * Loccy
 */
using UnityEngine;
using System.Collections;

public class CTimer
{
	bool isTick = false;
	float curTime;
	float triggerTime;

	EventHandler tick = null;
	object tickParams;


	public void Start(float time, object para)
	{
		curTime = 0;
		triggerTime = time;
		tickParams = para;
		isTick = true;
	}

	public bool Update(float deltaTime)
	{
		if (isTick)
		{
			curTime += deltaTime;
			if (curTime > triggerTime)
			{
				tick(tickParams);
				isTick = false;
				return true;
			}
		}
		return false;
	}

	public void Pause()
	{
		isTick = false;
	}

	public void Resume()
	{
		isTick = true;
	}

	public void Stop()
	{
		isTick = false;
		curTime = 0;
	}
}
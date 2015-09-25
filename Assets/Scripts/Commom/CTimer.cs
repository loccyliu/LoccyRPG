using UnityEngine;
using System.Collections;

public class CTimer
{
	bool isTick = false;
	float curTime;
	float triggerTime;

	EventHandler tick;
	object tickParams;


	public void Start(float time, object para)
	{
		curTime = 0;
		triggerTime = time;
		tickParams = para;
		isTick = true;
	}

	public void Update(float deltaTime)
	{
		if(isTick)
		{
			curTime += deltaTime;
			if(curTime > triggerTime)
			{
				tick(tickParams);
				isTick = false;
			}
		}
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
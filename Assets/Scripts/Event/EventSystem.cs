/*
 * EventSystem
 * 20150925 15:20:23
 * Loccy
 */

using System.Collections.Generic;

public delegate void EventHandler(object para);

public class EventSystem : Singleton<EventSystem>
{
	private Dictionary<EventCode, EventHandler> eventDic = new Dictionary<EventCode, EventHandler>();

	private EventSystem()
	{
	}
	
	public void RegistEvent(EventCode code, EventHandler callback)
	{
		if (eventDic.ContainsKey(code))
		{
			eventDic[code] += callback;
		}
		else
		{
			eventDic.Add(code, callback);
		}
	}
	
	public void FireEvent(EventCode code, object para = null)
	{
		if (eventDic.ContainsKey(code))
		{
			eventDic[code](para);
		}
	}
	
	public void UnregistEvent(EventCode code, EventHandler callback)
	{
		if (eventDic.ContainsKey(code))
		{
			eventDic[code] -= callback;
			if (eventDic[code] == null)
				eventDic.Remove(code);
		}
	}
}




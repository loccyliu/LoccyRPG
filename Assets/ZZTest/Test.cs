/*
 * Test
 * 20150925 15:20:56
 * Loccy
 */

using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{
	
		EventSystem.Instance.RegistEvent (EventCode.GameEvent1, onTest);
	}
	
	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown (KeyCode.B))
		{
			EventSystem.Instance.FireEvent (EventCode.EnableUIWindow, new UIEventParams (UIWindowID.PlayerListPop, "PlayerList"));
		}

		if (Input.GetKeyDown (KeyCode.C))
		{
			EventSystem.Instance.FireEvent (EventCode.DisableUIWindow, new UIEventParams (UIWindowID.PlayerListPop, "PlayerList"));
		}

		if (Input.GetKeyDown (KeyCode.A))
		{
			EventSystem.Instance.FireEvent (EventCode.EnableUIWindow, new UIEventParams (UIWindowID.TestView, "TestView"));
		}

		if (Input.GetKeyDown (KeyCode.S))
		{
			EventSystem.Instance.FireEvent (EventCode.DisableUIWindow, new UIEventParams (UIWindowID.TestView, "TestView"));
		}

		if (Input.GetKeyDown (KeyCode.T))
		{
			Application.LoadLevel ("Test2");
		}

		if (Input.GetKeyDown (KeyCode.M))
		{
			ioo.uiManager.CreateMainUI (UIClassNames.MainUI, (go) => {
				Util.Add<MainUIView>(go);
			});
		}
	}

	void onTest(object para)
	{

	}
}

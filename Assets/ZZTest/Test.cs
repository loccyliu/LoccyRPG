/*
 * Test
 * 20150925 15:20:56
 * Loccy
 */

using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
	Vector3 dir = new Vector3(0,0,30);

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
			EventSystem.Instance.FireEvent (EventCode.EnableUIWindow, new UIViewParams (UIWindowID.PlayerListPop, "PlayerList"));
		}

		if (Input.GetKeyDown (KeyCode.C))
		{
			EventSystem.Instance.FireEvent (EventCode.DisableUIWindow, new UIViewParams (UIWindowID.PlayerListPop, "PlayerList"));
		}

		if (Input.GetKeyDown (KeyCode.A))
		{
			EventSystem.Instance.FireEvent (EventCode.EnableUIWindow, new UIViewParams (UIWindowID.TestView, "TestView"));
		}

		if (Input.GetKeyDown (KeyCode.S))
		{
			EventSystem.Instance.FireEvent (EventCode.DisableUIWindow, new UIViewParams (UIWindowID.TestView, "TestView"));
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

		transform.rotation *= Quaternion.Euler (dir * Time.deltaTime );
	}

	void onTest(object para)
	{

	}

	void OnGUI()
	{
		if(GUILayout.Button("main",GUILayout.Width(150))){
			ioo.uiManager.CreateMainUI (UIClassNames.MainUI, (go) => {
				Util.Add<MainUIView>(go);
			});
		}
	}
}

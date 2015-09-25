/*
 * Test
 * 20150925 15:20:56
 * Loccy
 */

using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		EventSystem.Instance.RegistEvent(EventCode.GameEvent1, onTest);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void onTest(object para)
	{

	}
}

using UnityEngine;
using System.Collections;

public interface IConfig
{
	void onReadConfig();
}

public class NPCConfig : IConfig
{

	public int id;
	public string name;
	public string model;
	public string icon;

	public double physical_a;

	public void onReadConfig()
	{

	}
}
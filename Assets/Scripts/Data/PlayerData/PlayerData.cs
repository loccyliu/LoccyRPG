/*
 * PlayerData.cs
 * RpgFramework
 * Created by com.loccy on 10/28/2015 15:38:34.
 */

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Player data.
/// </summary>
public class PlayerData
{
	public PlayerBasicInfo playerInfo;
	public Dictionary<uint, ItemData> playerItemDic;
	//...

}

/// <summary>
/// Player basic info.
/// </summary>
public class PlayerBasicInfo
{
	public uint id;
	public string name;
	public string icon;
	public string resource;
	public string gender;
	public int level;
	//...

}
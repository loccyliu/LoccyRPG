/*
 * Entity
 * 20150925 15:22:23
 * Loccy
 */
using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
	//=========Component=============
	protected Transform trans;
	protected CTimerManager timeManager;

	//=========AI Property===========



	//Base Property
	protected ulong entity_id = 0;
	protected string entity_name;
	protected EntityType entity_type;
	protected NPCConfig config;


	public Transform Trans
	{
		get{return trans;}
	}

	public CTimerManager TimeManager
	{
		get{return timeManager;}
	}

	public EntityType Type
	{
		get{return entity_type;}
	}

	public NPCConfig NPCConfig
	{
		get{return config;}
	}

}
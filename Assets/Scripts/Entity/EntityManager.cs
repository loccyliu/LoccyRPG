/*
 * EntityManager
 * 20150929 11:12:10
 * Loccy
 */

using UnityEngine;
using System.Collections.Generic;

public class EntityManager : MonoBehaviour 
{

	ulong e_id = 0;
	Dictionary<ulong,Entity> allEntity = new Dictionary<ulong, Entity>();
	Dictionary<EntityType,List<Entity>> categoryEntity = new Dictionary<EntityType, List<Entity>>();

	static EntityManager instance;

	public EntityManager Instance
	{
		get{return instance;}
	}
	
	void Awake () 
	{
		instance = this;
	}
	
	public ulong AddEntity(Entity _entity)
	{
		allEntity[e_id] = _entity;
		categoryEntity[_entity.Type].Add(_entity);
		return e_id;
	}

	public void RemoveEntity(ulong _entityid)
	{
		if(allEntity.ContainsKey(_entityid))
			allEntity.Remove(_entityid);
	}

	public Entity FindEntity(ulong _entityid)
	{
		if(allEntity.ContainsKey(_entityid))
			return allEntity[_entityid];
		return null;
	}



}

/*
 * Entity
 * 20150925 15:22:23
 * Loccy
 */
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
	//=========Component=============
	[SerializeField]
	protected Transform trans;
	[SerializeField]
	protected CTimerManager timeManager;

	//=========AI Property===========
	protected Dictionary<NPCAIState,AIBase> entityActions = new Dictionary<NPCAIState, AIBase>();
	protected AIBase curAction;

	//===========Config==============
	protected NPCConfig config;

	//Base Property
	[SerializeField]
	protected ulong entity_id = 0;
	[SerializeField]
	protected string entity_name = "";
	[SerializeField]
	protected EntityType entity_type = EntityType.None;
	[SerializeField]
	protected int level = 0;
	[SerializeField]
	protected bool isDead = false;
	[SerializeField]
	protected int hp = 0;
	[SerializeField]
	protected int max_hp = 0;
	protected double move_speed = 0;
	protected double attack_speed = 0;
	protected double physical_a = 0;
	//物攻
	protected double physical_p = 0;
	//物穿
	protected double physical_d = 0;
	//物防
	protected double magical_a = 0;
	//法强
	protected double magical_p = 0;
	//法穿
	protected double magical_d = 0;
	//魔抗


	protected Entity target = null;
	protected Entity direct_father = null;



	public Transform Trans { get { return trans; } }

	public CTimerManager TimeManager {	get { return timeManager; } }

	public EntityType Type { get { return entity_type; } }

	public NPCConfig NPCConfig { get { return config; } }

	public ulong ID{ get { return entity_id; } }

	public string Name{ get { return entity_name; } }

	public EntityType EntityType{ get { return entity_type; } }

	public int Level{ get { return level; } }

	public bool IsDead{ get { return isDead; } }

	public int HP{get{ return hp;}}

	public int MaxHP{get{ return max_hp;}}
}
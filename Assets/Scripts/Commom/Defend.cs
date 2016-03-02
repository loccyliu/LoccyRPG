/*
 * Defend
 * 20150925 11:12:10
 * Loccy
 */

public class Defend
{
	
}

public enum LogType
{
	None,
	LogScreen,
	LogFile,
}

public enum UIWindowID
{
	None = 0,

	MainUI = 50,
	PlayerListPop,
	TestView,
}

public enum UIDialogID
{
	None=0,
	MsgBox,
	MsgBoxWithOk,
	MsgBoxWithOkCancel,
	Loading,
	Tips,
}

public enum EntityType
{
	None,
	Monster1,
	Boss1,
};

public enum NPCAIState {
	NULL, //空
	
	Idle, //空闲
	Attack, //攻击
	Dead, //死亡
	Unusual, //异常状态
};

//NPC的AI类型
public enum NPCAIType
{
	/***攻击ID段****/
	AttackAIStartID = 0, 
	AttackAIEndID = 199,
	/***待机ID段****/
	IdleAIStartID = 200,
	IdleAIEndID = 999,
	/***死亡状态ID段****/
	DeadAIStartID = 1000,
	DeadAIEndID = 1999,
	//***异常状态ID段****//
	UnusualAIStartID = 2000,
	UnusualAIEndID = 2999,
	
	//*** 攻击状态 (NPC)****//	
	AttackBack = 1, //边后退，边攻击
	StraightRunSpell = 2, //直线跑（子弹）
	HeroAI = 3, //英雄AI
	EjectSpellTimesOnEnemy = 4, //在数名敌人间弹射（子弹)
	CMoveSAndAttackNPC = 5, //S型向前移动，并接触攻击（NPC）
	StraightRunNPC = 6, //直线运动（NPC）
	StraightRunFaceToEnemy = 7, //一直向敌人直线运动(NPC)
	StraightRunFaceToEnemyAndAttack = 8, //一直向敌人直线运动，接触释放技能(NPC)
	StaySpace = 9, //固定一个地方，如果配置技能，就会攻击
	LongLaserLight = 10, //长激光
	GoldAI = 11, //金币AI
	TrapAI = 12, //陷阱AI
	FollowEntityBullet = 13, //跟踪子弹(子弹)
	BrushEntityPoint = 14, //兵营AI
	LaserLightTower = 15, //激光塔专用AI
	CycleBullet=16,//回旋子弹
	StartBaseSkillOnStay = 17, //呆在原地释放技能
	BoxAI = 18, //箱子AI
	StraightRunToSmallSpell = 19,//变小
	
	//子弹AI从50开始
	DivideBullet = 50, //分裂子弹AI(子弹)
	ParaCurveBullet = 51, //抛物线子弹(子弹)
	FollowSpecifyEntityBullet = 52, //跟踪指定entity的子弹(子弹)
	FireBullet = 53, //火焰的子弹
	StrikeBullet = 54, //闪击子弹

	
	//*** 待机状态 ****//
	PlayAnimation = 201, //播放一段出生动画
	
	//*** 死亡状态 ****//
	CRemoveOnPlayAnimation = 1001, //播放完动画，移出
	CRemoveOnce = 1002,  //立即移除
	CRemoveNext = 1003, //死亡，下一帧移除
	
	//*** 异常状态 ****// 
	StopFightState = 2001, //停止战斗AI
	CHoldState = 2002,  //定身，什么都不能做
	
	
}
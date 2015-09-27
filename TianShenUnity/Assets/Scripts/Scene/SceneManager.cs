using UnityEngine;
using System.Collections.Generic;

// 场景管理器 主要用于切换场景模式
// @ 实际村落数据由SceneComp管理
public class SceneManager : MonoBehaviour {
	
	public static SceneManager Instance {get; private set;}

	public SceneComp CurSceneComp;			// 当前场景组件
	public EffectRootComp EffectRootComp;	// 特效管理节点
	public EnemyGroupComp EnemyGroupComp;	// 敌人管理节点 (Actor)
	public SensorGroupComp SensorGroupComp;	// 感应区管理节点 - 3D场景交互的关键

	// 建筑物节点
	public List<BuildingComp> BuildingCompList{ get {return CurSceneComp.BuildingCompList;}}

	// 数据快捷访问
	public ESceneMode CurSceneMode{get{if(CurSceneComp)return CurSceneComp.SceneMode; return ESceneMode.None;}}
	public UserData VillageOwnerData {get { return CurSceneComp.OwnerData; }}
	public VillageData VillageData {get{return CurSceneComp.VillageData; }}

	// 便捷访问场景组件模式，但同一时间只能访问到其中之一
	public SceneComp_Build 	SceneComp_Build;
	public SceneComp_Visit  SceneComp_Visit;
	public SceneComp_Battle SceneComp_Battle;

	void Start () {
		Instance = this;

		// 存储管理节点
		EffectRootComp = GetComponentInChildren<EffectRootComp>();
		EnemyGroupComp = GetComponentInChildren<EnemyGroupComp>();
		SensorGroupComp = GetComponentInChildren<SensorGroupComp>();
	}

	void Update () {
	
	}

	// 切换场景
	// NOTE 因为根据游戏状态智能获取场景数据，你必须在调用前更新持有者PlayerManager中的数据
	public void SwitchScene(ESceneMode sceneMode)
	{
		if(CurSceneComp == null || CurSceneMode != sceneMode)
		{
			// 清理
			SceneComp oldSceneComp = GetComponent<SceneComp>();
			if(oldSceneComp)
			{
				oldSceneComp.CleanUp();
				Destroy(oldSceneComp);
			}
			SceneComp_Build = null;
			SceneComp_Battle = null;
			SceneComp_Visit = null;
			
			switch(sceneMode)
			{
			case ESceneMode.Player:
				CurSceneComp = SceneComp_Build = gameObject.AddComponent<SceneComp_Build>();
				break;
			case ESceneMode.Visit:
	            CurSceneComp = SceneComp_Visit = gameObject.AddComponent<SceneComp_Visit>();
	            break;
			case ESceneMode.Battle:
				CurSceneComp = SceneComp_Battle = gameObject.AddComponent<SceneComp_Battle>();
				break;
            }
		}

		if(sceneMode != ESceneMode.None)
		{
			CurSceneComp.SceneMode = sceneMode;
			CurSceneComp.RecreateAllBuildings();
		}
	}

	// 实用方法-返回玩家自己的场景
	public void SwitchToPlayerScene()
	{
		SwitchScene(ESceneMode.Player);
	}
}

public enum ESceneMode
{
	None,		// 没有场景
	Player,		// 经营
	Visit,		// 他人
	Battle,		// 他人 战场
}
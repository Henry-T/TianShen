using UnityEngine;
using System.Collections;

// 3D经营管理器
// 提供布置状态管理
// 当前场景必然是 - 玩家场景
public class SceneComp_Build : SceneComp
{
	public enum EBuildState
	{
		Default,	// 默认无状态
		Move,		// 移动状态
	}

	public EBuildState BuildState;			// 经营场景状态

	public bool MovingBuilding = false;		// 移动中

	// 交换建筑槽位
	public void SwapBuildingSlot()
	{
		if(true)
		{
			int _slotId = PlayerManager.Instance.PlayerBuildingDataList[2].SlotID;
			PlayerManager.Instance.PlayerBuildingDataList[2].SlotID = PlayerManager.Instance.PlayerBuildingDataList[0].SlotID;
			PlayerManager.Instance.PlayerBuildingDataList[0].SlotID = _slotId;
			
			PlayerManager.Instance.PlayerBuildingDataList.Sort((a,b)=>{
				return a.SlotID.CompareTo(b.SlotID);
			});
			
			SceneManager.Instance.CurSceneComp.RecreateAllBuildings();
		}
	}
}

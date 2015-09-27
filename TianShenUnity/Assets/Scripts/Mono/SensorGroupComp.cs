using UnityEngine;
using System.Collections.Generic;
using System.Linq;

// 这里混杂了各个系统 有点脏的地方 因此不被其他系统吸收独立存在
public class SensorGroupComp : MonoBehaviour {

	public Dictionary<int, SensorComp> SensorCompDic;

	// 是否不接受触控
	public bool IsTouchBlocked
	{
		get{
			UIManager uiManager = UIManager.Instance;
			return uiManager.WidgetYesNo.gameObject.activeSelf ||
				uiManager.WidgetBuildingControl.gameObject.activeSelf ||
				uiManager.WidgetBuildingInfo.gameObject.activeSelf ||
				uiManager.WidgetBuildingPicker.gameObject.activeSelf ||
					uiManager.WidgetWaiting.gameObject.activeSelf;
		}
	}

	void Start () {
		SensorCompDic = new Dictionary<int, SensorComp>();
		SensorComp[] sensorComps = GetComponentsInChildren<SensorComp>();
		foreach(SensorComp sensorComp in sensorComps)
		{
			SensorCompDic.Add(sensorComp.SlotId, sensorComp);
		}
	}

	void Update () {
		// 编辑建筑位置 - 所有可移动位置闪烁
		if(SceneManager.Instance.CurSceneMode == ESceneMode.Player && SceneManager.Instance.SceneComp_Build.MovingBuilding)
		{
			foreach(SensorComp sensor in SensorCompDic.Values)
			{
				if(SceneManager.Instance.CurSceneComp.BuildingCompList.Find(b=>b.Data.SlotID == sensor.SlotId))
				{
					sensor.GetComponentInChildren<Renderer>().enabled = false;
				}
				else
				{
					sensor.GetComponentInChildren<Renderer>().enabled = true;
				}
			}
		}
		else
		{
			foreach(SensorComp sensor in SensorCompDic.Values)
			{
				sensor.GetComponentInChildren<Renderer>().enabled = false;
			}
		}

		// 每帧获取一次点击
		RaycastHit hitResult;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		int sensorMask = LayerMask.GetMask(new string[]{"sensor"});
		bool hit =  Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hitResult,1000, sensorMask);
		int hitSlot = -1;
		if(hit)
			hitSlot = hitResult.collider.transform.parent.GetComponent<SensorComp>().SlotId;

		// 编辑模式 - 选择一个建筑
		if(hit && !IsTouchBlocked && SceneManager.Instance.CurSceneMode == ESceneMode.Player && !SceneManager.Instance.CurSceneComp.SelectedBuilding)
		{
			// 记录选中的建筑
			SceneManager.Instance.CurSceneComp.SelectedBuilding = SceneManager.Instance.CurSceneComp.BuildingCompList.Find(b=>b.Data.SlotID==hitSlot);
			UIManager.Instance.WidgetBuildingControl.Show(hitResult.collider.transform);		// 显示建筑操作面板
		}

		// 参观模式 - 选择一个建筑
		if(hit && !IsTouchBlocked && SceneManager.Instance.CurSceneMode == ESceneMode.Visit && !SceneManager.Instance.CurSceneComp.SelectedBuilding)
		{
			SceneManager.Instance.CurSceneComp.SelectedBuilding = SceneManager.Instance.CurSceneComp.BuildingCompList.Find(b=>b.Data.SlotID==hitSlot);
			UIManager.Instance.WidgetBuildingInfo.Show();
		}

		// 编辑模式 - 指定建筑物的新位置
		if(hit && !IsTouchBlocked && SceneManager.Instance.CurSceneMode == ESceneMode.Player && SceneManager.Instance.CurSceneComp.SelectedBuilding)
		{
			BuildingData selectedBuildingData = SceneManager.Instance.SceneComp_Build.SelectedBuilding.Data;
			int selSlot = selectedBuildingData.SlotID;

			// 检查目标位置是否已经有建筑物了
			BuildingData occupyBuilding = PlayerManager.Instance.PlayerBuildingDataList.Find(b=>b.SlotID == hitSlot);
			if(occupyBuilding != null)
			{
				occupyBuilding.SlotID = selectedBuildingData.SlotID;
			}
			
			// 设定位置
			selectedBuildingData.SlotID = hitSlot;

			GameManager.Instance.AsyncBeginWait();
			selectedBuildingData.AVObject.SaveAsync().ContinueWith(t=>{
				GameManager.Instance.AsyncEndWait(()=>{
					// 刷新场景 
					// SceneManager.Instance.CurSceneComp.
					SceneManager.Instance.CurSceneComp.RepositionAllBuildings();
					
					// 更新UI
					UIManager.Instance.WidgetCancelMove.gameObject.SetActive(false);
					Debug.LogWarning(SceneManager.Instance.SceneComp_Build.SelectedBuilding.transform.position);
					UIManager.Instance.WidgetBuildingControl.Show(SceneManager.Instance.SceneComp_Build.SelectedBuilding.transform);
				});
			});
		}

		// 战斗模式 - 指定攻击位置
		if(hit && !IsTouchBlocked && UIManager.Instance.CurScreen == EScreen.Fight)
		{
			// 在点击位置创建一条闪电
			GameObject prefab = Resources.Load<GameObject>("Effects/lightningBoltBase");
			GameObject lightning = Instantiate(prefab) as GameObject;
			lightning.transform.parent = SceneManager.Instance.EffectRootComp.transform;
			lightning.transform.position = new Vector3(hitResult.point.x, 2.05f, hitResult.point.z);
			
			// 敌人Actor
			EnemyComp enemyComp = SceneManager.Instance.EnemyGroupComp.EnemyComp;
			
			// 命中敌人检测
			if(SceneManager.Instance.SceneComp_Battle.BattleState == EBattleState.BattleOn && enemyComp.CurSlot == hitSlot && enemyComp.State == EnemyComp.EEnemyState.Show)
			{
				// TODO 根据神力和对手防御计算伤害
				SceneManager.Instance.SceneComp_Battle.HitCount ++;
				UIManager.Instance.UIFightPanel.DesDefence(30);
				UIManager.Instance.UIFightPanel.DesEnergy(2);
				if(SceneManager.Instance.SceneComp_Battle.HitCount >= 4)
				{
					SceneManager.Instance.SceneComp_Battle.BattleState = EBattleState.NotInBattle;

					// 提交战斗结果
					GameManager.Instance.AsyncBeginWait();
					PlayerManager.Instance.NetSubmitBattleResult(
						PlayerManager.Instance.PlayerVillageData.UserID, 
						PlayerManager.Instance.OtherVillageData.UserID, true, (data)=>{
							GameManager.Instance.AsyncEndWait(()=>{
								BattleData battleData = new BattleData();
								battleData.AVObject = data;
								PlayerManager.Instance.BattleDataList.Add(battleData);
								UIManager.Instance.WidgetYesNo.Show("胜利了！是否返回", ()=>{
									SceneManager.Instance.SceneComp_Battle.EndFight();
								}, ()=>{
									SceneManager.Instance.SceneComp_Battle.EndFight();
								});
							});
						}
					);
				}
			}
		}
	}
}

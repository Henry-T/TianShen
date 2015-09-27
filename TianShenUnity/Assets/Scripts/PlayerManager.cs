using UnityEngine;
using System.Collections.Generic;
using AVOSCloud;
using System.Threading.Tasks;
using System;

public class PlayerManager : MonoBehaviour {
	public static PlayerManager Instance {get; private set;}
	
	public bool IsLogin = false;		// 玩家是否登陆

	public VillageData PlayerVillageData;				// 玩家村落数据
	public List<BuildingData> PlayerBuildingDataList	// 玩家建筑数据
	{
		get{ return PlayerVillageData.BuildingDataList;}
	}

	// 观察目标
	public UserData OtherUserData;						// 当前敌人玩家数据
	public VillageData OtherVillageData;				// 当前敌人村落数据
	public List<BuildingData> OtherBuildingDataList;	// 当前敌人建筑数据

	// 事件&排行榜
	public List<BattleData> BattleDataList = new List<BattleData>();				// 与自己相关的战斗
	
	void Start()
	{
		Instance = this;
	}
	
	public int PP_UserLevel = 0;
	public int PP_UserBelief = 0;
	public int PP_UserPower = 0;
	public int PP_UserExp = 0;
	public int PP_UserDefence = 0;
	
	public int PP_EnemyLevel = 0;
	public int PP_EnemyBelief = 0;
	public int PP_EnemyPower = 0;
	public int PP_EnemyExp = 0;
	public int PP_EnemyDefence = 0;

	// 更新玩家计算属性
	public void RecalcPlayerProperties()
	{
		PP_UserLevel = (int)StaticVillage.CalcLevel(PlayerManager.Instance.PlayerBuildingDataList);
		PP_UserBelief = (int)PlayerManager.Instance.PlayerVillageData.Belief;
		PP_UserPower = (int)StaticVillage.CalcPower(PlayerManager.Instance.PlayerBuildingDataList);
		PP_UserExp = (int)StaticVillage.CalcExp(PlayerManager.Instance.PlayerBuildingDataList);
		PP_UserDefence = (int)StaticVillage.CalcDefence(PlayerManager.Instance.PlayerBuildingDataList);
	}

	// 更新他人计算属性
	public void RecalcOtherProperties()
	{
		PP_EnemyLevel = (int)StaticVillage.CalcLevel(PlayerManager.Instance.OtherBuildingDataList);
		PP_EnemyBelief = (int)PlayerManager.Instance.OtherVillageData.Belief;
		PP_EnemyPower = (int)StaticVillage.CalcPower(PlayerManager.Instance.OtherBuildingDataList);
		PP_EnemyExp = (int)StaticVillage.CalcExp(PlayerManager.Instance.OtherBuildingDataList);
		PP_EnemyDefence = (int)StaticVillage.CalcDefence(PlayerManager.Instance.OtherBuildingDataList);
	}

	// 网络-注册
	public void NetDoRegist(string userName, string password, string email)
	{
		AVUser user = new AVUser();
		user.Username = userName;
		user.Password = password;
		user.Email = email;

		GameManager.Instance.AsyncBeginWait();
		user.SignUpAsync().ContinueWith(t => {
			// TODO 创建玩家的Village
			GameManager.Instance.AsyncBeginWait();

			AVObject obj = new AVObject("Village");
			obj["UserID"] = AVUser.CurrentUser.ObjectId;
			obj["Belief"] = 0;
			obj["BeliefAll"] = 0;
			
			obj.SaveAsync().ContinueWith(t2 => {
				
				// 创建玩家神庙
				BuildingData newBuilding = new BuildingData();
				newBuilding.AVObject = new AVObject("Building");
				newBuilding.UserID = AVUser.CurrentUser.ObjectId;
				newBuilding.Type = EBuildingType.Altar;
				newBuilding.Level = 1;
				newBuilding.SlotID = 4;
				newBuilding.Sibling = 1;

				newBuilding.AVObject.SaveAsync().ContinueWith(t3=>{
					// 进入登录界面
					// TODO 将用户名和密码拷贝过去
					GameManager.Instance.AsyncEndWait(()=>{
						UIManager.Instance.ChangeScreen(EScreen.Login);
					});
				});
			});
		});
	}

	// 网络-登录
	public void NetDoLogin(String userName, String passWord, Action onLoggedIn)
	{
		GameManager.Instance.AsyncBeginWait();
		Debug.Log("开始登陆: " + Time.time);
		AVUser.LogInAsync(userName, passWord).ContinueWith(t=>{
			if(t.IsFaulted || t.IsCanceled)
			{
				var error = t.Exception.Message;
			}
			else
			{
				GameManager.Instance.AsyncEndWait(()=>{
					Debug.Log("登陆成功: " + Time.time);
					IsLogin = true;
					if(onLoggedIn != null)
						onLoggedIn();
				});
			}
		});
	}
	
	// 网络-获取玩家村落信息
	public void NetQueryPlayerVillageData(Action callback = null)
	{
		GameManager.Instance.AsyncBeginWait();
		AVQuery<AVObject> query=new AVQuery<AVObject>("Village").WhereEqualTo("UserID", AVUser.CurrentUser.ObjectId);
		query.FirstAsync().ContinueWith(t =>{
			AVObject villageObject = (t as Task<AVObject>).Result;
			VillageData villageData = new VillageData();
			villageData.AVObject = villageObject;
			
			AVQuery<AVObject> buildingQuery = new AVQuery<AVObject>("Building").WhereEqualTo("UserID", AVUser.CurrentUser.ObjectId);
			buildingQuery.FindAsync().ContinueWith(t2=>{
				List<BuildingData> buildingDataList = new List<BuildingData>();
				foreach(AVObject buildingObject in (t2 as Task<IEnumerable<AVObject>>).Result)
				{
					BuildingData buildingData = new BuildingData();
					buildingData.AVObject = buildingObject;
					buildingDataList.Add(buildingData);
				}
				
				GameManager.Instance.AsyncEndWait(()=>{
					PlayerManager.Instance.PlayerVillageData = villageData;
					PlayerManager.Instance.PlayerVillageData.BuildingDataList = new List<BuildingData>();
					PlayerManager.Instance.PlayerVillageData.BuildingDataList = buildingDataList;

					if(callback != null)
						callback();
				});
			});
		});
	}
	// 网络-建造
	// 祭坛Altar不要从这里建造
	public void NetBuildNew(EBuildingType buildingType, Action callback = null)
	{
		// 检查是否达到同类上限
		int maxSibling = BuildingData.GetMaxSibling(PlayerBuildingDataList, buildingType);
			
		int nextSibling = maxSibling + 1;

		if(nextSibling > StaticBuilding.MAX_SIBLING)
		{
			Debug.LogWarning("已经超出同类建筑上限，不可再建造");
			return;
		}

		BuildingData newBuilding = new BuildingData();
		newBuilding.AVObject = new AVObject("Building");
		newBuilding.UserID = AVUser.CurrentUser.ObjectId;
		newBuilding.Type = buildingType;
		newBuilding.Level = 1;
		newBuilding.SlotID = PlayerManager.Instance.PlayerVillageData.GetEmptySlot();
		newBuilding.Sibling = nextSibling;

		// ### 在线版
		GameManager.Instance.AsyncBeginWait();
		newBuilding.AVObject.SaveAsync().ContinueWith(t => {
			// 释放异步等待
			GameManager.Instance.AsyncEndWait(()=>{
				PlayerVillageData.AddNewBuilding(newBuilding);
				SceneManager.Instance.SceneComp_Build.AddNewBuilding(newBuilding);
				UIManager.Instance.WidgetBuildingPicker.gameObject.SetActive(false);

				if(callback != null)
					callback();
			});
		});
	}

	// 网络-升级建筑
	public void NetUpgradeBuilding(BuildingData buildingData, Action<BuildingData> onFinished=null)
	{
		if(buildingData!=null && buildingData.Level <= 2)
		{
			buildingData.Level = buildingData.Level + 1;

			// ### 联网版
			GameManager.Instance.AsyncBeginWait();
			buildingData.AVObject.SaveAsync().ContinueWith(t=>{
				GameManager.Instance.AsyncEndWait(()=>{
					SceneManager.Instance.CurSceneComp.RecreateAllBuildings();
				});
			});
		}
	}

	// 网络-移动建筑
	// 请确保slotA建筑存在，slotB随意
	public void NetMoveBuilding(int slotA, int slotB, Action onFinished=null)
	{
		BuildingData buildingA = PlayerManager.Instance.PlayerVillageData.BuildingDataList.Find(d=>d.SlotID == slotA);
		BuildingData buildingB = PlayerManager.Instance.PlayerVillageData.BuildingDataList.Find(d=>d.SlotID == slotB);

		buildingA.SlotID = slotB;

		GameManager.Instance.AsyncBeginWait();
		buildingA.AVObject.SaveAsync().ContinueWith (t => {
			if(buildingB != null)
			{
				buildingB.SlotID = slotA;
				buildingB.AVObject.SaveAsync().ContinueWith(t2=>{
					GameManager.Instance.AsyncEndWait(()=>{
						SceneManager.Instance.CurSceneComp.RecreateAllBuildings();
					});
				});
			}
			else
			{
				GameManager.Instance.AsyncEndWait(()=>{
					SceneManager.Instance.CurSceneComp.RecreateAllBuildings();
				});
			}
		});
	}

	// 网络-收获建筑
	public void NetHarvestBuilding(BuildingData buildingData, Action<BuildingData> onFinished=null)
	{
		// 计算收获信仰
		DateTime now = DateTime.Now;
		double second = (now - buildingData.CollectTime).TotalSeconds;
		float totalT = buildingData.Level * 60;
		totalT = Mathf.Clamp(totalT, 0, 180);
		StaticBuildingData data = StaticBuilding.GetByTypeLevel(buildingData.Type, buildingData.Level, buildingData.Sibling);
		float product = (float)(data.Value * second / totalT);

		// 更新服务器信仰
		PlayerVillageData.Belief += (int)product;
		PlayerVillageData.AVObject.SaveAsync().ContinueWith(t=>{
			// 更新服务器收获时间
			buildingData.CollectTime = now;
			GameManager.Instance.AsyncBeginWait();
			buildingData.AVObject.SaveAsync().ContinueWith(t2=>{
				GameManager.Instance.AsyncEndWait(()=>{
					// TODO 播放吸收小太阳的动画 ...
				});
			});
		});
	}

	// 网络-获取随机玩家
	public void NetGetRandomOther(Action callback)
	{
		GameManager.Instance.AsyncBeginWait();
		var query = new AVQuery<AVObject>("Village").WhereNotEqualTo("UserID", AVUser.CurrentUser.ObjectId);
		Debug.Log("开始查找敌人村落");
		query.FindAsync().ContinueWith(t=>{
			List<AVObject> objList = new List<AVObject>();
			objList.AddRange((t as Task<IEnumerable<AVObject>>).Result);
			if(objList.Count > 0)
			{
				var rand = new System.Random();
				int r = rand.Next(objList.Count);
				AVObject villageObject = objList[r];
				VillageData villageData = new VillageData();
				villageData.AVObject = villageObject;

				Debug.Log("开始查找敌人建筑");
				AVQuery<AVObject> buildingQuery = new AVQuery<AVObject>("Building").WhereEqualTo("UserID", villageData.UserID);
				buildingQuery.FindAsync().ContinueWith(t2=>{
					List<BuildingData> buildingDataList = new List<BuildingData>();
					foreach(AVObject buildingObject in (t2 as Task<IEnumerable<AVObject>>).Result)
					{
						BuildingData buildingData = new BuildingData();
						buildingData.AVObject = buildingObject;
						buildingDataList.Add(buildingData);
					}
					
					Debug.Log("找到敌人建筑");
					GameManager.Instance.AsyncEndWait(()=>{
						OtherVillageData = villageData;
						OtherVillageData.BuildingDataList = buildingDataList;

						if(callback != null)
							callback();
					});
				});
			}
		});
	}

	// 获取特定玩家信息
	public void NetGetOtherByID(string objectID, Action callback)
	{
		GameManager.Instance.AsyncBeginWait();
		var query = new AVQuery<AVObject>("Village").WhereEqualTo("UserID", objectID);
		query.FindAsync().ContinueWith(t=>{
			List<AVObject> objList = new List<AVObject>();
			objList.AddRange((t as Task<IEnumerable<AVObject>>).Result);
			if(objList.Count > 0)
			{
				AVObject villageObject = objList[0];
				VillageData villageData = new VillageData();
				villageData.AVObject = villageObject;

				AVQuery<AVObject> buildingQuery = new AVQuery<AVObject>("Building").WhereEqualTo("UserID", villageData.UserID);
				buildingQuery.FindAsync().ContinueWith(t2=>{
					List<BuildingData> buildingDataList = new List<BuildingData>();
					foreach(AVObject buildingObject in (t2 as Task<IEnumerable<AVObject>>).Result)
					{
						BuildingData buildingData = new BuildingData();
						buildingData.AVObject = buildingObject;
						buildingDataList.Add(buildingData);
					}

					GameManager.Instance.AsyncEndWait(()=>{
						OtherVillageData = villageData;
						OtherVillageData.BuildingDataList = buildingDataList;
						
						if(callback != null)
							callback();
					});
				});
			}
		});
	}

	// 网络-提交战斗结果
	public void NetSubmitBattleResult(string invader, string defender, bool isWin, Action<AVObject> callback)
	{
		AVObject battleData = new AVObject("Battle");
		battleData["Invader"] = invader;
		battleData["Defender"] = defender;
		battleData["IsWin"] = isWin;
		battleData["Time"] = DateTime.Now;

		battleData.SaveAsync().ContinueWith(t=>{
			if(callback != null)
				callback(battleData);
		});
	}

	// TODO 将测试战斗转换为单机关卡
	public void TestBattleInitial()
	{
		/*
		// 初始化假玩家数据
		PlayerUserData = new AVUser(){ObjectId = "player_user_id", Username = "虚拟玩家"};
		PlayerVillageData = new VillageData(){UserID = "player_user_id", Defence = 0, Power = 0, Trick = 0, Belief = 0};
		PlayerBuildingDataList = new List<BuildingData>();
		PlayerBuildingDataList.AddRange(new BuildingData[]{
			new BuildingData(){UserID = "player_user_id", Type = EBuildingType.Altar, SlotID = 0, Level = 3, Sibling = 1},
			new BuildingData(){UserID = "player_user_id", Type = EBuildingType.Church, SlotID = 1, Level = 3, Sibling = 1},
			new BuildingData(){UserID = "player_user_id", Type = EBuildingType.Lib, SlotID = 2, Level = 3, Sibling = 1},
			new BuildingData(){UserID = "player_user_id", Type = EBuildingType.Tower, SlotID = 3, Level = 3, Sibling = 1},
		});
		
		// 计算玩家属性
		RecalcAllPlayerProperty();
		*/
		
		// 初始化假敌人数据
		AVUser tempUser = new AVUser(){ObjectId = "enemy_user_id", Username = "虚拟敌人"};
		OtherUserData = new UserData(tempUser);
		OtherVillageData = new VillageData(){UserID = "enemy_user_id", Defence = 0, Power = 0, Trick = 0, Belief = 0};
		OtherBuildingDataList = new List<BuildingData>();
		OtherBuildingDataList.AddRange(new BuildingData[]{
			new BuildingData(){UserID = "enemy_user_id", Type = EBuildingType.Altar, SlotID = 0, Level = 3, Sibling = 1},
			new BuildingData(){UserID = "enemy_user_id", Type = EBuildingType.Church, SlotID = 1, Level = 3, Sibling = 1},
			new BuildingData(){UserID = "enemy_user_id", Type = EBuildingType.Lib, SlotID = 2, Level = 3, Sibling = 1},
			new BuildingData(){UserID = "enemy_user_id", Type = EBuildingType.Tower, SlotID = 3, Level = 3, Sibling = 1},
		});
		
		// 计算敌人属性
		RecalcPlayerProperties();
		
		// 初始化敌人场景
		SceneManager.Instance.SwitchScene(ESceneMode.Visit);
	}

	public void TestBattle()
	{
		// 初始化假玩家数据
		PlayerVillageData = new VillageData(){UserID = "player_user_id", Defence = 0, Power = 0, Trick = 0, Belief = 0};
		PlayerBuildingDataList.AddRange(new BuildingData[]{
			new BuildingData(){UserID = "player_user_id", Type = EBuildingType.Altar, SlotID = 0, Level = 3, Sibling = 1},
			new BuildingData(){UserID = "player_user_id", Type = EBuildingType.Church, SlotID = 1, Level = 3, Sibling = 1},
			new BuildingData(){UserID = "player_user_id", Type = EBuildingType.Lib, SlotID = 2, Level = 3, Sibling = 1},
			new BuildingData(){UserID = "player_user_id", Type = EBuildingType.Tower, SlotID = 3, Level = 3, Sibling = 1},
		});
		
		// 计算玩家属性
		RecalcPlayerProperties();
		
		
		// 初始化假敌人数据
		AVUser tempUser = new AVUser(){ObjectId = "enemy_user_id", Username = "虚拟敌人"};
		OtherUserData = new UserData(tempUser);
		OtherVillageData = new VillageData(){UserID = "enemy_user_id", Defence = 0, Power = 0, Trick = 0, Belief = 0};
		OtherBuildingDataList = new List<BuildingData>();
		OtherBuildingDataList.AddRange(new BuildingData[]{
			new BuildingData(){UserID = "enemy_user_id", Type = EBuildingType.Altar, SlotID = 0, Level = 3, Sibling = 1},
			new BuildingData(){UserID = "enemy_user_id", Type = EBuildingType.Church, SlotID = 1, Level = 3, Sibling = 1},
			new BuildingData(){UserID = "enemy_user_id", Type = EBuildingType.Lib, SlotID = 2, Level = 3, Sibling = 1},
			new BuildingData(){UserID = "enemy_user_id", Type = EBuildingType.Tower, SlotID = 3, Level = 3, Sibling = 1},
		});
		
		// 计算敌人属性
		RecalcPlayerProperties();
		
		// 初始化敌人场景
		SceneManager.Instance.SwitchScene(ESceneMode.Battle);
		
		// 启动战斗UI
		UIManager.Instance.ChangeScreen(EScreen.Fight);
		
		// 开启战斗状态
		SceneManager.Instance.SceneComp_Battle.StartFight();
		// -- 此处强切一次状态
		SceneManager.Instance.SceneComp_Battle.BattleState = EBattleState.BattleOn;
	}
}

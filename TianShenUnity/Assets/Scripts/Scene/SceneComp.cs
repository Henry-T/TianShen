using UnityEngine;
using System.Collections.Generic;
using AVOSCloud;

// 3D场景管理基类 提供通用管理接口
public class SceneComp : MonoBehaviour
{
	// 数据
	public ESceneMode SceneMode;		// 场景模式

	// 快捷数据访问 - ReadOnly
	public VillageData VillageData{
		get{ 
			if(SceneMode == ESceneMode.Player)
				return PlayerManager.Instance.PlayerVillageData;
			else if(SceneMode == ESceneMode.Visit || SceneMode == ESceneMode.Battle)
				return PlayerManager.Instance.OtherVillageData;
			return null;
		}
 	}
	
	public UserData OwnerData{
		get {
			return VillageData!=null?VillageData.OwnerData:null;
		}
	}

	public List<BuildingData> BuildingDataList{
		get{
			return VillageData!=null?VillageData.BuildingDataList:null;
		}
	}

	// 建筑物件
	public List<BuildingComp> BuildingCompList = new List<BuildingComp>();
	
	// 当前选中的建筑物
	public BuildingComp SelectedBuilding
	{
		get { return selectedBuilding; }
		set {
			if(selectedBuilding)
			{
				MeshRenderer renderer = selectedBuilding.GetComponentInChildren<MeshRenderer>();
				if(renderer)
					renderer.material.color = Color.white;
			}
			
			selectedBuilding = value;
			
			if(selectedBuilding)
			{
				MeshRenderer renderer2 = selectedBuilding.GetComponentInChildren<MeshRenderer>();
				if(renderer2)
					renderer2.material.color = Color.red;
			}
		}
	}
	private BuildingComp selectedBuilding;

	void Start()
	{
	}

	// 刷新场景物件
	public void RecreateAllBuildings()
	{
		foreach(BuildingComp buildingComp in BuildingCompList)
		{
			if(buildingComp)
				Destroy(buildingComp.gameObject);
		}

		BuildingCompList.Clear();
		
		foreach(BuildingData buildingData in BuildingDataList)
		{
			AddNewBuilding(buildingData);
		}
	}

	public void CleanUp()
	{
		foreach(BuildingComp buildingComp in BuildingCompList)
		{
			if(buildingComp)
				Destroy(buildingComp.gameObject);
		}
		
		BuildingCompList.Clear();
	}

	// 添加一座新的建筑
	public void AddNewBuilding(BuildingData newBuildingData)
	{
		string prefabPath = "Building/Building_" + newBuildingData.Type.ToString() + "_" + newBuildingData.Level.ToString("D2");
		GameObject prefab = Resources.Load<GameObject>(prefabPath);
		if(prefab)
		{
			GameObject newBuildingGO = GameObject.Instantiate(prefab) as GameObject;
			newBuildingGO.transform.parent = transform;
			BuildingComp buildingComp = newBuildingGO.GetComponent<BuildingComp>();
			buildingComp.Data = newBuildingData;
			BuildingCompList.Add(buildingComp);
			
			RepositionBuilding(buildingComp);
        }
        else
        {
            Debug.LogWarning("找不到Prefab " + prefabPath);
        }
    }

	// 恢复错位建筑
	public void RepositionBuilding(BuildingComp building)
	{
		SensorComp sensorComp = SceneManager.Instance.SensorGroupComp.SensorCompDic[building.Data.SlotID];
		building.transform.position = sensorComp.transform.position;
	}

	public void RepositionAllBuildings()
	{
		foreach(BuildingComp buildingComp in BuildingCompList)
		{
			if(buildingComp)
				RepositionBuilding(buildingComp);
		}
	}
}

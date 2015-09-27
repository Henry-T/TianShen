using UnityEngine;
using System.Collections.Generic;

public class StaticVillage
{
	public static List<StaticVillageData> DataList;
	
	public static StaticVillageData GetTypeLevel(int level)
	{
		return DataList.Find(d=>d.Level == level);
	}

	public static bool HasGreatorLevel(int curLevel)
	{
		return DataList.Exists(d=>d.Level > curLevel);
	}

	// 计算村落等级
	public static int CalcLevel(List<BuildingData> buildingDataList)
	{
		float curExp = CalcExp(buildingDataList);
		int id = 0;
		for(id = 0; id < DataList.Count; id++)
		{
			if(DataList[id].Exp > curExp)
				break;
		}
		return DataList[id].Level;
	}

	// 计算村落经验
	public static float CalcExp(List<BuildingData> buildingDataList)
	{
		float exp = 0;
		foreach(BuildingData data in buildingDataList)
		{
			StaticBuildingData staticData = StaticBuilding.GetByTypeLevel(data.Type, data.Level, data.Sibling);
			if(staticData != null)
				exp += staticData.Exp;
		}
		return exp;
	}

	// 计算下级需要经验
	public static float CalcNextExp(List<BuildingData> buildingDataList)
	{
		int curLevel = CalcLevel(buildingDataList);
		int staticDataId = DataList.FindIndex(d=>d.Level == curLevel);
		if(staticDataId < DataList.Count - 1)
			return DataList[staticDataId + 1].Exp;
		else 
			return 99999;
	}

	// 计算村庄神力
	public static float CalcPower(List<BuildingData> buildingDataList)
	{
		float power = 0;
		foreach(BuildingData data in buildingDataList)
		{
			if(data.Type == EBuildingType.Lib)
			{
				StaticBuildingData staticData = StaticBuilding.GetByTypeLevel(data.Type, data.Level, data.Sibling);
				if(staticData != null)
					power += staticData.Value;
			}
        }
		return power;
    }
    
    // 计算村庄防御
	public static float CalcDefence(List<BuildingData> buildingDataList)
	{
		float defence = 0;
		foreach(BuildingData data in buildingDataList)
		{
			if(data.Type == EBuildingType.Tower)
			{
				StaticBuildingData staticData = StaticBuilding.GetByTypeLevel(data.Type, data.Level, data.Sibling);
				if(staticData != null)
					defence += staticData.Value;
            }
        }
		return defence;
    }
    
    public static void Initialize()
	{
		DataList = new List<StaticVillageData>();

		DataList.AddRange(new StaticVillageData[]{
			new StaticVillageData(){Level = 1, Storage = 300, Exp = 0},
			new StaticVillageData(){Level = 2, Storage = 400, Exp = 20},
			new StaticVillageData(){Level = 3, Storage = 500, Exp = 80},
			new StaticVillageData(){Level = 4, Storage = 600, Exp = 120},
			new StaticVillageData(){Level = 5, Storage = 700, Exp = 183},
			new StaticVillageData(){Level = 6, Storage = 800, Exp = 225},
			new StaticVillageData(){Level = 7, Storage = 900, Exp = 309},
			new StaticVillageData(){Level = 8, Storage = 1000, Exp = 390},
			new StaticVillageData(){Level = 9, Storage = 1100, Exp = 528},
			new StaticVillageData(){Level = 10, Storage = 1200, Exp = 636},
		});
	}
}

public class StaticVillageData
{
	public int Level;			// 等级
	public float Storage;		// 信仰储量
	public float Exp;			// 需要经验
}


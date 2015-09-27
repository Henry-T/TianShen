using UnityEngine;
using System.Collections.Generic;
using AVOSCloud;
using System.Threading.Tasks;
using System.Linq;

// 数据类 - 村庄
public class VillageData
{
	public int BUILDING_SLOTS = 12;

	// 用户ID
	public string UserID{
		get{return AVObject.Get<string>("UserID");}
		set{AVObject["UserID"] = value;}
	}

	// 名字
	public string Name{
		get{return AVObject.Get<string>("Name");}
		set{AVObject["Name"] = value;}
	}

	// 守护
	public int Defence{
		get{return AVObject.Get<int>("Defence");}
		set{AVObject["Defence"] = value;}
	}

	// 力量
	public int Power{
		get{return AVObject.Get<int>("Power");}
		set{AVObject["Power"] = value;}
	}

	// 诡术
	public int Trick{
		get{return AVObject.Get<int>("Trick");}
		set{AVObject["Trick"] = value;}
	}

	// 信仰
	public int Belief{
		get{return AVObject.Get<int>("Belief");}
		set{AVObject["Belief"] = value;}
	}

	// 关联数据
	public AVObject AVObject;
	public UserData OwnerData;
	public List<BuildingData> BuildingDataList = new List<BuildingData>();

	// 从AVObject拷贝数据
	public static VillageData CreateFromAVObject(AVObject obj)
	{
		VillageData villageData = new VillageData();
		villageData.AVObject = obj;
		return villageData;
	}

	// 获取下一个空槽位
	public int GetEmptySlot()
	{
		for(int i = 0; i < BUILDING_SLOTS; i++)
		{
			if(!BuildingDataList.Exists(d=>d.SlotID == i))
				return i;
		}
		return -1;
	}

	// 添加一个建筑物
	public void AddNewBuilding(BuildingData buildingData)
	{
		BuildingDataList.Add(buildingData);
	}
}

using UnityEngine;
using System.Collections.Generic;
using AVOSCloud;
using System.Threading.Tasks;
using System;
using System.Linq;

public class BuildingData
{
	// Key
	public string ObjectId 
	{
		get { return AVObject.ObjectId;}
	}

	// 用户ID
	public string UserID
	{
		get { return AVObject.Get<string>("UserID");}
		set { AVObject["UserID"]=value;}
	}

	// 类型
	public EBuildingType Type
	{
		get{return (EBuildingType)AVObject.Get<int>("Type");}
		set{AVObject["Type"]=(int)value;}
	}

	// 等级
	public int Level {
		get {return AVObject.Get<int>("Level");}
		set {AVObject["Level"]=value;}
	}

	// 同类排位
	public int Sibling {
		get {return AVObject.Get<int>("Sibling");}
		set {AVObject["Sibling"]=value;}
	}

	// 位置
	public int SlotID {
		get{ return AVObject.Get<int>("SlotID");}
		set{AVObject["SlotID"]=value;}
	}

	public DateTime CollectTime{
		get{return AVObject.Get<DateTime>("CollectTime");}
		set{AVObject["CollectTime"]=value;}
	}

	public AVObject AVObject;		// Avos数据实例

	public static int GetMaxSibling(List<BuildingData> buildingDataList, EBuildingType type)
	{
		int sibling = 0;

		foreach(BuildingData data in buildingDataList)
		{
			if(data.Type == type && data.Sibling > sibling)
			{
				sibling = data.Sibling;
			}
		}
		return sibling;
	}

	/// <summary>
	/// Gets the objectId value of building.
	/// </summary>
	/// <returns>The building object identifier.</returns>
	/// <param name="slotId">Slot identifier.</param>
	public static string GetBuildingObjectId(int slotId)
	{
		BuildingData buildingData = PlayerManager.Instance.PlayerBuildingDataList.Find(i => 
		    i.SlotID == slotId && i.UserID == AVUser.CurrentUser.ObjectId);

		if(buildingData != null)
			return buildingData.ObjectId;
		else
			return null;
	}

	public static BuildingData GetBuildingData(string objectId)
	{
		BuildingData obj = null;

		obj = PlayerManager.Instance.PlayerBuildingDataList.Find(
			i=>i.ObjectId == objectId);

		return obj;
	}
}

public enum EBuildingType
{
	Altar,		// 祭坛
	Church,		// 教堂
	Tower,		// 哨塔
	Lib,		// 学院
}

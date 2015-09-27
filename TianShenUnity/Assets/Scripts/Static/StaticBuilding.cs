using UnityEngine;
using System.Collections.Generic;

public class StaticBuilding
{
	// 硬编码
	// 生产建筑充满T对应等级 1min 2min 3min

	public static int MAX_SIBLING = 3;

	public static List<StaticBuildingData> DataList;

	public static StaticBuildingData GetByTypeLevel(EBuildingType type, int level, int sibling)
	{
		return DataList.Find(d=>d.Type == type && d.Level == level && d.Sibling == sibling);
	}

	public static bool HasGreatorLevel(EBuildingType type, int sibling, int curLevel)
	{
		return DataList.Exists(d=>d.Type == type && d.Sibling == sibling && d.Level > curLevel);
	}

	public static void Initialize()
	{
		DataList = new List<StaticBuildingData>();

		DataList.AddRange(new StaticBuildingData[]{
			// 神坛
			new StaticBuildingData(){Type = EBuildingType.Altar, Sibling = 1, Level = 1, VLevel = 1, Cost = 0, 	 Exp =  0, Value = 0},
			new StaticBuildingData(){Type = EBuildingType.Altar, Sibling = 1, Level = 2, VLevel = 2, Cost = 200, Exp = 20, Value = 0},
			new StaticBuildingData(){Type = EBuildingType.Altar, Sibling = 1, Level = 3, VLevel = 6, Cost = 600, Exp = 60, Value = 0},

			// 教会
			// 主属性-全周期产量
			new StaticBuildingData(){Type = EBuildingType.Church, Sibling = 1, Level = 1, VLevel = 1, Cost = 100, Exp = 10, Value = 50},
			new StaticBuildingData(){Type = EBuildingType.Church, Sibling = 1, Level = 2, VLevel = 2, Cost = 200, Exp = 20, Value = 100},
			new StaticBuildingData(){Type = EBuildingType.Church, Sibling = 2, Level = 1, VLevel = 3, Cost = 300, Exp = 30, Value = 50},

			new StaticBuildingData(){Type = EBuildingType.Church, Sibling = 2, Level = 2, VLevel = 4, Cost = 350, Exp = 35, Value = 100},
			new StaticBuildingData(){Type = EBuildingType.Church, Sibling = 1, Level = 3, VLevel = 5, Cost = 400, Exp = 40, Value = 150},
			new StaticBuildingData(){Type = EBuildingType.Church, Sibling = 3, Level = 1, VLevel = 6, Cost = 300, Exp = 35, Value = 100},

			new StaticBuildingData(){Type = EBuildingType.Church, Sibling = 3, Level = 2, VLevel = 7, Cost = 450, Exp = 45, Value = 100},
			new StaticBuildingData(){Type = EBuildingType.Church, Sibling = 2, Level = 3, VLevel = 8, Cost = 500, Exp = 50, Value = 150},
			new StaticBuildingData(){Type = EBuildingType.Church, Sibling = 3, Level = 3, VLevel = 9, Cost = 550, Exp = 55, Value = 150},

			// 哨塔
			// 主属性-防御
			new StaticBuildingData(){Type = EBuildingType.Tower, Sibling = 1, Level = 1, VLevel = 1, Cost = 100, Exp = 10, Value = 10},
			new StaticBuildingData(){Type = EBuildingType.Tower, Sibling = 1, Level = 2, VLevel = 2, Cost = 200, Exp = 20, Value = 20},
			new StaticBuildingData(){Type = EBuildingType.Tower, Sibling = 2, Level = 1, VLevel = 3, Cost = 300, Exp = 30, Value = 10},
			
			new StaticBuildingData(){Type = EBuildingType.Tower, Sibling = 2, Level = 2, VLevel = 4, Cost = 350, Exp = 35, Value = 20},
			new StaticBuildingData(){Type = EBuildingType.Tower, Sibling = 1, Level = 3, VLevel = 5, Cost = 400, Exp = 40, Value = 30},
			new StaticBuildingData(){Type = EBuildingType.Tower, Sibling = 3, Level = 1, VLevel = 6, Cost = 300, Exp = 35, Value = 10},
			
			new StaticBuildingData(){Type = EBuildingType.Tower, Sibling = 3, Level = 2, VLevel = 7, Cost = 450, Exp = 45, Value = 20},
			new StaticBuildingData(){Type = EBuildingType.Tower, Sibling = 2, Level = 3, VLevel = 8, Cost = 500, Exp = 50, Value = 30},
			new StaticBuildingData(){Type = EBuildingType.Tower, Sibling = 3, Level = 3, VLevel = 9, Cost = 550, Exp = 55, Value = 30},

			// 学院
			// 主属性-攻击
			new StaticBuildingData(){Type = EBuildingType.Lib, Sibling = 1, Level = 1, VLevel = 1, Cost = 100, Exp = 10, Value = 20},
			new StaticBuildingData(){Type = EBuildingType.Lib, Sibling = 1, Level = 2, VLevel = 2, Cost = 200, Exp = 20, Value = 40},
			new StaticBuildingData(){Type = EBuildingType.Lib, Sibling = 2, Level = 1, VLevel = 3, Cost = 300, Exp = 30, Value = 20},
			
			new StaticBuildingData(){Type = EBuildingType.Lib, Sibling = 2, Level = 2, VLevel = 4, Cost = 350, Exp = 35, Value = 40},
			new StaticBuildingData(){Type = EBuildingType.Lib, Sibling = 1, Level = 3, VLevel = 5, Cost = 400, Exp = 40, Value = 60},
			new StaticBuildingData(){Type = EBuildingType.Lib, Sibling = 3, Level = 1, VLevel = 6, Cost = 300, Exp = 35, Value = 20},
			
			new StaticBuildingData(){Type = EBuildingType.Lib, Sibling = 3, Level = 2, VLevel = 7, Cost = 450, Exp = 45, Value = 40},
			new StaticBuildingData(){Type = EBuildingType.Lib, Sibling = 2, Level = 3, VLevel = 8, Cost = 500, Exp = 50, Value = 60},
			new StaticBuildingData(){Type = EBuildingType.Lib, Sibling = 3, Level = 3, VLevel = 9, Cost = 550, Exp = 55, Value = 60},
		});
	}
}

public class StaticBuildingData
{
	public EBuildingType Type;	// 类型
	public int Sibling;			// 同Type时的数量排行
	public int Level;			// 等级
	public int VLevel;			// 村落等级要求
	public float Cost;			// 消耗信仰
	public float Exp;			// 增加经验
	public float Value;			// 主属性
}

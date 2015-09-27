using UnityEngine;
using System.Collections.Generic;

public class StaticFight 
{
	public static float DIZZY_TIME = 3;
	public static float SCHEDULE_FECTOR = 0.5f;

	public static List<StaticBattleStage> DataList;

	public static void Initialize()
	{
		DataList = new List<StaticBattleStage>();

		DataList.AddRange(new StaticBattleStage[]{
			new StaticBattleStage(){Time = 10, AppearingTime = 0.2f, AppearTime = 1f},
			new StaticBattleStage(){Time = 20, AppearingTime = 0.2f, AppearTime = 0.9f},
			new StaticBattleStage(){Time = 30, AppearingTime = 0.2f, AppearTime = 0.75f},
			new StaticBattleStage(){Time = 40, AppearingTime = 0.2f, AppearTime = 0.6f},
			new StaticBattleStage(){Time = 50, AppearingTime = 0.2f, AppearTime = 0.5f},
			new StaticBattleStage(){Time = 60, AppearingTime = 0.2f, AppearTime = 0.45f},
			new StaticBattleStage(){Time = 70, AppearingTime = 0.2f, AppearTime = 0.40f},
			new StaticBattleStage(){Time = 80, AppearingTime = 0.2f, AppearTime = 0.35f},
			new StaticBattleStage(){Time = 90, AppearingTime = 0.2f, AppearTime = 0.30f},
		});
	}
}


public class StaticBattleStage
{
	public float Time;				// 状态转换时间
	public float AppearingTime;		// 渐现时间
	public float AppearTime;		// 现身时常
}
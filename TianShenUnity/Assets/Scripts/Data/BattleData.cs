using UnityEngine;
using System.Collections;
using System;
using AVOSCloud;

public class BattleData
{
	public AVObject AVObject;

	// 守方ObjectID
	public string Defender{
		get{return AVObject.Get<string>("Defender");}
		set{AVObject["Defender"] = value;}
	}

	// 攻防ObjectID
	public string Invader{
		get{return AVObject.Get<string>("Invader");}
		set{AVObject["Invader"] = value;}
	}

	// 是否攻下
	public bool IsWin{
		get{return AVObject.Get<bool>("IsWin");}
		set{AVObject["Invader"] = value;}
	}
	
	// 战斗发生的时间
	public DateTime Time{
		get{return AVObject.Get<DateTime>("Time");}
		set{AVObject["Time"] = value;}
	}

	// 防御者名
	public string DefenderName;

	// 进攻者名
	public string InvaderName;

	void Start () {
		
	}

	void Update () {
		
	}
}
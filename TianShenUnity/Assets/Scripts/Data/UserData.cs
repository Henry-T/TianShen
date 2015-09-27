using UnityEngine;
using System.Collections;
using AVOSCloud;

public class UserData
{
	// 通常是构造后关联的实际AVUser数据，不保证一直同步
	public AVUser AVUser;
	
	public UserData()
	{

	}

	public UserData(AVUser avUser)
	{
		AVUser = avUser;
	}

	// 将当前UserData转换为一个AVUser && All data nice and fresh
	public AVUser ToAVUser()
	{
		// TODO
		return new AVUser();
	}
}

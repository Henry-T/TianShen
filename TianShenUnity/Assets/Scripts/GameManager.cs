using UnityEngine;
using System.Collections.Generic;
using AVOSCloud;
using System;

// 框架级功能
// NOTE 不要有游戏逻辑相关的东西
public class GameManager : MonoBehaviour 
{
	public static GameManager Instance {get; private set;}

	// 异步控制对象
	public class AsyncWaitObj
	{
		public bool AsyncWaiting;
		public Action AsyncWaitAction;
	}
	private AsyncWaitObj asyncWaitObj;

	// 准备开启工作线程
	// NOTE 此方法只能在主线程中调用
	public void AsyncBeginWait()
	{
		if(asyncWaitObj.AsyncWaiting && asyncWaitObj.AsyncWaitAction != null)
		{
			Debug.LogWarning("已经有一个异步等待事务,禁止重复调用!");
			return;
		}
		
		UIManager.Instance.WidgetWaiting.Show(true);
		lock(asyncWaitObj)
		{
			asyncWaitObj.AsyncWaiting = true;
			asyncWaitObj.AsyncWaitAction = null;
		}
	}

	// 将工作线程标记为已完成
	// NOTE 可在任何线程中调用
	public void AsyncEndWait(Action callback)
	{
		lock(asyncWaitObj)
		{
			asyncWaitObj.AsyncWaiting = false;
			asyncWaitObj.AsyncWaitAction = callback;
		}
	}


	private float sharedTimer = 0;
	private Action timerAction;

	// 共享计时器
	public void ScheduleTimerAction(float time, Action action)
	{
		sharedTimer = time;
		timerAction = action;
	}

	void Start () {
		Instance = this;

		asyncWaitObj = new AsyncWaitObj(){AsyncWaiting = false, AsyncWaitAction = null};

		// 初始化静态数据和计算公式
		StaticBuilding.Initialize();
		StaticVillage.Initialize();
		StaticFight.Initialize();
	}

	void Update () {
		// 共享计时器
		if(sharedTimer > 0)
		{
			sharedTimer -= Time.deltaTime;
			if(sharedTimer < 0)
			{
				timerAction();
				sharedTimer = 0;
			}
		}

		// 处理工作线程结果
		lock(asyncWaitObj)
		{
			if(!asyncWaitObj.AsyncWaiting && asyncWaitObj.AsyncWaitAction != null)
			{
				asyncWaitObj.AsyncWaiting = false;
				UIManager.Instance.WidgetWaiting.Show(false);

				// 到此行已认为等待结束 允许在前一次回调中设置新的等待回调
				Action oldCallback = asyncWaitObj.AsyncWaitAction;
				asyncWaitObj.AsyncWaitAction = null;
				if(oldCallback != null)
					oldCallback();
			}
		}
	}
}
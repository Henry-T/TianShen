using UnityEngine;
using System.Collections.Generic;

public class WidgetCloud : WidgetBase {

	public List<GameObject> Clouds;

	public void Start()
	{
		Clouds = new List<GameObject>();
		Clouds.Add (transform.FindChild("c1").gameObject);
		Clouds.Add (transform.FindChild("c2").gameObject);
		Clouds.Add (transform.FindChild("c3").gameObject);
		Clouds.Add (transform.FindChild("c4").gameObject);
	}

	public void OnGUI()
	{
		/*
		if(GUILayout.Button("In"))
		{
			foreach(GameObject c in Clouds)
			{
				c.GetComponent<Animator>().Play(c.name + "_in");
			}
		}

		if(GUILayout.Button("Out"))
		{
			foreach(GameObject c in Clouds)
			{
				c.GetComponent<Animator>().Play(c.name + "_out");
			}
		}

		if(GUILayout.Button("Circle"))
		{
			ScheduleSwitch(EAnimMode.InOut, EScreen.Build);
		}
		*/
	}

	public enum EAnimMode
	{
		In,
		InOut,
		Out,
	}

	float sharedTimer = 0;
	int stage = 0;

	float inTime = 1;
	float waitTime = 1;
	float outTime = 1;

	void Update()
	{
		if(stage == 1)
		{
			if(sharedTimer > inTime)
			{
				stage = 2;
				sharedTimer = 0;
			}
		}
		if(stage == 2)
		{
			if(sharedTimer > waitTime)
			{
				stage = 3;
				sharedTimer = 0;
				
				foreach(GameObject c in Clouds)
				{
					c.GetComponent<Animator>().Play(c.name + "_out");
				}
			}
		}
		if(stage == 3)
		{
			// ...
			UIManager.Instance.ChangeScreen(nextScreen);
		}
	}

	private EScreen nextScreen;
	public void ScheduleSwitch(EAnimMode animMode, EScreen nextScreen)
	{
		stage = 1;
		sharedTimer = 0;
		this.nextScreen = nextScreen;

		foreach(GameObject c in Clouds)
		{
			c.GetComponent<Animator>().Play(c.name + "_in");
		}
	}

	// TODO 配合Loading让玩家没有等待感
	public void PlayIn()
	{
		gameObject.SetActive(true);
		foreach(GameObject c in Clouds)
		{
			c.GetComponent<Animator>().Play(c.name + "_in");
		}
	}

	public void PlayOut()
	{
		foreach(GameObject c in Clouds)
		{
			c.GetComponent<Animator>().Play(c.name + "_out");
		}
	}
}

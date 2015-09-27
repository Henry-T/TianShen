using UnityEngine;
using System.Collections;

public class WidgetWaiting : WidgetBase {

	public override	void Initialize()
	{
		if(!initialized)
		{
			initialized = true;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Show(bool isShow)
	{
		gameObject.SetActive(isShow);
	}
}

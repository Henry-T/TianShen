using UnityEngine;
using System.Collections;

public class WidgetCancelMove : WidgetBase {

	void Start () {
	
	}

	void Update () {
	
	}

	public void OnButton_CancelMove()
	{
		// TODO 退出移动模式 关闭此层
		gameObject.SetActive(false);
		SceneManager.Instance.SceneComp_Build.SelectedBuilding = null;
	}
}

using UnityEngine;
using System.Collections;
using AVOSCloud;

public class WidgetBuildingControl : WidgetBase {

	public UIButton btnInfo;
	public UIButton btnUpgrade;

	// Use this for initialization
	void Start () {	
		Initialize();
	}

	public override void Initialize()
	{
		if(!initialized)
		{
			btnInfo = transform.FindChild("btnInfo").GetComponent<UIButton>();
			btnUpgrade = transform.FindChild("btnUpgrade").GetComponent<UIButton>();

			initialized = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Show()
	{
		Initialize();
		gameObject.SetActive(true);

		// TODO 默认跟随当前选中的建筑

	}

	public void Show(Transform targetTrans)
	{
		Show();
		
		// 设定此Widget位置
		Vector3 pos = Camera.main.WorldToScreenPoint(targetTrans.position);
		pos.z = 0f;
		Vector3 pos2 = UICamera.currentCamera.ScreenToWorldPoint(pos + new Vector3(0, -80, 0));
		transform.position = pos2;
	}

	public void OnButton_Info()
	{
		UIManager.Instance.WidgetBuildingInfo.Show();
		gameObject.SetActive(false);
	}

	public void OnButton_Move()
	{
		// Idea any是一种需要复查的状态，复查是一件容易遗忘，有容易因复查不全而造成错误的状况

		// TODO 进入移动模式 .. (任意... 是件很讨厌的事情 能否改进？)
		UIManager.Instance.WidgetCancelMove.gameObject.SetActive(true);
		UIManager.Instance.WidgetBuildingControl.gameObject.SetActive(false);

		// TODO 什么情况下退出移动模式？ 移动完成/UI层上提供唯一退出口，而屏蔽其他层 (这样的好处是将any限定在了一个易复查的位置上)
	}

	public void OnButton_Upgrade()
	{
		UIManager.Instance.WidgetYesNo.Show("是否升级", ()=>{
			// TODO 检查是否达到顶级

			// TODO 检查是否有足够的升级资源

			GameManager.Instance.AsyncBeginWait();

			BuildingData data = SceneManager.Instance.CurSceneComp.SelectedBuilding.Data;
			AVObject avData = data.AVObject;

			avData["Level"] = data.Level + 1;

			avData.SaveAsync().ContinueWith(t=>{
				if(t.IsCompleted)
				{
					data.Level++;
					GameManager.Instance.AsyncEndWait(()=>{
						SceneManager.Instance.CurSceneComp.RecreateAllBuildings();
					});
				}
				else
				{
					Debug.LogWarning("提交升级信息出错");
				}
			});
		}, ()=>{

		});
		gameObject.SetActive(false);
	}

	public void OnButton_AnyClose()
	{
		SceneManager.Instance.CurSceneComp.SelectedBuilding = null;
		gameObject.SetActive(false);
	}
}

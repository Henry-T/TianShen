using UnityEngine;
using System.Collections;

public class UIBuildPanel : WidgetBase {

	public UILabel lbLevel;
	public UILabel lbBelief;
	public UILabel lbPower;
	public UILabel lbExp;
	public UILabel lbDefence;

	public override void Initialize ()
	{
		if(!initialized)
		{
			lbLevel = transform.FindInChildren("lbLevel").GetComponent<UILabel>();
			lbBelief = transform.FindInChildren("lbBelief").GetComponent<UILabel>();
			lbPower = transform.FindInChildren("lbPower").GetComponent<UILabel>();
			lbExp = transform.FindInChildren("lbExp").GetComponent<UILabel>();
			lbDefence = transform.FindInChildren("lbDefence").GetComponent<UILabel>();

			initialized = true;
		}
	}

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		/*
		GUILayout.BeginArea(new Rect(Screen.width - 100, 0, 100, Screen.height));
		if(GUILayout.Button("升级1号位建筑"))
		{
			// TODO DB处理 ...
			BuildingData.DB_BuildingLevelUp(2);
		}

		if(GUILayout.Button("交换1号和3号"))
		{
			// TODO DB处理 ...
			BuildingData.DB_BuildingSlotChanged (2, 4);
		}
		GUILayout.EndArea();
		*/
	}
	
	public void Show()
	{
		PlayerManager.Instance.RecalcPlayerProperties();
		// 获取计算属性
		lbLevel.text = PlayerManager.Instance.PP_UserLevel.ToString("F0");
		lbBelief.text = PlayerManager.Instance.PP_UserBelief.ToString("F0");
		lbPower.text = PlayerManager.Instance.PP_UserPower.ToString("F0");
		lbExp.text = PlayerManager.Instance.PP_UserExp.ToString("F0");
		lbDefence.text = PlayerManager.Instance.PP_UserDefence.ToString("F0");
	}

	public void OnButton_OpenBuildPicker()
	{
		UIManager.Instance.WidgetBuildingPicker.Show();
	}

	public void OnButton_StartSearch()
	{
		UIManager.Instance.WidgetCloud.PlayIn();
		GameManager.Instance.ScheduleTimerAction(1.5f, ()=>{
			
			UIManager.Instance.WidgetCloud.PlayOut();

			// PlayerManager.Instance.TestBattleInitial();
			PlayerManager.Instance.NetGetRandomOther(()=>{
				SceneManager.Instance.SwitchScene(ESceneMode.Visit);
				UIManager.Instance.ChangeScreen(EScreen.Search);
			});
		});
	}

	public void OnButton_EventLog()
	{
		UIManager.Instance.WidgetEventLog.Show();
	}
}
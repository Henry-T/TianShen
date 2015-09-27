using UnityEngine;
using System.Collections.Generic;
using System;

public class UIManager : MonoBehaviour {

	public static UIManager Instance {get; private set;}
	
	public EScreen CurScreen = EScreen.Login;
	public Dictionary<EScreen, List<MonoBehaviour>> ScreenAgentMap = new Dictionary<EScreen, List<MonoBehaviour>>();
	
	public UIRegistPanel UIRegistPanel;
	public UILoginPanel UILoginPanel;
	public UIBuildPanel UIBuildPanel;
	public UISearchPanel UISearchPanel;
	public UIFightPanel UIFightPanel;

	public WidgetYesNo WidgetYesNo;
	public WidgetBuildingControl WidgetBuildingControl;
	public WidgetBuildingInfo WidgetBuildingInfo;
	public WidgetBuildingPicker WidgetBuildingPicker;
	public WidgetWaiting WidgetWaiting;
	public WidgetCloud WidgetCloud;
	public WidgetCancelMove WidgetCancelMove;
	public WidgetEventLog WidgetEventLog;

	void Start () {
		Instance = this;

		// 初始化 - Widget
		WidgetYesNo = transform.FindChild("WidgetYesNo").GetComponent<WidgetYesNo>();
		WidgetBuildingControl = transform.FindChild("WidgetBuildingControl").GetComponent<WidgetBuildingControl>();
		WidgetBuildingInfo = transform.FindChild("WidgetBuildingInfo").GetComponent<WidgetBuildingInfo>();
		WidgetBuildingPicker = transform.FindChild("WidgetBuildingPicker").GetComponent<WidgetBuildingPicker>();
		WidgetWaiting = transform.FindChild("WidgetWaiting").GetComponent<WidgetWaiting>();
		WidgetCloud = transform.FindChild("WidgetCloud").GetComponent<WidgetCloud>();
		WidgetCancelMove = transform.FindChild("WidgetCancelMove").GetComponent<WidgetCancelMove>();
		WidgetEventLog = transform.FindChild("WidgetEventLog").GetComponent<WidgetEventLog>();
		
		Action<WidgetBase> initWidget = comp=>{
			comp.Initialize();
			comp.gameObject.SetActive(false);
		};
		
		initWidget(WidgetYesNo);
		initWidget(WidgetBuildingControl);
		initWidget(WidgetBuildingInfo);
		initWidget(WidgetBuildingPicker);
		initWidget(WidgetWaiting);
		initWidget(WidgetCloud);
		initWidget(WidgetCancelMove);
		initWidget(WidgetEventLog);

		// 初始化 - Panel
		UIRegistPanel = transform.FindChild("UIRegistPanel").GetComponent<UIRegistPanel>();
		UILoginPanel = transform.FindChild("UILoginPanel").GetComponent<UILoginPanel>();
		UIBuildPanel = transform.FindChild("UIBuildPanel").GetComponent<UIBuildPanel>();
		UISearchPanel = transform.FindChild("UISearchPanel").GetComponent<UISearchPanel>();
		UIFightPanel = transform.FindChild("UIFightPanel").GetComponent<UIFightPanel>();

		ScreenAgentMap.Add(EScreen.Regist, new List<MonoBehaviour>(){UIRegistPanel});
		ScreenAgentMap.Add(EScreen.Login, new List<MonoBehaviour>(){UILoginPanel});
		ScreenAgentMap.Add(EScreen.Build, new List<MonoBehaviour>(){UIBuildPanel});
		ScreenAgentMap.Add(EScreen.Search, new List<MonoBehaviour>(){UISearchPanel});
		ScreenAgentMap.Add(EScreen.Fight, new List<MonoBehaviour>(){UIFightPanel});

		UIBuildPanel.Initialize();
		UIFightPanel.Initialize();
		
		ChangeScreen(EScreen.Login);
    }
    
    void Update () {
	
	}

	public void ChangeScreen(EScreen screen)
	{
		CurScreen = screen;

		HideAllScreen();
		foreach(MonoBehaviour agent in ScreenAgentMap[screen])
		{
			agent.gameObject.SetActive(true);

			if(screen== EScreen.Search && UISearchPanel.btnFight)
				UISearchPanel.btnFight.gameObject.SetActive(false);
		}
	}
	
	void HideAllScreen()
	{
		UIRegistPanel.gameObject.SetActive(false);
		UILoginPanel.gameObject.SetActive(false);
		UIBuildPanel.gameObject.SetActive(false);
		UISearchPanel.gameObject.SetActive(false);
		UIFightPanel.gameObject.SetActive(false);
    }
}


public enum EScreen
{
	Regist,
	Login,
	Build,
	Search,
	Fight,
}


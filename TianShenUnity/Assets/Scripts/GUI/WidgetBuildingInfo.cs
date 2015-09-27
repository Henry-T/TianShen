using UnityEngine;
using System.Collections;

public class WidgetBuildingInfo : WidgetBase {

	public UILabel lbType;
	public UILabel lbLevel;
	public UILabel lbValueType;
	public UILabel lbValue;
	public UILabel lbDesc;

	public UIButton btnOK;

	// Use this for initialization
	void Start () {
		Initialize();
	}

	public override void Initialize()
	{
		if(!initialized)
		{
			lbType = transform.FindChild("lbType").GetComponent<UILabel>();
			lbLevel = transform.FindChild("lbLevel").GetComponent<UILabel>();
			lbValueType = transform.FindChild("lbValueType").GetComponent<UILabel>();
			lbValue = transform.FindChild("lbValue").GetComponent<UILabel>();
			lbDesc = transform.FindChild("lbDesc").GetComponent<UILabel>();
			
			btnOK = transform.Find("btnOK").GetComponent<UIButton>();

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

		// TODO 默认显示SelectedBuilding的信息
	}

	public void OnButton_OK()
	{
		gameObject.SetActive(false);

		if(SceneManager.Instance.CurSceneMode == ESceneMode.Player)
			UIManager.Instance.WidgetBuildingControl.Show();
		else if(SceneManager.Instance.CurSceneMode == ESceneMode.Visit)
			SceneManager.Instance.CurSceneComp.SelectedBuilding = null;
	}
}

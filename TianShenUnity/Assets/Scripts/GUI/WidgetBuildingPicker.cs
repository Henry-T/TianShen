using UnityEngine;
using System.Collections;

public class WidgetBuildingPicker : WidgetBase {

	public UIButton btnBuildChurch;
	public UIButton btnBuildLib;
	public UIButton btnBuildTower;

	public UIButton btnClose;

	public UILabel lbCanChurch;
	public UILabel lbCanLib;
	public UILabel lbCanTower;

	void Start () {
		Initialize();
	}

	public override void Initialize()
	{
		if(!initialized)
		{
			btnBuildChurch = transform.FindChild("btnBuildChurch").GetComponent<UIButton>();
			btnBuildLib = transform.FindChild("btnBuildLib").GetComponent<UIButton>();
			btnBuildTower = transform.FindChild("btnBuildTower").GetComponent<UIButton>();

			btnClose = transform.FindChild("btnClose").GetComponent<UIButton>();

			lbCanChurch = transform.FindChild("lbCanChurch").GetComponent<UILabel>();
			lbCanLib = transform.FindChild("lbCanLib").GetComponent<UILabel>();
			lbCanTower = transform.FindChild("lbCanTower").GetComponent<UILabel>();

			initialized = true;
		}
	}

	void Update () {
	
	}

	public void Show()
	{
		Initialize();
		gameObject.SetActive(true);

		// 更新可建造状态 (是否达到建造上限)
		int maxSiblingChurch = BuildingData.GetMaxSibling(PlayerManager.Instance.PlayerBuildingDataList, EBuildingType.Church);
		if(maxSiblingChurch >= StaticBuilding.MAX_SIBLING)
		{
			lbCanChurch.text = "达到建造上限";
			lbCanChurch.color = Color.red;
			btnBuildChurch.SetState(UIButtonColor.State.Disabled, false);
			btnBuildChurch.isEnabled = false;
		}
		else
		{
			lbCanChurch.text = "可以建造";
			lbCanChurch.color = Color.green;
			btnBuildChurch.SetState(UIButtonColor.State.Normal, false);
			btnBuildChurch.isEnabled = true;
		}

		
		int maxSiblingLib = BuildingData.GetMaxSibling(PlayerManager.Instance.PlayerBuildingDataList, EBuildingType.Lib);
		if(maxSiblingLib >= StaticBuilding.MAX_SIBLING)
		{
			lbCanLib.text = "达到建造上限";
			lbCanLib.color = Color.red;
			btnBuildLib.SetState(UIButtonColor.State.Disabled, false);
			btnBuildLib.isEnabled = false;
		}
		else
		{
			lbCanLib.text = "可以建造";
			lbCanLib.color = Color.green;
			btnBuildLib.SetState(UIButtonColor.State.Normal, false);
			btnBuildLib.isEnabled = true;
		}
		
		int maxSiblingTower = BuildingData.GetMaxSibling(PlayerManager.Instance.PlayerBuildingDataList, EBuildingType.Tower);
		if(maxSiblingTower >= StaticBuilding.MAX_SIBLING)
		{
			lbCanTower.text = "达到建造上限";
			lbCanTower.color = Color.red;
			btnBuildTower.SetState(UIButtonColor.State.Disabled, false);
			btnBuildTower.isEnabled = false;
		}
		else
		{
			lbCanTower.text = "可以建造";
			lbCanTower.color = Color.green;
			btnBuildTower.SetState(UIButtonColor.State.Normal, false);
			btnBuildTower.isEnabled = true;
		}
	}

	public void OnButton_Close()
	{
		gameObject.SetActive(false);
	}

	public void OnButton_BuildChurch()
	{
		PlayerManager.Instance.NetBuildNew(EBuildingType.Church);
	}

	public void OnButton_BuildLib()
	{
		PlayerManager.Instance.NetBuildNew(EBuildingType.Lib);
	}

	public void OnButton_BuildTower()
	{
		PlayerManager.Instance.NetBuildNew(EBuildingType.Tower);
	}
}

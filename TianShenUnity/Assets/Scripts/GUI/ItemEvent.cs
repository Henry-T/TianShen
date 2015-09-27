using UnityEngine;
using System.Collections;
using AVOSCloud;

public class ItemEvent : MonoBehaviour {

	public BattleData Data;

	public UILabel lbBattleName;
	public UILabel lbBattleResult;
	public UIButton btnRevenge;

	void Start () {
		lbBattleName = transform.FindInChildren("lbBattleName").GetComponent<UILabel>();
		lbBattleResult = transform.FindInChildren("lbBattleResult").GetComponent<UILabel>();
		btnRevenge = transform.FindInChildren("btnRevenge").GetComponent<UIButton>();
	}

	void Update () {
		
	}

	public void Refresh(){
		if(PlayerManager.Instance.PlayerVillageData.UserID == Data.Invader)
		{
			lbBattleName.text = "你入侵了[ff0000]" + Data.DefenderName + "的村落";
		}
		else
		{
			lbBattleName.text = "[ff0000]" + Data.InvaderName + "入侵了你的村落";
		}

		if(Data.IsWin)
		{
			lbBattleResult.text = "胜利";
		}
		else
		{
			lbBattleResult.text = "失败";
		}
	}

	public void OnButton_Revenge()
	{
		// TODO 复仇 
		Debug.LogWarning("点击了复仇");

		// string name = "3853dc5206e4b0bcb4f57de7d8";
		PlayerManager.Instance.NetGetOtherByID("3853dc5206e4b0bcb4f57de7d8", ()=>{
			UIManager.Instance.WidgetEventLog.gameObject.SetActive(false);
			UIManager.Instance.ChangeScreen(EScreen.Search);
		});
	}
}

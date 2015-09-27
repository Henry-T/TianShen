using UnityEngine;
using System.Collections.Generic;
using AVOSCloud;
using System.Threading.Tasks;

public class UISearchPanel : MonoBehaviour {

	public UILabel lbMatchBeliefAll;
	public UILabel lbMatchUserName;
	public UIButton btnFight;

	// Use this for initialization
	void Start () {
		lbMatchBeliefAll = transform.FindChild("lbMatchBeliefAll").GetComponent<UILabel>();
		lbMatchUserName = transform.FindChild("lbMatchUserName").GetComponent<UILabel>();
		btnFight = transform.FindChild("btnFight").GetComponent<UIButton>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnButton_Search()
	{
		PlayerManager.Instance.NetGetRandomOther(()=>{
			SceneManager.Instance.SwitchScene(ESceneMode.Visit);
			UIManager.Instance.UISearchPanel.RefreshOtherInfo();
		});
	}

	public void OnButton_Fight()
	{
		UIManager.Instance.WidgetCloud.PlayIn();
		GameManager.Instance.ScheduleTimerAction(1.5f, ()=>{
			
			UIManager.Instance.WidgetCloud.PlayOut();
			
			// 启动战斗UI
			UIManager.Instance.ChangeScreen(EScreen.Fight);

			// 切换到战斗场景
			SceneManager.Instance.SwitchScene(ESceneMode.Battle);
			SceneManager.Instance.SceneComp_Battle.StartFight();
		});
    }
    
	public void RefreshOtherInfo()
	{
		lbMatchBeliefAll.text = PlayerManager.Instance.OtherVillageData.Belief.ToString();
		lbMatchUserName.text = PlayerManager.Instance.OtherUserData.AVUser.Username;

		btnFight.gameObject.SetActive(true);
	}
}

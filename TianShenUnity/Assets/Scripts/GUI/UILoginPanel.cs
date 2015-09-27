using UnityEngine;
using System.Collections;

public class UILoginPanel : MonoBehaviour {

	public UIInput tbUserName;
	public UIInput tbPassword;

	// Use this for initialization
	void Start () {
		tbUserName = transform.FindChild("tbUserName").GetComponent<UIInput>();
		tbPassword = transform.FindChild("tbPassword").GetComponent<UIInput>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DoRegist()
	{
		TestData.DB_UpdateTestData("test update is ok or not?");
		UIManager.Instance.ChangeScreen(EScreen.Regist);
	}

	public void DoLogin()
	{
		GameManager.Instance.AsyncBeginWait();
		PlayerManager.Instance.NetDoLogin(tbUserName.label.text, tbPassword.label.text, ()=>{
			GameManager.Instance.AsyncEndWait(()=>{
				// 登陆时拉取村落数据
				PlayerManager.Instance.NetQueryPlayerVillageData(()=>{
					SceneManager.Instance.SwitchToPlayerScene();
					UIManager.Instance.ChangeScreen(EScreen.Build);
					UIManager.Instance.UIBuildPanel.Show();
					UIManager.Instance.WidgetCloud.PlayOut();
				});
			});
		});
	}

	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(Screen.width/2 - 50, 0, 100, 30));
		GUILayout.BeginHorizontal();
		GUILayout.Label("账号: ");
		if(PlayerManager.Instance.PlayerVillageData != null)
			GUILayout.Label(PlayerManager.Instance.PlayerVillageData.Name);
		GUILayout.EndHorizontal();
		GUILayout.EndArea();

		if(GUILayout.Button("战斗测试"))
		{
			PlayerManager.Instance.TestBattle();
		}	
	}
}

using UnityEngine;
using System.Collections;

// 敌人Actor控制器
public class EnemyComp : MonoBehaviour {

	public static float TO_SHOW_TIME = 0.2f;
	public static float SHOW_TIME = 0.4f;
	public static float TO_HIDE_TIME = 0.2f;
	public static float HIDE_TIME = 0.4f;

	public enum EEnemyState
	{
		Show,			// 显示
		ShowToHide,		// 渐隐
		Hide,			// 隐藏
		HideToShow,		// 渐现
	}
	public EEnemyState State;

	public float ShareTimer = 0;

	private Renderer renderer;

	public int CurSlot = -1;

	void Start () {
		renderer = GetComponentInChildren<Renderer>();
		gameObject.SetActive(false);
	}

	public void Restart()
	{
		State = EEnemyState.Hide;
		ShareTimer = 0;
		gameObject.SetActive(true);
	}

	void Update () {
		if(SceneManager.Instance.SceneComp_Battle.BattleState != EBattleState.BattleOn)
			gameObject.SetActive(false);

		ShareTimer += Time.deltaTime;

		if(State == EEnemyState.Hide)
		{
			if(ShareTimer > HIDE_TIME)
			{
				int id = Random.Range(0, SceneManager.Instance.CurSceneComp.BuildingCompList.Count - 1);
				BuildingComp buildingComp = SceneManager.Instance.CurSceneComp.BuildingCompList[id];

				if(buildingComp)
				{
					transform.position = buildingComp.transform.position + Vector3.up * 1;

					State = EEnemyState.HideToShow;
					ShareTimer = 0;
					CurSlot = buildingComp.Data.SlotID;

					//animation["HideToShow"].normalizedSpeed = TO_SHOW_TIME;
					//animation.Play("HideToShow");
				}
			}
		}
		else if(State == EEnemyState.HideToShow)
		{
			if(ShareTimer > TO_SHOW_TIME)
			{
				State = EEnemyState.Show;
				ShareTimer = 0;

				GameObject prefab = Resources.Load<GameObject>("Effects/fireBallBase");
				GameObject effect = Instantiate(prefab) as GameObject;
				effect.AddComponent<MoveToCamera>();
				effect.transform.parent = SceneManager.Instance.EffectRootComp.transform;
				effect.transform.position = transform.position;
			}
			else
			{
				renderer.material.color = new Color(1,1,1, ShareTimer / TO_SHOW_TIME);
			}
		}
		else if(State == EEnemyState.Show)
		{
			if(ShareTimer > SHOW_TIME)
			{
				State = EEnemyState.ShowToHide;
				ShareTimer = 0;

				//animation["HideToShow"].normalizedSpeed = TO_HIDE_TIME;
				//animation.Play("ShowToHide");	
			}
		}
		else if(State == EEnemyState.ShowToHide)
		{
			if(ShareTimer > TO_HIDE_TIME)
			{
				State = EEnemyState.Hide;
				ShareTimer = 0;
			}
			else
			{
				renderer.material.color = new Color(1,1,1, 1-ShareTimer / TO_SHOW_TIME);
			}
		}
	}
}

using UnityEngine;
using System.Collections;

// 3D战场管理器
// 当前场景必然是 - 他人场景
public class SceneComp_Battle : SceneComp
{
	public EBattleState BattleState = EBattleState.NotInBattle;

	public int HitCount = 0;

	public void StartFight()
	{
		UIManager.Instance.ChangeScreen(EScreen.Fight);
		BattleState = EBattleState.BattleOn;
		HitCount = 0;
		SceneManager.Instance.EnemyGroupComp.EnemyComp.Restart();
	}
	
	public void EndFight()
	{
		BattleState = EBattleState.NotInBattle;
		UIManager.Instance.WidgetCloud.PlayIn();
		GameManager.Instance.ScheduleTimerAction(1.5f, ()=>{
			UIManager.Instance.WidgetCloud.PlayOut();
			SceneManager.Instance.SwitchToPlayerScene();
		});
		UIManager.Instance.ChangeScreen(EScreen.Build);
	}
}

public enum EBattleState
{
	NotInBattle,
	BattleWait,
	BattleOn,
	BattleOver,
}

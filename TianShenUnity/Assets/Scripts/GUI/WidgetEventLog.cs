using UnityEngine;
using System.Collections;
using AVOSCloud;

public class WidgetEventLog : WidgetBase {

	public UIButton btnClose;

	public UIScrollView scrEvents;

	public UIWidget tplEvent;

	void Start () {
		btnClose = transform.FindInChildren("btnBack").GetComponent<UIButton>();
		scrEvents = transform.FindInChildren("scrEvent").GetComponent<UIScrollView>();
		tplEvent = transform.FindInChildren("itemEvent").GetComponent<UIWidget>();
		tplEvent.gameObject.SetActive(false);
		tplEvent.transform.parent = null;
	}

	void Update () {
	
	}

	public void Show()
	{
		gameObject.SetActive(true);
		RefreshEvents();
	}
	
	public void OnButton_Close()
	{
		gameObject.SetActive(false);
	}

	// 添加一个事件UI项
	public void AddEvent(BattleData data)
	{
		GameObject itemEventObj = NGUITools.AddChild(scrEvents.gameObject, tplEvent.gameObject);
		 
		itemEventObj.SetActive(true);
		ItemEvent itemEvent = itemEventObj.GetComponent<ItemEvent>();
		itemEvent.Data = data;
		itemEvent.transform.parent = scrEvents.transform;

		scrEvents.ResetPosition();
		scrEvents.UpdatePosition();
		scrEvents.GetComponent<UIGrid>().Reposition();
	}

	// 刷新所有事件
	public void RefreshEvents()
	{
		foreach(BattleData data in PlayerManager.Instance.BattleDataList)
		{
            AddEvent(data);
        }
	}
}

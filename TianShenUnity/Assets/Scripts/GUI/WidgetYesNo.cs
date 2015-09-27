using UnityEngine;
using System.Collections;
using System;

public class WidgetYesNo : WidgetBase {

	public UIButton btnYes;
	public UIButton btnNo;

	public UILabel lbInfo;

	public Action OnYes;
	public Action OnNo;

	// Use this for initialization
	void Start () {
		Initialize();
	}

	public override void Initialize()
	{
		if(!initialized)
		{
			lbInfo = transform.FindChild("lbInfo").GetComponent<UILabel>();
			btnYes = transform.FindChild("btnYes").GetComponent<UIButton>();
			btnNo = transform.FindChild("btnNo").GetComponent<UIButton>();

			initialized = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  public void Show(string info, Action onYes, Action onNo)
  {
		Initialize();

    lbInfo.text = info;
    OnYes = onYes;
    OnNo = onNo;
    gameObject.SetActive(true);
  }

  public void OnButton_Yes()
  {
    if(OnYes != null)
      OnYes();
    gameObject.SetActive(false);
  }

  public void OnButton_No()
  {
    if(OnNo != null)
      OnNo();
    gameObject.SetActive(false);
  }
}

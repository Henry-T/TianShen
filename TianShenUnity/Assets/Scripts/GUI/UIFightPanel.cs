using UnityEngine;
using System.Collections;

public class UIFightPanel : WidgetBase {

	public UISprite barEnergy;
	public UISprite barDefence;

	private float baseScaleEnergy;
	private float baseScaleDefence;

	public float EnergyValue = 100;
	public float DefenceValue = 100;

	public override void Initialize ()
	{
		if(!initialized)
		{
			barEnergy = transform.FindChild("barEnergy").GetComponent<UISprite>();
			barDefence = transform.FindChild("barDefence").GetComponent<UISprite>();

			baseScaleEnergy = barEnergy.transform.localScale.x;
			baseScaleDefence = barDefence.transform.localScale.x;

			initialized = true;
		}
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DesEnergy(float val)
	{
		EnergyValue -= val;
		
		Vector3 _scale = barEnergy.transform.localScale;
		barEnergy.transform.localScale = new Vector3(baseScaleEnergy * EnergyValue / 100, _scale.y, _scale.z );		
	}

	public void DesDefence(float val)
	{
		DefenceValue -= val;
		
		Vector3 _scale = barDefence.transform.localScale;
		barDefence.transform.localScale = new Vector3(baseScaleDefence * DefenceValue / 100, _scale.y,  _scale.z );		
	}
}

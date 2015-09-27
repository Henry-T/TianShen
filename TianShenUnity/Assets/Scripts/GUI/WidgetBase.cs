using UnityEngine;
using System.Collections;

public class WidgetBase : MonoBehaviour {
	
	protected bool initialized;

	public virtual void Initialize()
	{
		initialized = true;
	}
}

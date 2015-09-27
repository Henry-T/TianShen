using UnityEngine;
using System.Collections;

public class TouchWidget : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseDown()
	{
		Debug.LogWarning("Down");
		//screenPoint = Camera.main.WorldToScreenPoint(scanPos);
		
		//offset = scanPos - Camera.main.ScreenToWorldPoint(
		//	new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}
	
	
	void OnMouseDrag()
	{
		Debug.LogWarning("Dragging");
		//Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		
		//Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
		//transform.position = curPosition;
	}
}

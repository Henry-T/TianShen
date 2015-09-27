using UnityEngine;
using System.Collections;

public class SunComp : MonoBehaviour 
{
	public GenerateBeliefData GenerateBeliefData;

	void Start () {
		transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
	}

	void Update () {
		// 持续更新尺寸
		float scale = (GenerateBeliefData.CurrentValue - GenerateBeliefData.MaxValue / 2f) / (GenerateBeliefData.MaxValue /2f) * 0.2f + 0.8f;
		transform.localScale = new Vector3(scale, scale, scale);
	}

	void OnClick(){
		// TODO 删除这个太阳 / 播放太阳回收动画

		// TODO 向服务器提交转换请求

		// TODO 成功时清空本地状态
	}
}

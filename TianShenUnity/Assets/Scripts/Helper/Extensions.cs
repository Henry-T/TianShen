using UnityEngine;
using System.Collections;

public static class Extensions
{
	public static Transform FindInChildren(this Transform transform, string name)
	{
		if( transform.name == name)
			return transform;
		Transform find = transform.Find(name);
		if(find)
			return find;
		else
		{
			foreach(Transform child in transform)
			{
				find = child.FindInChildren(name);
				if(find)
					return find;
			}
			return null;
		}
	}
}

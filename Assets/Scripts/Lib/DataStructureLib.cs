using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DataStructureLib
{
	public static T Choice<T>(List<T> list) {
		return list[Random.Range(0, list.Count)];
	}
}
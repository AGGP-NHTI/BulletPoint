using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataObject : MonoBehaviour
{
	private void Awake()
	{
		Debug.LogError($"[{gameObject.name}] DataObjects should not exist in the scene");
	}
}

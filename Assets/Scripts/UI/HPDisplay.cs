using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPDisplay : MonoBehaviour
{
	public Image image;
	public TMP_Text text;

	public void UpdatePosition(Vector3 worldPosition)
	{
		image.rectTransform.position = Camera.main.WorldToScreenPoint(worldPosition + Vector3.up * 4);
	}

	public void UpdateHP(int value, int maxvalue)
	{
		if (value <= 0) value = 0;

		image.fillAmount = (float)value / maxvalue;
		text.text = $"{value}/{maxvalue}";
	}

	public void Remove()
	{
		Destroy(gameObject);
	}
}

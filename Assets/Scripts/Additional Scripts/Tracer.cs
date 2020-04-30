using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracer : MonoBehaviour
{
	public AnimationCurve curve;
	public LineRenderer line;
	public float lifetime = 4;

	float life = 0;
	
    void Update()
    {
		line.widthMultiplier = curve.Evaluate(life / (float)lifetime);

		life += Time.deltaTime;
		if (life >= lifetime)
		{
			Destroy(gameObject);
		}
    }
}

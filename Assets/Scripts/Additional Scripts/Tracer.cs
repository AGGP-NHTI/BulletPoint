using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracer : MonoBehaviour
{
	public AnimationCurve curve;
	public LineRenderer line;
	public short lifetime = 4;

	short life = 0;
	
    void Update()
    {
		line.widthMultiplier = curve.Evaluate(life / (float)lifetime);

		life++;
		if (life >= lifetime)
		{
			Destroy(gameObject);
		}
    }
}

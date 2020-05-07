using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_Script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.Translate(transform.TransformDirection(Vector3.zero));
    }
}

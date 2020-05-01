using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelScript : MonoBehaviour
{

    public Transform transf;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("SET POSITION");
        transf.SetPositionAndRotation(Vector3.zero, transf.rotation);
    }
}

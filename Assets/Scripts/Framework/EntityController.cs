using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{

    

    protected GameObject _obj;
    protected Transform _transf;
    protected Rigidbody _rb;

    protected virtual void  Awake()
    {
        _obj = gameObject;
        _transf = gameObject.GetComponent<Transform>();
        _rb = gameObject.GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }



    protected void LOG(string input)
    {
        Debug.Log(input);
    }
}

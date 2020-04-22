using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : EntityController
{
    public EntityController owner;

    public float lifetime = 5f;
    public float speed = 10f;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        Destroy(_obj, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        PlayerManager player = other.GetComponent<PlayerManager>();
        if (player)
        {
            player.takeDamage(2);
        }
        if (!enemy)
        {
            Destroy(_obj);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadButton = UnityEngine.InputSystem.LowLevel.GamepadButton;

public abstract class Weapons : EntityController
{
	public Vector3 PlacedRotation;
	public Vector3 PlacedScale;
	public Vector3 OwnedScale;
	public float defaultHeight = 2.5f;

	public int damage = 10;
	public float coolDownDuration = 0.5f;

	public GameObject tracerPrefab;

	public bool takes_Continuous_Input;
	public Collider trigger;

	protected bool coolingDown = false;

	GameObject parent;


	private void Update()
	{
		if (Game.player.weaponOwned == this)
		{
			Game.player.Drop(this);
		}
	}

    protected virtual void OnTriggerStay(Collider other)
    {
        PlayerManager player = other.GetComponent<PlayerManager>();
        if (player)
        {
            Game.player.PickUp(this);
        }
    }

		if (player)
		{
			Game.player.PickUp(this);
		}
	}

	public abstract void Use();



	protected IEnumerator coolDown(float coolDownTime)
	{
		yield return new WaitForSeconds(coolDownTime);

		coolingDown = false;
	}

	protected void bulletTrail(Vector3 origin, Vector3 direction, float magnitude)
	{
		GameObject tracer = Instantiate(tracerPrefab, origin, Quaternion.identity);
		Vector3 endPoint = origin + (direction.normalized * magnitude);
		tracer.GetComponent<LineRenderer>().SetPosition(1, direction * magnitude);
	}

}

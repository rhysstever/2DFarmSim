using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public UnityGameObjectEvent pickupEvent;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnCollisionEnter(Collision collision)
	{
        GameObject collidedGO = collision.gameObject;
        // If the player is colliding with the ground, end early
        if(collidedGO.tag == "Ground")
            return;

        // The player is slowed if they are in water
        if(collidedGO.tag == "Water")
		{
            transform.parent.GetComponent<Movement>().isSlowed = true;
            return;
		}

        pickupEvent.Invoke(collidedGO);
        collidedGO.SetActive(false);
	}

	private void OnCollisionExit(Collision collision)
	{
        GameObject collidedGO = collision.gameObject;
        // If the player is not colliding with water, end early
        if(collidedGO.tag != "Water")
            return;

        // The player is no longer slowed
        transform.parent.GetComponent<Movement>().isSlowed = false;
    }
}

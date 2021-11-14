using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHandler : MonoBehaviour
{
    public UnityGameObjectEvent pickupEvent;
    private Rigidbody rBody;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnCollisionEnter(Collision collision)
	{
        GameObject collidedGO = collision.gameObject;
        // If the object that was collided into is the ground or water, end early
        if(collidedGO.tag == "Water" || collidedGO.tag == "Ground")
            return;
        pickupEvent.Invoke(collidedGO);
        collidedGO.SetActive(false);
	}
}

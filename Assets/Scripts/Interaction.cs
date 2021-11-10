using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    // Set at Start()
    public GameObject currentInteractable;
    
    // Start is called before the first frame update
    void Start()
    {
        currentInteractable = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void FixedUpdate()
	{
        currentInteractable = FindInteractable();
    }

    /// <summary>
    /// Raycasts ahead of the player to see if there is anything to interact with
    /// </summary>
    /// <returns>A raycast hit object, or null</returns>
    GameObject FindInteractable()
	{
        // ===== Unity's Raycast Script =====

        // Create a layerMask that includes everything except layer 7 (the Player layer)
        int layerMask = 1 << 7;
        layerMask = ~layerMask;

        RaycastHit hit;
        Vector3 forward = transform.parent.GetComponent<Movement>().transform.forward;
        Vector3 direction = forward.normalized;
        direction += Vector3.down;
        // Does the ray intersect any objects excluding the player layer
        if(Physics.Raycast(transform.position, transform.TransformDirection(direction), out hit, Mathf.Infinity, layerMask))
        {
            GameObject hitObject = hit.transform.gameObject;

            // Gets the parent-most object, stopping early if the parent is a manager
            while(hitObject.transform.parent != null && hitObject.transform.parent.tag != "ParentObj")
            {
                hitObject = hitObject.transform.parent.gameObject;
            }

            // Return null if the ground was hit
            // Otherwise return the hit object
            if(hitObject.tag == "Ground")
                return null;
            else
                return hitObject;
        }
        
        return null;
    }
}

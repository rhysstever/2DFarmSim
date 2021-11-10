using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject currentInteractable;
    public GameObject currentItem;
    
    private Transform playerTrans;

    // Start is called before the first frame update
    void Start()
    {
        currentInteractable = null;
        currentItem = null; // the player starts with nothing in hand

        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
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
        // Modified but still the core

        // Create a layerMask that includes everything except layer 7 (the Player layer)
        int layerMask = 1 << 7;
        layerMask = ~layerMask;

        RaycastHit hit;
        Vector3 forward = playerTrans.parent.GetComponent<Movement>().transform.forward;
        Vector3 direction = forward.normalized;
        direction += Vector3.down;
        // Does the ray intersect any objects excluding the player layer
        if(Physics.Raycast(playerTrans.position, playerTrans.TransformDirection(direction), out hit, Mathf.Infinity, layerMask))
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

    /// <summary>
    /// Sets a new current item
    /// </summary>
    /// <param name="newItem">The new item</param>
    public void PickupItem(GameObject newItem) {
        if(currentItem != null)
            Debug.Log("Hands are full! Discard current item.");
        else
            currentItem = newItem;
	}

    /// <summary>
    /// Removes the current item from the player
    /// </summary>
    /// <returns>The current item</returns>
    public GameObject RemoveCurrentItem() {
        GameObject temp = currentItem;
        currentItem = null;
        return temp;
	}
}

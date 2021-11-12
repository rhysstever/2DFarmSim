using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // Set at Start()
    public GameObject currentInteractable;
    public GameObject currentItem;

    // Based on keyboard number locations
    // (1,2,3,...,9,0) for indecies 0->9
    private int currentItemSlot;    
    private Transform playerTrans;

    // Start is called before the first frame update
    void Start()
    {
        currentInteractable = null;
        currentItemSlot = 1;    // the "1" number key correlates to the 0th index
        currentItem = GetComponent<Inventory>().GetInventoryItem(currentItemSlot);

        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Get current item from the inventory
        currentItem = GetComponent<Inventory>().GetInventoryItem(currentItemSlot);

        // Get user input
        string input = Input.inputString;
        if(!string.IsNullOrEmpty(input))
            ParseInput(input);
    }

    void FixedUpdate()
    {
        currentInteractable = FindInteractable();
    }

    /// <summary>
    /// Parses user input
    /// </summary>
    /// <param name="input">The input (key, button, etc) pressed</param>
    private void ParseInput(string input)
	{
        // Handle number key presses
        if(int.TryParse(input, out int numInput))
        {
            // If the input is able to be parsed into a number,
            // change the current item slot to that number
            currentItemSlot = numInput;
            return;
		}

        // Standard letter key input
        switch(input.ToUpper())
        {
            case "Q":
                RemoveCurrentItem();
                break;
            case "E":
                if(currentInteractable != null)
                {
                    if(currentInteractable.tag == "FarmPlot")
                        currentInteractable.GetComponent<FarmPlot>().Interact(currentItem);
                    else if(currentInteractable.GetComponent<Item>() != null ||
                        currentInteractable.tag == "Water")
                        PickupItem(currentInteractable);
                }
                break;
            default:
                break;
        }
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
    /// Adds an item to the inventory
    /// </summary>
    /// <param name="newItem">The new item</param>
    public void PickupItem(GameObject newItem) {
        GetComponent<Inventory>().AddToInventory(newItem);
	}

    /// <summary>
    /// Removes the current item from the player
    /// </summary>
    /// <returns>The current item</returns>
    public GameObject RemoveCurrentItem() {
        return GetComponent<Inventory>().RemoveFromInventory(currentItemSlot);
	}
}

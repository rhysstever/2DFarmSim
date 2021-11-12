using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityGameObjectEvent : UnityEvent<GameObject>
{
}

public class ItemManager : MonoBehaviour
{
    // Set at Start()
    public GameObject currentInteractable;
    public GameObject currentItem;  // Updated in Update()

    public UnityGameObjectEvent pickupEvent = new UnityGameObjectEvent();
    public UnityEvent removeEvent = new UnityEvent();

    // Based on keyboard number locations
    // (1,2,3,...,9,0) for indecies 0->9
    private int currentItemSlot;    

    // Start is called before the first frame update
    void Start()
    {
        currentInteractable = null;
        currentItemSlot = 1;    // the "1" number key correlates to the 0th index
        ChangeSelectedItem(1);

        pickupEvent.AddListener(PickupItem);
        removeEvent.AddListener(RemoveCurrentItem);
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

        ScrollThroughInventory();
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
            ChangeSelectedItem(numInput);
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
                        GetComponent<GameManager>().plotsParent.GetComponent<PlotManager>().Interact(currentInteractable, currentItem, pickupEvent, removeEvent);
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
        Transform playerTrans = GetComponent<GameManager>().player.transform;

        // Create a layerMask that includes everything except layer 7 (the Player layer)
        int layerMask = 1 << 7;
        layerMask = ~layerMask;

        RaycastHit hit;
        Vector3 forward = playerTrans.forward;
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
    public void RemoveCurrentItem() {
        GetComponent<Inventory>().RemoveFromInventory(currentItemSlot);
        return;
	}

    /// <summary>
    /// Changes the current item being selected
    /// </summary>
    private void ScrollThroughInventory()
	{
        // Scrolling forward
        if(Input.GetAxis("Mouse ScrollWheel") > 0f) 
            ChangeSelectedItem(currentItemSlot - 1);
        // Scrolling backwards
        else if(Input.GetAxis("Mouse ScrollWheel") < 0f)
            ChangeSelectedItem(currentItemSlot + 1);
    }

    /// <summary>
    /// Called whenever the current selected item slot is changed
    /// </summary>
    /// <param name="newIndex">The index of the newly selected item</param>
    private void ChangeSelectedItem(int newSlot)
	{
        // Ensures the new item slot is within bounds
        currentItemSlot = newSlot;
        if(currentItemSlot == -1)
            currentItemSlot = 9;
        else if(currentItemSlot == 10)
            currentItemSlot = 0;

        // Updates the UI
        int index = GetComponent<Inventory>().SlotToIndex(currentItemSlot);
        GetComponent<UIManager>().UpdateSelectedItemUI(index);
    }
}

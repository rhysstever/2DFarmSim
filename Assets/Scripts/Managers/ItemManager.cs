using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject currentItem;

    // Start is called before the first frame update
    void Start()
    {
        currentItem = null; // the player starts with nothing in hand
    }

    // Update is called once per frame
    void Update()
    {
        
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
    public GameObject RemoveItem() {
        GameObject temp = currentItem;
        currentItem = null;
        return temp;
	}
}

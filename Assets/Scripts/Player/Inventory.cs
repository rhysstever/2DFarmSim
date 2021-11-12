using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject[] inventory;
    public GameObject[,] backpack;

    // Start is called before the first frame update
    void Start()
    {
        inventory = new GameObject[10];
        backpack = new GameObject[10, 4];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Gets an item at the given inventory slot
    /// </summary>
    /// <param name="slot">The slot to be checked for an item</param>
    /// <returns>The item at that inventory slot</returns>
    public GameObject GetInventoryItem(int slot)
	{
        int correctIndex = SlotToIndex(slot);
        if(correctIndex == -1)
            return null;
        else
            return inventory[correctIndex];
	}

    /// <summary>
    /// Adds an item to the inventory
    /// </summary>
    /// <param name="newObject">The item to be added</param>
    public void AddToInventory(GameObject newObject)
	{
        int inventorySlot = NextAvailableSlot();
        if(inventorySlot == -1)
		{
            Debug.Log("Inventory Full");
            return;
		}
		else
		{
            int index = SlotToIndex(inventorySlot);
            inventory[index] = newObject;
            GetComponent<UIManager>().AddImageToInventory(newObject, index);
		}
	}

    /// <summary>
    /// Removes an item from the inventory
    /// </summary>
    /// <param name="slot">The slot of the item being removed</param>
    /// <returns>The removed item</returns>
    public GameObject RemoveFromInventory(int slot)
    {
        // Convert the slot to an index and confirm it is a valid index
        int index = SlotToIndex(slot);
        if(index == -1)
            return null;

        GameObject removedObject = inventory[index];
        inventory[index] = null;
        GetComponent<UIManager>().RemoveImageFromInventory(index);
        return removedObject;
	}

    /// <summary>
    /// A helper method that converts a slot number to an index
    /// </summary>
    /// <param name="slot">The numbers on a keyboard (1, 2, 3, ..., 9, 0)</param>
    /// <returns>A valid index (0->9)</returns>
    public int SlotToIndex(int slot)
    {
        // Ensure slot is between 0 and 9
        if(slot < 0 || slot > 9)
            return -1;

        if(slot == 0)
            return 9;
        else
            return slot - 1;
    }

    /// <summary>
    /// A helper method that converts an index to a slot
    /// </summary>
    /// <param name="index">An index of the inventory array (0->9)</param>
    /// <returns>A number at the top of the keyboard (1, 2, 3, ..., 9, 0)</returns>
    public int IndexToSlot(int index)
    {
        // Ensure index is between 0 and 9
        if(index < 0 || index > 9)
            return -1;

        if(index == 9)
            return 0;
        else
            return index + 1;
    }

    /// <summary>
    /// A helper method that finds the next open slot in the inventory
    /// </summary>
    /// <returns>The next open inventory slot, or -1 if the inventory is full</returns>
    private int NextAvailableSlot()
    {
        for(int i = 0; i < inventory.Length; i++)
        {
            if(inventory[i] == null)
                return IndexToSlot(i);
        }

        return -1;
    }

}

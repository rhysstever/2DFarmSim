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

    public void AddToInventory(GameObject newObject)
	{
        int inventorySlot = NextAvailableSlot();
        if(inventorySlot == -1)
            return;
        else
            inventory[inventorySlot] = newObject;
	}

    private int NextAvailableSlot()
	{
        for(int i = 0; i < inventory.Length; i++)
		{
            if(inventory[i] == null)
                return i;
		}

        return -1;
	}

    public GameObject RemoveFromInventory(int slot)
	{
        GameObject removedObject = inventory[slot];
        inventory[slot] = null;
        return removedObject;
	}
}

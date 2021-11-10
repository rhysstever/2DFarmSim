using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CropType
{
    Empty,
    Blueberry
}

public enum GrowthState
{
    None,
    Planted,
    GrowingStart,
    GrowingMid,
    FullyGrown
}

public class FarmPlot : MonoBehaviour
{
    // Set in inspector

    // Set at Start()
    public GameObject currentCrop;
    public GrowthState currentState;
    public float growTimer; 
    public float totalTimeToGrow;

    // Start is called before the first frame update
    void Start()
    {
        currentCrop = null;
        currentState = GrowthState.None;
        growTimer = 0.0f;
        totalTimeToGrow = 0.0f;
    }

	void FixedUpdate() {
        if(currentState == GrowthState.GrowingStart
            || currentState == GrowthState.GrowingMid)
            Grow();
    }

	// Update is called once per frame
	void Update()
    {
        
    }

    /// <summary>
    /// Called once when the player interacts with the plot
    /// </summary>
    /// <param name="currentItem">The item the player is holding</param>
    public void Interact(GameObject currentItem)
	{
        switch(currentState) {
            case GrowthState.None:
                if(currentItem != null &&
                    currentItem.tag == "Seeds") {
                    Plant(currentItem.GetComponent<Seeds>().crop);
                    GameObject.Find("gameManager").GetComponent<GameManager>().player.GetComponent<ItemManager>().RemoveCurrentItem();
				}
                else
                    Debug.Log("You need seeds in hand!");
                break;
            case GrowthState.Planted:
                if(currentItem != null &&
                    currentItem.tag == "Water") {
                    Water();
                    GameObject.Find("gameManager").GetComponent<GameManager>().player.GetComponent<ItemManager>().RemoveCurrentItem();
                }
                else
                    Debug.Log("You need water!");
                break;
            case GrowthState.GrowingStart:
                break;
            case GrowthState.GrowingMid:
                break;
            case GrowthState.FullyGrown:
                GameObject.Find("gameManager").GetComponent<GameManager>().player.GetComponent<ItemManager>().PickupItem(Harvest());
                break;
        }
	}

    /// <summary>
    /// Plants the crop on the plot
    /// </summary>
    private void Plant(GameObject crop)
	{
        // Adds the crop, updates the plot state, and resets the timer
        currentCrop = crop;
        currentState = GrowthState.Planted;
        growTimer = 0.0f;
        totalTimeToGrow = crop.GetComponent<Crop>().timeToGrow;
    }

    /// <summary>
    /// Waters the crop on the plot
    /// </summary>
    private void Water() 
    {
        // Updates state of the plot
        currentState = GrowthState.GrowingStart;
    }

    /// <summary>
    /// Runs a timer until the crop is fully grown
    /// </summary>
    private void Grow()
	{
        // Calculate what percent grown the crop is
        float growPercent = growTimer / totalTimeToGrow;

        // Checks if the crop is halfway grown
        switch(growPercent) {
            // 50% growth
            case float perc when perc >= 0.5f:
                currentState = GrowthState.GrowingMid;
                break;
        }

        // Increment timer
        growTimer += Time.deltaTime / totalTimeToGrow;

        // Checks if the crop is fully grown
        if(growPercent >= 1.0f) {
            currentState = GrowthState.FullyGrown;
            return;
        }
    }

    /// <summary>
    /// Harvests the crop of the plot
    /// </summary>
    /// <returns>The grown crop</returns>
    private GameObject Harvest()
	{
        GameObject grownCrop = currentCrop;
        // Removes the crop and plot state
        currentCrop = null;
        currentState = GrowthState.None;
        return grownCrop;
    }
}

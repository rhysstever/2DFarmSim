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

    // An event that updates the plot's materials when the growth state changes
    public UnityPlotMatEvent matUpdateEvent;

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
    /// Plants the crop on the plot
    /// </summary>
    public void Plant(GameObject crop)
	{
        // Adds the crop, updates the plot state, and resets the timer
        currentCrop = crop;
        currentState = GrowthState.Planted;
        growTimer = 0.0f;
        totalTimeToGrow = crop.GetComponent<Crop>().timeToGrow;
        matUpdateEvent.Invoke(transform, currentState);
    }

    /// <summary>
    /// Waters the crop on the plot
    /// </summary>
    public void Water() 
    {
        // Updates state of the plot
        currentState = GrowthState.GrowingStart;
        matUpdateEvent.Invoke(transform, currentState);
    }

    /// <summary>
    /// Runs a timer until the crop is fully grown
    /// </summary>
    public void Grow()
	{
        // Calculate what percent grown the crop is
        float growPercent = growTimer / totalTimeToGrow;

        // Checks if the crop is halfway grown
        switch(growPercent) {
            // 50% growth
            case float perc when perc >= 0.5f:
                currentState = GrowthState.GrowingMid;
                matUpdateEvent.Invoke(transform, currentState);
                break;
        }

        // Increment timer
        growTimer += Time.deltaTime / totalTimeToGrow;

        // Checks if the crop is fully grown
        if(growPercent >= 1.0f) {
            currentState = GrowthState.FullyGrown;
            matUpdateEvent.Invoke(transform, currentState);
            return;
        }
    }

    /// <summary>
    /// Harvests the crop of the plot
    /// </summary>
    /// <returns>The grown crop</returns>
    public GameObject Harvest()
	{
        GameObject grownCrop = currentCrop;
        // Removes the crop and plot state
        currentCrop = null;
        currentState = GrowthState.None;
        matUpdateEvent.Invoke(transform, currentState);
        return grownCrop;
    }
}

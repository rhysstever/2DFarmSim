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
    public string plotName;
    public GameObject currentCrop;
    public GrowthState currentGrowthState;
    public float growTimer; 
    public float totalTimeToGrow;
    public float growPercent;

    // An event that updates the plot's materials when the growth state changes
    // Set in PlotManager.cs
    public UnityPlotMatEvent matUpdateEvent;

    private GameObject cropGrowthGO;

    // Start is called before the first frame update
    void Start()
    {
        UpdateName();
        currentCrop = null;
        currentGrowthState = GrowthState.None;
        growTimer = 0.0f;
        totalTimeToGrow = 0.0f;
        growPercent = 0.0f;

        cropGrowthGO = null;
    }

	void FixedUpdate() {
        if(currentGrowthState == GrowthState.GrowingStart
            || currentGrowthState == GrowthState.GrowingMid)
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
        currentGrowthState = GrowthState.Planted;
        growTimer = 0.0f;
        totalTimeToGrow = crop.GetComponent<Crop>().timeToGrow;
        growPercent = 0.0f;

        // Update the growing gameObj on the plot in the scene
        UpdateGrowthGameObject();
        UpdateName();
        // Invoke the update material event
        matUpdateEvent.Invoke(transform, currentGrowthState);
    }

    /// <summary>
    /// Waters the crop on the plot
    /// </summary>
    public void Water() 
    {
        // Updates state of the plot
        currentGrowthState = GrowthState.GrowingStart;
        // Invoke the update material event
        UpdateName();
        matUpdateEvent.Invoke(transform, currentGrowthState);
    }

    /// <summary>
    /// Runs a timer until the crop is fully grown
    /// </summary>
    public void Grow()
	{
        // Calculate what percent grown the crop is
        growPercent = growTimer / totalTimeToGrow;
        growPercent *= 100.0f;
        UpdateName();

        // Checks if the crop is halfway grown
        switch(growPercent) {
            // 50% growth
            case float perc when perc >= 50.0f:
                currentGrowthState = GrowthState.GrowingMid;
                // Update the growing gameObj on the plot in the scene
                UpdateGrowthGameObject();
                // Invoke the update material event
                matUpdateEvent.Invoke(transform, currentGrowthState);
                break;
        }

        // Increment timer
        growTimer += Time.deltaTime / totalTimeToGrow;

        // Checks if the crop is fully grown
        if(growPercent >= 100.0f) {
            currentGrowthState = GrowthState.FullyGrown;
            growPercent = 100.0f;
            // Update the growing gameObj on the plot in the scene
            UpdateGrowthGameObject();
            UpdateName();
            // Invoke the update material event
            matUpdateEvent.Invoke(transform, currentGrowthState);
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
        currentGrowthState = GrowthState.None;
        growPercent = 0.0f;
        // Update the growing gameObj on the plot in the scene
        UpdateGrowthGameObject();
        UpdateName();
        // Invoke the update material event
        matUpdateEvent.Invoke(transform, currentGrowthState);
        return grownCrop;
    }

    /// <summary>
    /// Destroys the current growth gameObject and replaces it with the correct one based on the growth state
    /// </summary>
    private void UpdateGrowthGameObject()
	{
        // Kill the current gameObject on the plot
        Destroy(cropGrowthGO);

        // Find the new gameObject based on the growth state of the plot/crop
        GameObject growthGameObj = null;
        switch(currentGrowthState)
        {
            case GrowthState.None:
                return;
            case GrowthState.Planted:
            case GrowthState.GrowingStart:
                growthGameObj = currentCrop.GetComponent<Crop>().planted;
                break;
            case GrowthState.GrowingMid:
                growthGameObj = currentCrop.GetComponent<Crop>().growingMid;
                break;
            case GrowthState.FullyGrown:
                growthGameObj = currentCrop.GetComponent<Crop>().fullyGrown;
                break;
        }

        // Calculate the position of the new gameObject
        Vector3 spawnPos = transform.position;
        spawnPos.y += transform.Find("dirt").localScale.y;

        // Create the new gameObject and store it
        cropGrowthGO = Instantiate(growthGameObj, spawnPos, Quaternion.identity, transform);
    }

    /// <summary>
    /// Updates the name of the plot based on the current growth state
    /// </summary>
    public void UpdateName()
	{
        switch(currentGrowthState)
        {
            case GrowthState.None:
                plotName = "Empty Plot";
                break;
            case GrowthState.Planted:
                plotName = currentCrop.GetComponent<Crop>().cropType + " Plot (unwatered)";
                break;
            case GrowthState.GrowingStart:
            case GrowthState.GrowingMid:
                plotName = currentCrop.GetComponent<Crop>().cropType + " Plot (" + (int)growPercent + "%)";
                break;
            case GrowthState.FullyGrown:
                plotName = currentCrop.GetComponent<Crop>().cropType + " Plot (Grown)";
                break;
        }
	}
}

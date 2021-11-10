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
    Growing,
    FullyGrown
}

public class FarmPlot : MonoBehaviour
{
    // Set in inspector
    public Material noCropMat;
    public Material plantedMat;
    public Material growingStartMat;
    public Material growingMidMat;
    public Material grownMat;

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
        transform.Find("progress").GetComponent<MeshRenderer>().material = noCropMat;
    }

	void FixedUpdate() {
        if(currentState == GrowthState.Growing)
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
                    GameObject.Find("gameManager").GetComponent<GameManager>().player.GetComponent<ItemManager>().RemoveItem();
				}
                else
                    Debug.Log("You need seeds in hand!");
                break;
            case GrowthState.Planted:
                if(currentItem != null &&
                    currentItem.tag == "Water") {
                    Water();
                    GameObject.Find("gameManager").GetComponent<GameManager>().player.GetComponent<ItemManager>().RemoveItem();
                }
                else
                    Debug.Log("You need water!");
                break;
            case GrowthState.Growing:
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
        // Adds the crop, updates the plot state, resets the timer, and updates the material
        currentCrop = crop;
        currentState = GrowthState.Planted;
        growTimer = 0.0f;
        totalTimeToGrow = crop.GetComponent<Crop>().timeToGrow;
        UpdateMaterial(plantedMat);
    }

    /// <summary>
    /// Waters the crop on the plot
    /// </summary>
    private void Water() 
    {
        // Updates state and material of the plot
        currentState = GrowthState.Growing;
        UpdateMaterial(growingStartMat);
    }

    /// <summary>
    /// Runs a timer until the crop is fully grown
    /// </summary>
    private void Grow()
	{
        // Calculate what percent grown the crop is
        float growPercent = growTimer / totalTimeToGrow;

        // Gets the correct material based on how much the crop has grown
        Material currentMat = growingStartMat;
        switch(growPercent) {
            case float perc when perc >= 0.5f:
                // 50% growth
                currentMat = growingMidMat;
                break;
        }

        // Increment timer and apply material
        growTimer += Time.deltaTime / totalTimeToGrow;
        transform.Find("progress").GetComponent<MeshRenderer>().material = currentMat;

        // Checks if the crop is fully grown
        if(growPercent >= 1.0f) {
            currentState = GrowthState.FullyGrown;
            UpdateMaterial(grownMat);
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
        // Removes the crop, plot state, and updates material
        currentCrop = null;
        currentState = GrowthState.None;
        UpdateMaterial(noCropMat);
        return grownCrop;
    }

    /// <summary>
    /// Helper function to update the material of the plot rim
    /// </summary>
    private void UpdateMaterial(Material newMat)
	{
        transform.Find("progress").GetComponent<MeshRenderer>().material = newMat;
    }
}

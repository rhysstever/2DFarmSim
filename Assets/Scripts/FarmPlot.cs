using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CropType
{
    Empty,
    Crop
}

public enum GrowthState
{
    None,
    Planted, 
    Sprouting,
    Growing,
    FullyGrown
}

public class FarmPlot : MonoBehaviour
{
    public CropType crop;
    public GrowthState currentTier;
    public Material currentMat;
    public Material[] tierMats;

    // Start is called before the first frame update
    void Start()
    {
        crop = CropType.Empty;
        currentTier = GrowthState.None;
        currentMat = tierMats[(int)currentTier];
        gameObject.transform.Find("progress").GetComponent<MeshRenderer>().material = currentMat;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(
            GameObject.Find("GameManager").GetComponent<PlotManager>().player.transform.position,
            gameObject.transform.position)
            < GameObject.Find("GameManager").GetComponent<PlotManager>().nearbyPlotDist
            && Input.GetKeyDown(KeyCode.E)) {
            Interact();
        }
    }

    /// <summary>
    /// Interact with the plot
    /// </summary>
    void Interact()
	{
        switch(currentTier) {
            case GrowthState.None:
                Plant();
                break;
            case GrowthState.FullyGrown:
                Harvest();
                break;
            default:
                Grow();
                break;
        }
	}

    /// <summary>
    /// Plants a crop on the plot
    /// </summary>
    void Plant()
	{
        crop = CropType.Crop;
        currentTier = GrowthState.Planted;

        UpdateMat(1);
        Debug.Log("Planted " + crop);
    }

    /// <summary>
    /// Increments the GrowthState of the plot
    /// </summary>
    void Grow()
	{
        int tierInt = (int)currentTier;
        tierInt++;
        currentTier = (GrowthState)tierInt;

        UpdateMat(tierInt);
        // Debug.Log("Growth to state: " + currentTier);
	}

    /// <summary>
    /// Harvests the current crop of the plot
    /// </summary>
    void Harvest()
	{
        UpdateMat(0);
        Debug.Log("Harvested " + crop);
        
        crop = CropType.Empty;
        currentTier = GrowthState.None;
    }

    /// <summary>
    /// Helper function to update the material of the plot rim
    /// </summary>
    /// <param name="matIndex"></param>
    void UpdateMat(int matIndex)
	{
        currentMat = tierMats[matIndex];
        gameObject.transform.Find("progress").GetComponent<MeshRenderer>().material = currentMat;
    }
}

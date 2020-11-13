using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlotType
{

}

public enum GrowthState
{
    Empty,
    Planted, 
    Sprouting,
    Growing,
    FullyGrown
}

public class FarmPlot : MonoBehaviour
{
    public GrowthState currentTier;
    public Material currentMat;
    public Material[] tierMats;

    // Start is called before the first frame update
    void Start()
    {
        currentTier = GrowthState.Planted;
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
            && Input.GetKeyDown(KeyCode.E))
            Grow();
    }

    /// <summary>
    /// Up's the Grow State of the plot to the next tier
    /// </summary>
    void Grow()
	{
        // === Update GrowthState ===
        // if its already at the end growth, then Harvest is called instead
        if(currentTier == GrowthState.FullyGrown) {
            Harvest();
            return;
		}
        
        // "increment" the enum value
        int tierInt = (int)currentTier;
        tierInt++;
        currentTier = (GrowthState)tierInt;

        // === Update Material ===
        currentMat = tierMats[tierInt];
        gameObject.transform.Find("progress").GetComponent<MeshRenderer>().material = currentMat;

        // Debug.Log("Growth to state: " + currentTier);
	}

    void Harvest()
	{
        if(currentTier != GrowthState.FullyGrown)
            return;

        // Updates plot GrowthState to be empty
        currentTier = GrowthState.Empty;

        // Updates Material
        currentMat = tierMats[0];
        gameObject.transform.Find("progress").GetComponent<MeshRenderer>().material = currentMat;
        Debug.Log("Harvested!");
    }
}

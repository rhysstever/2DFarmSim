using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityPlotMatEvent : UnityEvent<Transform, GrowthState>
{
}

public class PlotManager : MonoBehaviour
{
    // === Set in inspector ===
    // Crop Materials
    public Material noCropMat;          // White
    public Material plantedMat;         // Red
    public Material growingStartMat;    // Orange
    public Material growingMidMat;      // Yellow
    public Material grownMat;           // Green

    // Watered Materials
    public Material unwateredMat;       // Tan
    public Material wateredMat;         // Brown

    // Set at Start()


    // Start is called before the first frame update
    void Start()
    {
        // For every child plot, set its materials to be empty
        // and create the event to be invoked later by the plot itself
        foreach(Transform childTrans in transform)
		{
            ChangeMat(childTrans, GrowthState.None);
            childTrans.gameObject.GetComponent<FarmPlot>().matUpdateEvent = new UnityPlotMatEvent();
            childTrans.gameObject.GetComponent<FarmPlot>().matUpdateEvent.AddListener(ChangeMat);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Interact(GameObject plot, GameObject item, UnityGameObjectEvent pickupEvent, UnityEvent removeEvent)
	{
        // Exit early if the plot does not exist or is not a plot
        if(plot == null || plot.GetComponent<FarmPlot>() == null)
            return;

        // Interact with the plot based on the plot's current growth state
        switch(plot.GetComponent<FarmPlot>().currentState)
        {
            case GrowthState.None:
                // Check if the held item is a seed
                if(item != null && item.tag == "Seeds")
                {
                    // Plant the seed in the plot and remove it from the player's inventory
                    plot.GetComponent<FarmPlot>().Plant(item.GetComponent<Seeds>().crop);
                    removeEvent.Invoke();
                }
                else
                    Debug.Log("You need seeds in hand!");
                break;
            case GrowthState.Planted:
				// Water the plot if the held item is water
				if(item != null && item.tag == "Water")
                {
                    plot.GetComponent<FarmPlot>().Water();
                    removeEvent.Invoke();
                }
                else
                    Debug.Log("You need water!");
                break;
            case GrowthState.GrowingStart:
                break;
            case GrowthState.GrowingMid:
                break;
            case GrowthState.FullyGrown:
                // Pickup the Harvestable item
                pickupEvent.Invoke(plot.GetComponent<FarmPlot>().Harvest());
                break;
        }
    }

    public void ChangeMat(Transform plotObj, GrowthState growState)
	{
        switch(growState)
        {
            case GrowthState.None:
                plotObj.Find("progress").GetComponent<MeshRenderer>().material = noCropMat;
                plotObj.Find("dirt").GetComponent<MeshRenderer>().material = unwateredMat;
                break;
            case GrowthState.Planted:
                plotObj.Find("progress").GetComponent<MeshRenderer>().material = plantedMat;
                break;
            case GrowthState.GrowingStart:
                plotObj.Find("progress").GetComponent<MeshRenderer>().material = growingStartMat;
                plotObj.Find("dirt").GetComponent<MeshRenderer>().material = wateredMat;
                break;
            case GrowthState.GrowingMid:
                plotObj.Find("progress").GetComponent<MeshRenderer>().material = growingMidMat;
                break;
            case GrowthState.FullyGrown:
                plotObj.Find("progress").GetComponent<MeshRenderer>().material = grownMat;
                plotObj.Find("dirt").GetComponent<MeshRenderer>().material = unwateredMat;
                break;
        }
    }
}

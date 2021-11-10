using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        foreach(Transform childTrans in transform)
		{
            ChangeMat(childTrans, GrowthState.None);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Inefficient but there are few plots atm, so its cheap
        foreach(Transform childTrans in transform)
        {
            ChangeMat(childTrans, childTrans.gameObject.GetComponent<FarmPlot>().currentState);
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

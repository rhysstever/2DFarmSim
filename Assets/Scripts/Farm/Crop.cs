using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    // === Set in inspector ===
    public float value;
    public CropType cropType;
    public float timeToGrow;

    // GameObjects for growth
    public GameObject planted;
    public GameObject growingMid;
    public GameObject fullyGrown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

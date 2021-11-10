using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public GameObject currentInteractable;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Interaction interactionComp = transform.GetComponentInChildren<Interaction>();
        currentInteractable = interactionComp.currentInteractable;
    }
}

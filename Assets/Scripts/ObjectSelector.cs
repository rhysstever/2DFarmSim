using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    // Set in inspector
    public float range;

    // Set at Start()
    public GameObject currentObj;

    // Start is called before the first frame update
    void Start()
    {
        currentObj = null;
    }

    // Update is called once per frame
    void Update()
    {
        FindObjectWithinRange();
    }

    /// <summary>
    /// Finds an object if it within range
    /// </summary>
    private void FindObjectWithinRange() {
        // Create layerMask and invert it to mask only the UI layer 
        int layerMask = 1 << 5;
        layerMask = ~layerMask;

        // Create a ray from the player's camera
        Ray ray = transform.GetComponentInChildren<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;

        // Does the ray intersect any objects excluding the UI layer
        if(Physics.Raycast(ray, out rayHit, range, layerMask)) {
            // Capture the hit gameObject
            GameObject hitGO = rayHit.transform.gameObject;

            // Makes sure the hitGO is the parent (never a child)
            do {
                if(hitGO.transform.parent != null
                    && hitGO.transform.parent.tag != "ParentObj")
                    hitGO = hitGO.transform.parent.gameObject;
            } while(hitGO.tag == "Untagged" &&
                hitGO.transform.parent != null);

            // Assigns the hit object to the current object,
            // unless it is the ground
            if(hitGO.name == "ground")
                currentObj = null;
            else 
                currentObj = hitGO;
        }
        else {
            currentObj = null;
        }
    }
}

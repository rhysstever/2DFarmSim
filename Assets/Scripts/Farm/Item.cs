using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public Texture image;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.GetComponent<Rigidbody>() == null)
            rb = gameObject.AddComponent<Rigidbody>();
        else
            rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

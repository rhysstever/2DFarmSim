using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Texture image;

    // Start is called before the first frame update
    void Start()
    {
        string imagePath = "Images/" + name;
        image = Resources.Load<Texture>(imagePath);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

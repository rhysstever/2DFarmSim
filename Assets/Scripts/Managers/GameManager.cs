using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject plotsParent;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.E)) {
            GameObject currentObj = player.GetComponent<PlayerInfo>().currentInteractable;
            GameObject currentItem = player.GetComponent<ItemManager>().currentItem;
            if(currentObj != null) {
                if(currentObj.tag == "FarmPlot")
                    currentObj.GetComponent<FarmPlot>().Interact(currentItem);
                else if(currentObj.GetComponent<Item>() != null ||
                    currentObj.tag == "Water")
                    player.GetComponent<ItemManager>().PickupItem(currentObj);
            }
        } else if(Input.GetKeyDown(KeyCode.Q)) {
            // Q removes the current item
            player.GetComponent<ItemManager>().RemoveItem();
		}
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	// === Set in inspector ===
	public Canvas canvas;

	[SerializeField]
	public GameObject gameUI;

	[SerializeField]
	public GameObject currentInteractablePanel;

	[SerializeField]
	public GameObject inventoryUI;

	[SerializeField]
	public GameObject inventoryPanel;

	[SerializeField]
	public GameObject backpackPanel;

	// Set at Start()

	// Start is called before the first frame update
	void Start()
	{
		SetupUI();
	}

	// Update is called once per frame
	void Update()
	{
		UpdateUI();
	}

	private void SetupUI()
	{
		// Initially the backpack should not be visible
		backpackPanel.SetActive(false);

		// Hide all item image children of each inventory slot
		// (the player starts with nothing)
		foreach(Transform childPanel in inventoryPanel.transform)
			childPanel.transform.GetChild(0).gameObject.SetActive(false);
	}

	private void UpdateUI()
	{
		// Update the Current Interactable panel with the name of the object
		GameObject currentInteractable = GetComponent<ItemManager>().currentInteractable;
		if(currentInteractable == null)
			currentInteractablePanel.SetActive(false);	// deactivate if there is no interactable
		else
		{
			// Activacte and set the proper name
			currentInteractablePanel.SetActive(true);
			string name = currentInteractable.name;
			if(currentInteractable.GetComponent<FarmPlot>() != null)
				name = currentInteractable.GetComponent<FarmPlot>().plotName;
			else if(currentInteractable.GetComponent<Item>() != null)
				name = currentInteractable.GetComponent<Item>().itemName;
			currentInteractablePanel.GetComponentInChildren<Text>().text = " " + name;
		}

		// Tab - Toggles the backpack
		if(Input.GetKeyDown(KeyCode.Tab))
		{
			// Toggle backpack panel 
			backpackPanel.SetActive(!backpackPanel.activeInHierarchy);
			// Set the player's ability to move based on if the panel is open (opposite from each other)
			GetComponent<GameManager>().player.GetComponent<Movement>().canMove = !backpackPanel.activeInHierarchy;
		}
	}

	/// <summary>
	/// Adds an item's image to the inventory panel
	/// </summary>
	/// <param name="gameObj">The object being added to the inventory</param>
	/// <param name="index">The index it is being added at</param>
	public void AddImageToInventory(GameObject gameObj, int index)
	{
		// Check if the object is an item
		if(gameObj == null || gameObj.GetComponent<Item>() == null)
			return;

		// Activate image gameObj active (child of inventory slot panel) and add the raw image to it
		GameObject imageGO = inventoryPanel.transform.GetChild(index).GetChild(0).gameObject;
		imageGO.SetActive(true);
		imageGO.GetComponent<RawImage>().texture = gameObj.GetComponent<Item>().image;
	}

	/// <summary>
	/// Removes an item's image from the inventory panel
	/// </summary>
	/// <param name="index">The child panel's index that is being cleared</param>
	public void RemoveImageFromInventory(int index)
	{
		// Remove raw image from the image gameObj (child of inventory slot panel) and deactivate it
		GameObject imageGO = inventoryPanel.transform.GetChild(index).GetChild(0).gameObject;
		imageGO.GetComponent<RawImage>().texture = null;
		imageGO.SetActive(false);
	}

	/// <summary>
	/// Activates the correct item selected image based on the given index
	/// </summary>
	/// <param name="index">The index of the currently selected item</param>
	public void UpdateSelectedItemUI(int index)
	{
		// Deactivates all selectedItem images
		foreach(Transform childPanel in inventoryPanel.transform)
			childPanel.transform.GetChild(1).gameObject.SetActive(false);

		// Activates the selectedItem image of the currently selected item
		inventoryPanel.transform.GetChild(index).transform.GetChild(1).gameObject.SetActive(true);
	}
}

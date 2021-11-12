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
	public GameObject inventoryUI;

	[SerializeField]
	public GameObject inventoryPanel;

	[SerializeField]
	public GameObject backpackPanel;

	// Set at Start()
	private Texture waterImage;

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

		// Hide all children of each inventory slot
		foreach(Transform childPanel in inventoryPanel.transform)
		{
			childPanel.transform.GetChild(0).gameObject.SetActive(false);
		}
	}

	private void UpdateUI()
	{
		// Tab - Toggles the backpack
		if(Input.GetKeyDown(KeyCode.Tab))
		{
			// Toggle backpack panel 
			backpackPanel.SetActive(!backpackPanel.activeInHierarchy);
			// Set the player's ability to move based on if the panel is open (opposite from each other)
			GetComponent<GameManager>().player.GetComponent<PlayerInfo>().canMove = !backpackPanel.activeInHierarchy;
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
		if(gameObj.GetComponent<Item>() == null)
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
}

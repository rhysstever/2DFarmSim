using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	// Set in inspector
	public float moveSpeed;
	public bool canMove;

	// Set at Start()

	// Start is called before the first frame update
	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	// Update is called once per frame
	void Update()
	{

	}

	// FixedUpdate is called once every fixed framerate frame
	void FixedUpdate()
	{
		if(canMove) {
			BasicMovement();
		}
	}

	/// <summary>
	/// Handles basic forward/backward & turning movement
	/// </summary>
	private void BasicMovement()
	{
		// Sprinting
		if(Input.GetKey(KeyCode.LeftShift))
			moveSpeed = 20.0f;
		else
			moveSpeed = 10.0f;

		// Foward / Backward movement
		float movement = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
		transform.Translate(0, 0, movement);

		// Side movement 
		float movementSide = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
		transform.Translate(movementSide, 0, 0);
	}
}

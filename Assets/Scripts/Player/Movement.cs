using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	// Set in inspector
	public float baseMoveSpeed;
	public bool canMove;

	// Set at Start()
	public bool isSlowed;
	private float currentMoveSpeed;

	// Start is called before the first frame update
	void Start()
	{
		// Scene settings
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	
		isSlowed = false;
		currentMoveSpeed = baseMoveSpeed;
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
			SnapLook();
		}
	}

	/// <summary>
	/// Handles basic forward/backward & turning movement
	/// </summary>
	private void BasicMovement()
	{
		// Reset to base move speed
		currentMoveSpeed = baseMoveSpeed;

		// Sprinting
		if(Input.GetKey(KeyCode.LeftShift))
			currentMoveSpeed *= 2.0f;

		// Being slowed - slowed by 40% (move at 60%)
		if(isSlowed)
			currentMoveSpeed *= 0.6f;

		// Foward / Backward movement
		float movement = Input.GetAxis("Vertical") * Time.deltaTime * currentMoveSpeed;
		transform.Translate(0, 0, movement);

		// Side movement 
		float movementSide = Input.GetAxis("Horizontal") * Time.deltaTime * currentMoveSpeed;
		transform.Translate(movementSide, 0, 0);
	}

	/// <summary>
	/// Snap the player to the direction they are moving in
	/// </summary>
	private void SnapLook()
	{
		// Find the player transform
		// (the transform of the actual player object)
		Transform playerBodyTrans = null;
		for(int c = 0; c < transform.childCount; c++)
		{
			if(transform.GetChild(c).gameObject.tag == "Player")
			{
				playerBodyTrans = transform.GetChild(c);
				break;
			}
		}

		// End early if no tranform was found
		if(playerBodyTrans == null)
			return;

		// Start with the transform's position,
		Vector3 toLookPos = playerBodyTrans.position;

		// Add the corresponding vector, based on the player's input
		// Up
		if(Input.GetAxis("Vertical") > 0)
			toLookPos += Vector3.forward;
		// Down
		else if(Input.GetAxis("Vertical") < 0)
			toLookPos += Vector3.back;

		// Right
		if(Input.GetAxis("Horizontal") > 0)
			toLookPos += Vector3.right;
		// Left
		else if(Input.GetAxis("Horizontal") < 0)
			toLookPos += Vector3.left;

		// Make the transform look at the final vector
		playerBodyTrans.LookAt(toLookPos);

		// Zero out the x and z rotations
		Quaternion newRot = playerBodyTrans.rotation;
		newRot.x = 0.0f;
		newRot.z = 0.0f;
		playerBodyTrans.rotation = newRot;
	}
}

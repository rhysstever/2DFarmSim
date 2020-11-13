using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	public float moveSpeed;
	public float turnSpeed;
	public float jumpAmount;
	public float gravity;
	public bool canMove;
	Vector3 acceleration;

	// Start is called before the first frame update
	void Start()
	{
		moveSpeed = 10.0f;
		turnSpeed = 100.0f;
		jumpAmount = 15.0f;
		canMove = true;
		acceleration = new Vector3();
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
			VerticalMovement();
		}
	}

	/// <summary>
	/// Handles basic forward/backward & turning movement
	/// </summary>
	void BasicMovement()
	{
		// Sprinting - only allowed on the ground
		if(isGrounded()) {
			if(Input.GetKey(KeyCode.LeftShift))
				moveSpeed = 20.0f;
			else
				moveSpeed = 10.0f;
		}

		// Foward / Backward movement
		float movement = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
		transform.Translate(0,0,movement);

		// Turning 
		float turning = Input.GetAxis("Horizontal") * Time.deltaTime * turnSpeed;
		transform.Rotate(0,turning,0);
	}

	/// <summary>
	/// Handles jumping and gravity
	/// </summary>
	void VerticalMovement()
	{
		if(!isGrounded()) {
			acceleration.y -= gravity * Time.deltaTime;
		}
		else {
			gameObject.transform.position = new Vector3(
				gameObject.transform.position.x,
				0.85f,
				gameObject.transform.position.z);

			acceleration = new Vector3();

			float jumpAccel = Input.GetAxis("Jump") * Time.deltaTime * jumpAmount;
			acceleration.y += jumpAccel;
			//transform.Translate(0, jump, 0);
		}

		transform.Translate(acceleration);
	}

	bool isGrounded()
	{
		// Currently, checks if the player is on the same y-value as the ground
		return gameObject.transform.position.y <= 0.85f;
	}
}

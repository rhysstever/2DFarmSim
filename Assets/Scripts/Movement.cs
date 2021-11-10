using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	// Set in inspector
	public float moveSpeed;
	public float jumpAmount;
	public float gravity;
	public bool canMove;
	public bool isGrounded;
	public float horizontalMouseSensitivity;
	public float verticalMouseSensitivity;

	// Set at Start()
	Camera playerCam;
	Vector3 acceleration;
	Vector2 rotation;

	// Start is called before the first frame update
	void Start()
	{
		playerCam = transform.GetComponentInChildren<Camera>();

		acceleration = new Vector3();
		rotation = Vector2.zero;

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
		CheckGroundedness();

		if(canMove) {
			BasicMovement();
			VerticalMovement();
			MouseLook();
		}
	}

	/// <summary>
	/// Handles basic forward/backward & turning movement
	/// </summary>
	private void BasicMovement()
	{
		// Sprinting - only allowed on the ground
		if(isGrounded) {
			if(Input.GetKey(KeyCode.LeftShift))
				moveSpeed = 20.0f;
			else
				moveSpeed = 10.0f;
		}

		// Foward / Backward movement
		float movement = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
		transform.Translate(0, 0, movement);

		// Side movement 
		float movementSide = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
		transform.Translate(movementSide, 0, 0);
	}

	/// <summary>
	/// Handles jumping and gravity
	/// </summary>
	private void VerticalMovement()
	{
		if(!isGrounded) {
			acceleration.y -= gravity * Time.deltaTime;
		}
		else {
			acceleration = new Vector3();

			float jumpAccel = Input.GetAxis("Jump") * Time.deltaTime * jumpAmount;
			acceleration.y += jumpAccel;
		}

		transform.Translate(acceleration);
	}

	/// <summary>
	/// Checks if the player is on the ground
	/// </summary>
	private void CheckGroundedness()
	{
		Vector3 origin = transform.position;
		origin.y -= GetComponent<CapsuleCollider>().height / 2;
		RaycastHit rayHit;
		if(Physics.Raycast(origin, Vector3.down, out rayHit, Mathf.Infinity)) {
			if(LayerMask.LayerToName(rayHit.transform.gameObject.layer) == "Ground") {
				if(rayHit.distance < 0.5f)
					isGrounded = true;
				else
					isGrounded = false;
			}
		}
	}

	/// <summary>
	/// Rotates the player's camera and transform based on mouse position
	/// </summary>
	public void MouseLook()
	{
		// Horizontal mouse look: rotating the player's transform
		rotation.y += Input.GetAxis("Mouse X");
		transform.eulerAngles = new Vector2(0, rotation.y) * horizontalMouseSensitivity;

		// Vertical mouse look: rotating the player camera
		rotation.x += -Input.GetAxis("Mouse Y");
		rotation.x = Mathf.Clamp(rotation.x, -5.0f, 5.0f);
		playerCam.transform.localRotation = Quaternion.Euler(rotation.x * verticalMouseSensitivity, 0, 0);
	}
}

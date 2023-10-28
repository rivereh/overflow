using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

	public float walkSpeed = 5;
	public float runSpeed = 10; 

	float speed;
	public float jumpForce = 5;
	MouseLook mouseLook;
	Rigidbody rb;
	float distToGround;

	void Start () {
		mouseLook = transform.FindChild("Camera").GetComponent<MouseLook>();
		rb = GetComponent<Rigidbody> ();
		distToGround = GetComponent<CapsuleCollider> ().bounds.extents.y;
	}
		

	void FixedUpdate () {
		transform.rotation = Quaternion.Euler (0, mouseLook.yRotation, 0);

		Vector3 moveInput = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));
		Vector3 inputDir = moveInput.normalized;
		Vector3 moveVelocity = moveInput.normalized * speed;
		Vector3 move = transform.TransformDirection (moveVelocity);

		rb.MovePosition (rb.position + move * Time.deltaTime);

		if (Input.GetKeyDown (KeyCode.Space) && isGrounded()) {
			rb.AddForce (new Vector3 (0, jumpForce, 0), ForceMode.Impulse);
		}

		bool running = Input.GetKey (KeyCode.LeftShift);
		speed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;

	}


	bool isGrounded() {
		return Physics.Raycast (transform.position, -Vector3.up, distToGround + 0.2f);
	}



}

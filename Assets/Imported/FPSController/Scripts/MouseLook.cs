using UnityEngine;
using System.Collections;

public class MouseLook : MonoBehaviour {

	public float lookSensitivity = 5;

	[HideInInspector]
	public float yRotation;
	float xRotation;

	void Start () {
	
	}
	
	void LateUpdate () {
		yRotation += Input.GetAxis ("Mouse X") * lookSensitivity;
		xRotation -= Input.GetAxis ("Mouse Y") * lookSensitivity;

		xRotation = Mathf.Clamp (xRotation, -85, 85);

		transform.rotation = Quaternion.Euler (xRotation, yRotation, 0);

	}
}

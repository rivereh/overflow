using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBullet : MonoBehaviour {

	Transform myTransform;

	float speed = 10f;

	bool expended = false;

	RaycastHit hit;

	float range = 1.5f;

	float lifeTime = 5;

	void Start () {
		myTransform = GetComponent<Transform> ();
		Destroy (gameObject, lifeTime);
	}
	
	void Update () {
		myTransform.Translate (Vector3.up * speed * Time.deltaTime);
	}
}

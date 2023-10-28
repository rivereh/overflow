using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(LineRenderer))]
public class Weapon : MonoBehaviour {

	public enum GunType {Semi, Burst, Auto};
	public GunType gunType;

	public int gunDamage = 1;
	public float fireRate = 0.25f;
	public float weaponRange = 100f;
	public float hitForce = 100f;
	public Transform gunEnd;

	//public Vector3 hipHoldPos;
	//public Vector3 aimHoldPos;
	//public float zoomedInFOV = 40;
	//[HideInInspector]
	//public float defaultFOV;

	[HideInInspector]
	public Camera myCam;
	WaitForSeconds shotDuration = new WaitForSeconds (0.02f);
	AudioSource gunAudio;
	LineRenderer gunLine;

	public bool beingHeld = false;
	int countToThrow = -1;

	float throwGunUpForce = 100;
	float throwGunFowardForce = 300;


	public void CmdOnPickup () {
		RpcPickup ();
	}


	void RpcPickup () {

		Debug.Log ("ye");

		beingHeld = true;
		GetComponent<Rigidbody> ().useGravity = false;
		GetComponent<BoxCollider> ().enabled = false;
		GetComponent<Rigidbody> ().isKinematic = true;
	}


	float nextFire;

	void Start () {
		//gunLine = GetComponent<LineRenderer> ();
		gunLine = GetComponent<LineRenderer> ();
		gunAudio = GetComponent<AudioSource> ();
	}
	
	void Update () {



		if (beingHeld) {
			//timer += Time.deltaTime;
			//if (timer >= timeBetweenShots * effectsDisplayTime) {
				//DisableEffects ();
			//}

			/*if (Input.GetKeyDown (KeyCode.Mouse1)) {
				transform.parent.localPosition = aimHoldPos;
				myCam.fieldOfView = zoomedInFOV;
			}

			if (Input.GetKeyUp (KeyCode.Mouse1)) {
				transform.parent.localPosition = hipHoldPos;
				myCam.fieldOfView = defaultFOV;
			}*/
		}

	}

	public void Pickup () {
		beingHeld = true;
		GetComponent<Rigidbody> ().useGravity = false;
		GetComponent<BoxCollider> ().enabled = false;
		GetComponent<Rigidbody> ().isKinematic = true;
	}

	public void Shoot () {



		if (Time.time > nextFire) {

			nextFire = Time.time + fireRate;

			StartCoroutine (ShotEffect ());

			Vector3 rayOrigin = myCam.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, 0.0f));

			RaycastHit hit;

			gunLine.SetPosition (0, gunEnd.position);

			if (Physics.Raycast (rayOrigin, myCam.transform.forward, out hit, weaponRange)) {
				gunLine.SetPosition (1, hit.point);
			} else {
				gunLine.SetPosition (1, rayOrigin + (myCam.transform.forward * weaponRange));
			}



		}

	}


	public void CmdOnDrop () {
		RpcDrop ();
	}

	// called on clients for shoot effect

	void RpcDrop () {

		Debug.Log ("ye");


		GetComponent<Rigidbody> ().useGravity = true;
		GetComponent<BoxCollider> ().enabled = true;
		GetComponent<Rigidbody> ().isKinematic = false;

		GetComponent<Rigidbody> ().AddRelativeForce (0, throwGunUpForce, throwGunFowardForce);
		beingHeld = false;

		transform.parent = null;
	}


	public void DropWeapon () {

		GetComponent<Rigidbody> ().useGravity = true;
		GetComponent<BoxCollider> ().enabled = true;
		GetComponent<Rigidbody> ().isKinematic = false;

		GetComponent<Rigidbody> ().AddRelativeForce (0, throwGunUpForce, throwGunFowardForce);
		beingHeld = false;

		transform.parent = null;

	}

	IEnumerator ShotEffect () {
		gunAudio.Play ();
		gunLine.enabled = true;
		yield return shotDuration;
		gunLine.enabled = false;
	}

	public void ShootContinuous () {
		if (gunType == GunType.Auto) {
			Shoot ();
		}
	}
		



}

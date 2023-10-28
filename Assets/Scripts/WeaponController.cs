using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponController : NetworkBehaviour {

	[SerializeField] string weaponLayerName = "Weapon";

	public Transform weaponHold;
	//public Weapon startingWeapon;
	Weapon equippedWeapon;

	[SerializeField] Weapon startingWeapon;

	int pickupWepDistance = 6;
	float throwGunUpForce = 100;
	float throwGunFowardForce = 300;


	Camera myCam;

	void Start () {

		// Checks to see if starting weapon is not empty;
		// If there's a weapon assigned then equip it
		//if (startingWeapon != null) {
		//	EquipWeapon (startingWeapon);
		//}



		if (isLocalPlayer) {
			myCam = GetComponentInChildren<Camera> ();
			CmdSpawn ();
		}
	}




	void Update () {

		if (!isLocalPlayer)
			return;




		/*Vector3 gunRay = myCam.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));
		RaycastHit gunRayHit;
		if (Physics.Raycast (gunRay, myCam.transform.forward, out gunRayHit, 100f)) {
			if (gunRayHit.transform.tag == "Weapon") {
				if (Input.GetKeyDown (KeyCode.E)) {
					EquipWeapon (gunRayHit.transform.GetComponent<Weapon>());
				}

			}
		}

		if (equippedWeapon != null) {
			if (Input.GetKeyDown (KeyCode.Q)) {
				equippedWeapon.GetComponent<NetworkIdentity> ().RemoveClientAuthority (this.GetComponent<NetworkIdentity>().connectionToClient);
				equippedWeapon.DropWeapon ();
				equippedWeapon = null;
			}
		}*/

	}

	public GameObject gunObject;

	[Command]
	void CmdSpawn () {
		var go = (GameObject)Instantiate (gunObject, weaponHold.transform.position, weaponHold.transform.rotation);
		go.transform.SetParent(weaponHold);
		NetworkServer.Spawn (go);
	}



	void SetLayerRecursively(GameObject obj, string newLayer) {
		if (null == obj) {
			return;
		}
		obj.layer = LayerMask.NameToLayer (newLayer);

		foreach (Transform child in obj.transform) {
			if (null == child) {
				continue;
			}
			SetLayerRecursively(child.gameObject, newLayer);
		}
	}


	void OnGUI () {

		if (!isLocalPlayer)
			return;

		Vector3 gunRay = myCam.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));
		RaycastHit gunRayHit;

		if (Physics.Raycast (gunRay, myCam.transform.forward, out gunRayHit, 100f)) {
			if (gunRayHit.transform.tag == "Weapon") {
				GUI.Label (new Rect (10, 10, 100, 20), gunRayHit.transform.name);

				//if (Input.GetKeyDown (KeyCode.E) && equippedWeapon != null) {
				//equippedWeapon.DropWeapon ();
				//equippedWeapon = null;
				//EquipWeapon (gunRayHit.transform.GetComponent<Weapon> ());
				//}

			}
		}
	}




	// OLD SHIT THAT DIDN'T WORK


	/*[SerializeField] string weaponLayerName = "Weapon";

	public Transform weaponHold;
	//public Weapon startingWeapon;
	Weapon equippedWeapon;

	[SerializeField] Weapon startingWeapon;

	int pickupWepDistance = 6;
	float throwGunUpForce = 100;
	float throwGunFowardForce = 300;


	Camera myCam;


	void Start () {

		// Checks to see if starting weapon is not empty;
		// If there's a weapon assigned then equip it
		//if (startingWeapon != null) {
			//EquipWeapon (startingWeapon);
		//}
		myCam = GetComponentInChildren<Camera> ();

	}
	
	void Update () {

		if (!isLocalPlayer)
			return;


		if (equippedWeapon) {
			if (Input.GetButtonDown ("Fire1")) {
				equippedWeapon.Shoot ();
			} else if (Input.GetButton ("Fire1")) {
				equippedWeapon.ShootContinuous ();
			}
		}

		Vector3 gunRay = myCam.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));
		RaycastHit gunRayHit;
		if (Physics.Raycast (gunRay, myCam.transform.forward, out gunRayHit, 100f)) {
			if (gunRayHit.transform.tag == "Weapon") {
				if (Input.GetKeyDown (KeyCode.E)) {
					EquipWeapon (gunRayHit.transform.GetComponent<Weapon>());
				}

			}
		}

		if (equippedWeapon != null) {
			if (Input.GetKeyDown (KeyCode.Q)) {
				equippedWeapon.GetComponent<NetworkIdentity> ().RemoveClientAuthority (this.GetComponent<NetworkIdentity>().connectionToClient);
				equippedWeapon.DropWeapon ();
				equippedWeapon = null;
			}
		}

	}	


	// The EquipWeapon will first check if there is already a weapon equipped;
	// if so it will destory the current weapon and then instantiate the new weapon
	// at the weaponHold's position and set the new weapons parent to the weaponHold
	public void EquipWeapon (Weapon weaponToEquipped) {

		if (!isLocalPlayer)
			return;


		if (equippedWeapon != null) {
			//Destroy (equippedWeapon.gameObject);
			equippedWeapon.GetComponent<NetworkIdentity> ().RemoveClientAuthority (this.GetComponent<NetworkIdentity>().connectionToClient);
			equippedWeapon.DropWeapon ();
			equippedWeapon = null;
		}
		//equippedWeapon = Instantiate (weaponToEquipped, weaponHold.position, weaponHold.rotation) as Weapon;
		equippedWeapon = weaponToEquipped;

		//equippedWeapon.transform.GetComponent<SyncPos> ().enabled = false;
		//equippedWeapon.transform.GetComponent<SyncRot> ().enabled = false;

		equippedWeapon.transform.position = weaponHold.position;
		equippedWeapon.transform.rotation = weaponHold.rotation;
		equippedWeapon.transform.parent = weaponHold;
		equippedWeapon.myCam = GetComponentInChildren<Camera>();
		equippedWeapon.GetComponent<NetworkIdentity> ().AssignClientAuthority (this.GetComponent<NetworkIdentity>().connectionToClient);
		//equippedWeapon.hipHoldPos = equippedWeapon.transform.parent.localPosition;
		equippedWeapon.Pickup ();
		//equippedWeapon.defaultFOV = GetComponentInChildren<Camera> ().fieldOfView;
			//_weaponIns.layer = LayerMask.NameToLayer (weaponLayerName);
		SetLayerRecursively(weaponToEquipped.gameObject, weaponLayerName);
	}


	[Command]
	public void CmdOnPickup () {


		RpcPickup ();
	}

	// called on clients for shoot effect
	[ClientRpc]
	void RpcPickup () {

		equippedWeapon.Pickup ();
	}

	[Command]
	public void CmdOnDrop () {

		RpcDrop ();
	}

	// called on clients for shoot effect
	[ClientRpc]
	void RpcDrop () {


		equippedWeapon.DropWeapon ();
		equippedWeapon = null;
	}



	void SetLayerRecursively(GameObject obj, string newLayer) {
		if (null == obj) {
			return;
		}
		obj.layer = LayerMask.NameToLayer (newLayer);

		foreach (Transform child in obj.transform) {
			if (null == child) {
				continue;
			}
			SetLayerRecursively(child.gameObject, newLayer);
		}
	}


	void OnGUI () {

		if (!isLocalPlayer)
			return;

		Vector3 gunRay = myCam.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));
		RaycastHit gunRayHit;

		if (Physics.Raycast (gunRay, myCam.transform.forward, out gunRayHit, 100f)) {
			if (gunRayHit.transform.tag == "Weapon") {
				GUI.Label (new Rect (10, 10, 100, 20), gunRayHit.transform.name);

				//if (Input.GetKeyDown (KeyCode.E) && equippedWeapon != null) {
					//equippedWeapon.DropWeapon ();
					//equippedWeapon = null;
					//EquipWeapon (gunRayHit.transform.GetComponent<Weapon> ());
				//}

			}
		}
	}*/

}

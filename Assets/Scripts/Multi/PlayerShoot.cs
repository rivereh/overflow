using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour {

	const string PLAYER_TAG = "Player";

	//[SerializeField] GameObject weaponGFX;
	//[SerializeField] string weaponLayerName = "Weapon";

	[SerializeField] Camera cam;

	[SerializeField] LayerMask mask;

	PlayerWeapon currentWeapon;

	WeaponManager weaponManager;

	WaitForSeconds shotDuration = new WaitForSeconds (0.02f);
	//AudioSource gunAudio;
	LineRenderer gunLine;

	[SerializeField] Transform weaponEnd;
	
	void Start () {
		if (cam == null) {
			Debug.LogError ("PlayerShoot: No camera referenced!");
			this.enabled = false;
		}

		//weaponGFX.layer = LayerMask.NameToLayer (weaponLayerName);
		//SetLayerRecursively(weaponGFX, weaponLayerName);
		weaponManager = GetComponent<WeaponManager> ();
		gunLine = GetComponent<LineRenderer> ();
		//gunAudio = GetComponent<AudioSource> ();
	}

//	void SetLayerRecursively(GameObject obj, string newLayer) {
//		if (null == obj) {
//			return;
//		}
//		obj.layer = LayerMask.NameToLayer (newLayer);
//
//		foreach (Transform child in obj.transform) {
//			if (null == child) {
//				continue;
//			}
//			SetLayerRecursively(child.gameObject, newLayer);
//		}
//	}

	void Update () {
		currentWeapon = weaponManager.GetCurrentWeapon ();

		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			Shoot ();
		}
	}

	// called on server when player shoots
	[Command]
	void CmdOnShoot () {
		RpcShootEffect ();
	}

	// called on clients for shoot effect
	[ClientRpc]
	void RpcShootEffect () {
		weaponManager.GetCurrentGraphics ().muzzleFlash.Play ();
		gunLine.SetPosition (0, weaponEnd.transform.position);
		StartCoroutine (ShotEffect ());
	}

	[Command]
	void CmdOnHit (Vector3 _pos, Vector3 _normal) {
		RpcHitEffect (_pos, _normal);
	}

	[ClientRpc]
	void RpcHitEffect (Vector3 _pos, Vector3 _normal) {
		GameObject _hitEffect = (GameObject)Instantiate (weaponManager.GetCurrentGraphics ().hitEffectPrefab, _pos, Quaternion.LookRotation (_normal));
		gunLine.SetPosition (1, _pos);
		Destroy (_hitEffect, 2f);
	}

	IEnumerator ShotEffect () {
		//gunAudio.Play ();
		gunLine.enabled = true;
		yield return shotDuration;
		gunLine.enabled = false;
	}
		
	[Client]
	void Shoot () {
		if (!isLocalPlayer) {
			return;
		}
		Vector3 rayOrigin = cam.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, 0.0f));
		CmdOnShoot ();

		RaycastHit _hit;
		if (Physics.Raycast (cam.transform.position, cam.transform.forward, out _hit, currentWeapon.range, mask)) {
			if (_hit.collider.tag == PLAYER_TAG) {
				CmdPlayerShot (_hit.collider.name, currentWeapon.damage);
			}
			CmdOnHit (_hit.point, _hit.normal);
		}else {
			gunLine.SetPosition (1, rayOrigin + (cam.transform.forward * 100));
		}

	}

	[Command]
	void CmdPlayerShot (string _playerID, int _damage) {
		//Debug.Log (_ID + " has been shot.");

		Player _player = GameManager.GetPlayer (_playerID);
		_player.RpcTakeDamage (_damage);

	}

}

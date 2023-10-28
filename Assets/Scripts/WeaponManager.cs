using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour {

	[SerializeField] string weaponLayerName = "Weapon";

	[HideInInspector]
	public Transform weaponHold;

	[SerializeField] PlayerWeapon primaryWeapon;

	PlayerWeapon currentWeapon;
	WeaponGraphics currentGraphics;

	void Start () {
		EquipWeapon (primaryWeapon);
	}

	public PlayerWeapon GetCurrentWeapon () {
		return currentWeapon;
	}

	public WeaponGraphics GetCurrentGraphics () {
		return currentGraphics;
	}

	void EquipWeapon (PlayerWeapon _weapon) {
		currentWeapon = _weapon;
		GameObject _weaponIns = Instantiate (_weapon.graphics, weaponHold.position, weaponHold.rotation) as GameObject;
		_weaponIns.transform.SetParent (weaponHold);

		currentGraphics = _weaponIns.GetComponent<WeaponGraphics> ();
		if (currentGraphics == null) {
			Debug.LogError ("No Graphics component on weapon: " + _weaponIns.name);
		}

		if (isLocalPlayer) {
			//_weaponIns.layer = LayerMask.NameToLayer (weaponLayerName);
			SetLayerRecursively(_weaponIns, weaponLayerName);
		}
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

}

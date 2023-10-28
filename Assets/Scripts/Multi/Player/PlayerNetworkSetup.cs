using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerNetworkSetup : NetworkBehaviour {

	[SerializeField] Behaviour[] componentsToDisable;

	string remoteLayerName = "RemotePlayer";

	Camera sceneCamara;

	[SerializeField] GameObject playerUIPrefab;
	GameObject playerUIInstance;

	void Start () {
		if (!isLocalPlayer) {
			DisableComponents ();
			AssignRemoteLayer ();
		} else {
			sceneCamara = Camera.main;
			if (sceneCamara != null) {
				sceneCamara.gameObject.SetActive (false);
			}

			playerUIInstance = Instantiate (playerUIPrefab);
			playerUIInstance.name = playerUIPrefab.name;

		}

		GetComponent<Player> ().Setup ();

	}

	public override void OnStartClient () {
		base.OnStartClient ();

		string _netID = GetComponent<NetworkIdentity> ().netId.ToString();
		Player _player = GetComponent<Player> ();

		GameManager.RegisterPlayer (_netID, _player);
	}

	void AssignRemoteLayer () {
		gameObject.layer = LayerMask.NameToLayer (remoteLayerName);
	}

	void DisableComponents () {
		for (int i = 0; i < componentsToDisable.Length; i++) {
			componentsToDisable [i].enabled = false;
		}
	}

	void OnDisable () {
		Destroy (playerUIInstance);

		if (sceneCamara != null) {
			sceneCamara.gameObject.SetActive (true);
		}

		GameManager.UnRegisterPlayer (transform.name);
	}
}

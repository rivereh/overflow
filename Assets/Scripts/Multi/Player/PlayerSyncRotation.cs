using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSyncRotation : NetworkBehaviour {

	[SyncVar] Quaternion syncPlayerRotation;
	[SyncVar] Quaternion syncCamRotation;

	[SerializeField] Transform playerTransform;
	[SerializeField] Transform camTranform;
	[SerializeField] float lerpRate = 15;

	void FixedUpdate () {
		TransmitRotations ();
		LerpRotations ();
	}

	void LerpRotations () {
		if (!isLocalPlayer) {
			playerTransform.rotation = Quaternion.Lerp (playerTransform.rotation, syncPlayerRotation, Time.deltaTime * lerpRate);
			camTranform.rotation = Quaternion.Lerp (camTranform.rotation, syncCamRotation, Time.deltaTime * lerpRate);
		}
	}

	[Command]
	void CmdSendRotationsToServer (Quaternion _playerRot, Quaternion _camRot) {
		syncPlayerRotation = _playerRot;
		syncCamRotation = _camRot;
	}

	[Client]
	void TransmitRotations () {
		if (isLocalPlayer) {
			CmdSendRotationsToServer (playerTransform.rotation, camTranform.rotation);
		}
	}

}

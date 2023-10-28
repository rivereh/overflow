using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SyncMovement : NetworkBehaviour {

	[SyncVar] Vector3 syncPos;
	[SyncVar] Quaternion syncRot;

	[SerializeField] Transform myTransform;
	[SerializeField] float lerpRate = 15;
	
	void Update () {
		TransmitPosition ();
		LerpPosition ();
		TransmitRotations ();
		LerpRotations ();
	}

	void LerpPosition () {
		if (!isLocalPlayer) {
			myTransform.position = Vector3.Lerp (myTransform.position, syncPos, Time.deltaTime * lerpRate);
		}
	}

	[Command]
	void CmdSendPositionToServer (Vector3 _pos) {
		syncPos = _pos;
	}

	[Client]
	void TransmitPosition () {
		if (isLocalPlayer) {
			CmdSendPositionToServer (myTransform.position);
		}
	}

	void LerpRotations () {
		if (!isLocalPlayer) {
			myTransform.rotation = Quaternion.Lerp (myTransform.rotation, syncRot, Time.deltaTime * lerpRate);
		}
	}

	[Command]
	void CmdSendRotationsToServer (Quaternion _playerRot) {
		myTransform.rotation = _playerRot;
	}

	[Client]
	void TransmitRotations () {
		if (isLocalPlayer) {
			CmdSendRotationsToServer (myTransform.rotation);
		}
	}

}

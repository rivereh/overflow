using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSyncPosition : NetworkBehaviour {

	[SyncVar] Vector3 syncPos;

	[SerializeField] Transform myTransform;
	[SerializeField] float lerpRate = 15;

	void FixedUpdate () {
		TransmitPosition ();
		LerpPosition ();
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
}

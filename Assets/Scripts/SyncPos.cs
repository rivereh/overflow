using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SyncPos : NetworkBehaviour {

	[SyncVar] Vector3 syncPos;
	[SerializeField] Transform myTransform;
	[SerializeField] float lerpRate = 15;
	
	void FixedUpdate () {
		TransmitPosition ();
		LerpPosition ();
	}

	void LerpPosition () {
		if (!hasAuthority) {
			myTransform.position = Vector3.Lerp (myTransform.position, syncPos, Time.deltaTime * lerpRate);
		}
	}

	[Command]
	void CmdProvidePositionToServer (Vector3 pos) {
		syncPos = pos;
	}

	[ClientCallback]
	void TransmitPosition () {
		if (hasAuthority) {
			CmdProvidePositionToServer (myTransform.position);
		}
	}
}

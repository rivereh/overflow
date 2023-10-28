using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SyncRot : NetworkBehaviour {

	[SyncVar] Quaternion syncRot;
	[SerializeField] Transform myTransform;
	[SerializeField] float lerpRate = 15;
	
	void FixedUpdate () {
		SendRot ();
		LerpRot ();
	}

	void LerpRot () {
		if (!isLocalPlayer) {
			myTransform.rotation = Quaternion.Lerp (myTransform.rotation, syncRot, Time.deltaTime * lerpRate);
		}
	}

	[Command]
	void CmdProvideRotToServer (Quaternion rot) {
		syncRot = rot;
	}

	[Client]
	void SendRot () {
		if (isLocalPlayer) {
			CmdProvideRotToServer (myTransform.rotation);
		}
	}
}

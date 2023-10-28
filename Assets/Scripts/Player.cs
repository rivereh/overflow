using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

	[SyncVar] bool _isDead = false;
	public bool isDead {
		get { return _isDead; }
		protected set { _isDead = value; }
	}

	int maxHealth = 100;
	[SyncVar] [SerializeField] int currentHealth;

	[SerializeField] Behaviour[] disableOnDeath;
	bool[] wasEnabled;

	public void Setup () {
		wasEnabled = new bool[disableOnDeath.Length];
		for (int i = 0; i < wasEnabled.Length; i++) {
			wasEnabled [i] = disableOnDeath [i].enabled;
		}

		SetDefaults ();
	}

	[ClientRpc]
	public void RpcTakeDamage (int _amount) {
		if (isDead)
			return;

		currentHealth -= _amount;

		Debug.Log (transform.name + " now has " + currentHealth + " health");

		if (currentHealth <= 0) {
			Die ();
		}
	} 

	void Die () {
		isDead = true;

		for (int i = 0; i < disableOnDeath.Length; i++) {
			disableOnDeath [i].enabled = false;
		}

//		Collider _col = GetComponent<Collider> ();
//		if (_col != null)
//			_col.enabled = false;

		Debug.Log(transform.name + " is DEAD");

		// CALL RESPAWN METHOD
		StartCoroutine(Respawn());
	}

	IEnumerator Respawn () {
		yield return new WaitForSeconds (3);

		SetDefaults ();
		Transform _startPoint = NetworkManager.singleton.GetStartPosition ();
		transform.position = _startPoint.position;
		transform.rotation = _startPoint.rotation;

	}

	public void SetDefaults () {
		isDead = false;

		currentHealth = maxHealth;

		for (int i = 0; i < disableOnDeath.Length; i++) {
			disableOnDeath [i].enabled = wasEnabled [i];
		}

//		Collider _col = GetComponent<Collider> ();
//		if (_col != null)
//			_col.enabled = true;

	}
	
	void Update () {
		
	}

	void OnGUI () {
		GUILayout.BeginArea (new Rect (200, 200, 200, 500));
		GUILayout.BeginVertical ();
		if (isDead && isLocalPlayer) {
			GUILayout.Label ("You died! Respawning...");
		}
		GUILayout.EndVertical ();
		GUILayout.EndArea ();
	}

}

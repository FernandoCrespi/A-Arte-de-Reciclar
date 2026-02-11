using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestrutorPorTempo : MonoBehaviour {

	public float tempo = 1.0f;
	public bool liberaFilhos = false;

	// destroi o objeto após tempo segundos
	void Awake () {
		Invoke ("DestroyNow", tempo);
	}

	// destroi o gameobject
	void DestroyNow ()
	{
		if (liberaFilhos) { // libera os filhos para que não sejam destriuídos juntos, se for o caso.
			transform.DetachChildren ();
		}

		// destroi o gameobject
		Destroy(gameObject);
	}

}

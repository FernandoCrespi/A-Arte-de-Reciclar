using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAtaqueInimigo : MonoBehaviour {

	public int dano = 5;
	public string tagInimigo = "Player";
	public GameObject inimigo;

	void OnTriggerEnter2D (Collider2D outro)
	{
		if ((outro.tag == tagInimigo ) && (!outro.gameObject.GetComponent<Saude>().morto))
		{
			inimigo.GetComponent<IAInimigoRonda> ().ataca ();
		}
	}

}
